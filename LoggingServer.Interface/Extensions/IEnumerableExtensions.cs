using System;
using System.Collections.Generic;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Tasks;
using MvcContrib.Pagination;

namespace LoggingServer.Interface.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IPagination<T> ToPagination<T>(this IEnumerable<T> source, int pageNumber, int totalItems)
        {
            return ToPagination(source, pageNumber, LogEntryTasks.DefaultPageSize, totalItems);
        }

        public static IPagination<T> ToPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize, int totalItems)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
            return new Pagination<T>(source, pageNumber, pageSize, totalItems);
        }
    }
}