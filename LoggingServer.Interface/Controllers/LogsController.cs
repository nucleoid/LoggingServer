using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MvcContrib.UI.Grid;

namespace LoggingServer.Interface.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogEntryTasks _logEntryTasks;
        private readonly IReadableRepository<Project> _projectRepository;
        private readonly IReadableRepository<SearchFilter> _searchFilterRepository;

        public LogsController(ILogEntryTasks logEntryTasks, IReadableRepository<Project> projectRepository, IReadableRepository<SearchFilter> searchFilterRepository)
        {
            _logEntryTasks = logEntryTasks;
            _projectRepository = projectRepository;
            _searchFilterRepository = searchFilterRepository;
        }

        public ActionResult Index(SearchFilter filter, GridSortOptions gridSortOptions, int? page)
        {
            var projects = _projectRepository.All().ToList();
            return View(projects);
        }

        public ActionResult Search(SearchFilter filter, GridSortOptions gridSortOptions, int? page)
        {
            var results = _logEntryTasks.Paged(page, null /*default pageSize*/, filter);
            var totalCount = _logEntryTasks.Count(filter);
            var pagedModel = new PagedModel<LogEntry, LogEntrySummaryModel>
            {
                ViewData = ViewData,
                Query = results,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "DateAdded",
                Page = page,
                Total = totalCount
            }.Setup();
            ViewData["filter.LogLevel"] = filter.LogLevel;
            return View(pagedModel);
        }

        public ActionResult SavedSearch(Guid id, int? page)
        {
            var filter = _searchFilterRepository.Get(id);
            var results = _logEntryTasks.Paged(page, null /*default pageSize*/, filter);
            var totalCount = _logEntryTasks.Count(filter);
            var pagedModel = new PagedModel<LogEntry, LogEntrySummaryModel>
            {
                ViewData = ViewData,
                Query = results,
                GridSortOptions = new GridSortOptions(),
                DefaultSortColumn = "DateAdded",
                Page = page,
                Total = totalCount
            }.Setup();
            AddFilterToViewData(filter);
            return View("Search", pagedModel);
        }

        public ActionResult Details(Guid id)
        {
            var log = _logEntryTasks.Get(id);
            var model = Mapper.Map<LogEntry, LogEntryModel>(log);
            return View(model);
        }

        private void AddFilterToViewData(SearchFilter filter)
        {
            if (filter == null)
                return;
            ViewData["filter.LogLevel"] = filter.LogLevel;
            ViewData["filter.ProjectName"] = filter.ProjectName;
            ViewData["filter.ComponentName"] = filter.ComponentName;
            ViewData["filter.StartDate"] = filter.StartDate;
            ViewData["filter.EndDate"] = filter.EndDate;
            ViewData["filter.MachineNamePartial"] = filter.MachineNamePartial;
            ViewData["filter.ExceptionPartial"] = filter.ExceptionPartial;
            ViewData["filter.MessagePartial"] = filter.MessagePartial;
        }
    }
}
