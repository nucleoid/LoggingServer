using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;

namespace LoggingServer.Server.Tasks
{
    public class SubscriptionTasks : ISubscriptionTasks
    {
        private delegate void NotifyDelegate(IList<LogEntry> data, bool isDaily);

        private readonly NotifyDelegate _worker;
        private readonly AsyncCallback _completedCallback;
        private readonly IReadableRepository<Subscription> _subscriptionRepository;
        private readonly IEmailTasks _emailTasks;
        private readonly ILogEntryTasks _logEntryTasks;

        public SubscriptionTasks(IReadableRepository<Subscription> subscriptionRepository, IEmailTasks emailTasks, ILogEntryTasks logEntryTasks)
        {
            _worker = Notify;
            _completedCallback = TaskCompletedCallback;
            _subscriptionRepository = subscriptionRepository;
            _emailTasks = emailTasks;
            _logEntryTasks = logEntryTasks;
        }

        public event AsyncCompletedEventHandler TaskCompleted;

        public void AsyncNotify(IList<LogEntry> data, bool isDaily)
        {
            AsyncOperation async = AsyncOperationManager.CreateOperation(null);
            _worker.BeginInvoke(data, isDaily, _completedCallback, async);
        }

        public void Notify(IList<LogEntry> data, bool isDaily)
        {
            var subscriptions = _subscriptionRepository.All().Where(x => x.IsDailyOverview == isDaily).ToList();
            foreach (var subscription in subscriptions)
            {
                var count = SubscriptionMatchCount(data, subscription);
                if (count > 0)
                {
                    var client = _emailTasks.GenerateClient();
                    var message = GenerateMessage(subscription, count);
                    _emailTasks.SendEmail(client, message);
                }
            }
        }

        private MailMessage GenerateMessage(Subscription subscription, int logCount)
        {
            var from = ConfigurationManager.AppSettings["subscriptionFromAddress"];
            var subject = subscription.IsDailyOverview ? string.Format("Daily Log Subscription: {0}", subscription.Filter.Description) : 
                string.Format("Log Subscription: {0}", subscription.Filter.Description);
            var filterLink = string.Format(ConfigurationManager.AppSettings["filterLink"], subscription.Filter.ID);
            var body = subscription.IsDailyOverview ? string.Format("<a href=\"{0}\">Log Count: {1}</a>", filterLink, logCount) : 
                string.Format("<a href=\"{0}\">New matching log</a>", filterLink);
            var message = _emailTasks.GenerateMessage(from, subscription.EmailList, subject, body);
            return message;
        }

        private int SubscriptionMatchCount(IEnumerable<LogEntry> data, Subscription subscription)
        {
            var checkData = new List<LogEntry>(data).AsQueryable();
            return _logEntryTasks.AddFilterToQuery(subscription.Filter, checkData).Count();
        }

        private void TaskCompletedCallback(IAsyncResult ar)
        {
            var worker = (NotifyDelegate)((AsyncResult)ar).AsyncDelegate;
            var async = (AsyncOperation)ar.AsyncState;
            worker.EndInvoke(ar);

            var completedArgs = new AsyncCompletedEventArgs(null, false, null);
            async.PostOperationCompleted(e => OnTaskCompleted((AsyncCompletedEventArgs) e), completedArgs);
        }

        protected virtual void OnTaskCompleted(AsyncCompletedEventArgs e)
        {
            if (TaskCompleted != null)
                TaskCompleted(this, e);
        }
    }
}
