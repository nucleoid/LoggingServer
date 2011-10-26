using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Interface.Extensions;
using LoggingServer.Server.Tasks;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;

namespace LoggingServer.Interface.Models
{
    /// <summary>
    /// Modified from http://www.codeproject.com/KB/aspnet/Grid_Paging_In_MVC3.aspx
    /// </summary>
    public class PagedModel<T, TK>
    {
        public ViewDataDictionary ViewData { get; set; }
        public IQueryable<T> Query { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
        public string DefaultSortColumn { get; set; }
        public IPagination<TK> PagedList { get; private set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? Total { get; set; }
   
        public PagedModel<T, TK> Setup()
        {
            if (string.IsNullOrWhiteSpace(GridSortOptions.Column))
                GridSortOptions.Column = DefaultSortColumn;

            PagedList = Query.ToList()
                        .Select(Mapper.Map<T, TK>)
                        .ToPagination(Page ?? 1, PageSize ?? LogEntryTasks.DefaultPageSize, Total ?? Query.Count());
            
            return this;
        }
    }
}