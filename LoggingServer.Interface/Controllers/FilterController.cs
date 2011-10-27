using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MvcContrib;

namespace LoggingServer.Interface.Controllers
{
    public class FilterController : Controller
    {
        private readonly IWritableRepository<SearchFilter> _searchFilterRepository;

        public FilterController(IWritableRepository<SearchFilter> searchFilterRepository)
        {
            _searchFilterRepository = searchFilterRepository;
        }

        public ActionResult Index()
        {
            IList<SearchFilter> filters = _searchFilterRepository.All().Where(x => x.UserName == User.Identity.Name || x.IsGlobal).ToList();
            var mapped = Mapper.Map<IList<SearchFilter>, IList<SearchFilterModel>>(filters);
            return View(mapped);
        }

        [HttpPost]
        public ActionResult Create(SearchFilterModel filter)
        {
            if (ModelState.IsValid)
            {
                var mapped = Mapper.Map<SearchFilterModel, SearchFilter>(filter);
                _searchFilterRepository.Save(mapped);
                if(filter.IsGlobal)
                    return new ContentResult { Content = "Save as global successful!" };
                return new ContentResult {Content = "Save successful!"};
            }

            return new ContentResult { Content = "Save failure, invalid fields." };
        }

        public ActionResult Edit(Guid id)
        {
            var filter = _searchFilterRepository.Get(id);
            var mapped = Mapper.Map<SearchFilter, SearchFilterModel>(filter);
            return View(mapped);
        }

        [HttpPost]
        public ActionResult Edit(SearchFilterModel filter)
        {
            if (ModelState.IsValid)
            {
                var mapped = Mapper.Map<SearchFilterModel, SearchFilter>(filter);
                _searchFilterRepository.Save(mapped);
                return this.RedirectToAction(x => x.Index());
            }

            return View(filter);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var filter = _searchFilterRepository.Get(id);
            if(filter != null)
                _searchFilterRepository.Delete(filter);
            return RedirectToAction("Index");
        }
    }
}
