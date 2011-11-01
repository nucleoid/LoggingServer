using System.Linq;
using System.Web.Mvc;

namespace LoggingServer.Interface.Extensions
{
    public static class IQueryableExtensions
    {
        public static SelectList ToSelectList<T>(this IQueryable<T> query, string dataValueField, string dataTextField, object selectedValue)
        {
            return new SelectList(query, dataValueField, dataTextField, selectedValue ?? -1);
        }
    }
}