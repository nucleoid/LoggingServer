using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Interface.Extensions;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MvcContrib;

namespace LoggingServer.Interface.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly IWritableRepository<Subscription> _subscriptionRepository;
        private static IWritableRepository<SearchFilter> _filterRepository;

        public SubscriptionsController(IWritableRepository<Subscription> subscriptionRepository, IWritableRepository<SearchFilter> filterRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _filterRepository = filterRepository;
        }

        public ActionResult Index()
        {
            var subscriptions = _subscriptionRepository.All().ToList();
            var mapped = Mapper.Map<IList<Subscription>, IList<SubscriptionModel>>(subscriptions);
            return View(mapped);
        }

        public ActionResult Edit(Guid id)
        {
            var subscription = new Subscription();
            if(Guid.Empty != id)
                subscription = _subscriptionRepository.Get(id);
            var mapped = Mapper.Map<Subscription, SubscriptionModel>(subscription);
            ViewBag.FilterId = _filterRepository.All().Where(x => x.UserName == User.Identity.Name || x.IsGlobal)
                .ToSelectList("ID", "Description", subscription.Filter != null ? subscription.Filter.ID : Guid.Empty);
            return View(mapped);
        }

        [HttpPost]
        public ActionResult Edit(SubscriptionModel subscription)
        {
            if (ModelState.IsValid)
            {
                var mapped = Mapper.Map<SubscriptionModel, Subscription>(subscription);
                _subscriptionRepository.Save(mapped);
                return this.RedirectToAction(x => x.Index());
            }
            ViewBag.FilterId = _filterRepository.All().Where(x => x.UserName == User.Identity.Name || x.IsGlobal)
                .ToSelectList("ID", "Description", subscription.FilterId);
            return View(subscription);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var subscription = _subscriptionRepository.Get(id);
            if (subscription != null)
                _subscriptionRepository.Delete(subscription);
            return RedirectToAction("Index");
        }
    }
}
