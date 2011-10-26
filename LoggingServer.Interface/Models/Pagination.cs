using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.Pagination;

namespace LoggingServer.Interface.Models
{
    public class Pagination<T> : IPagination<T>
    {
        private readonly IEnumerable<T> _items;

        public Pagination(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
        {
            _items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        public int PageSize { get; private set; }

        public int PageNumber { get; private set; }

        public int TotalItems { get; private set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItems / (double)PageSize);
            }
        }

        public int FirstItem
        {
            get
            {
                return (PageNumber - 1) * PageSize + 1;
            }
        }

        public int LastItem
        {
            get
            {
                return FirstItem + _items.Count() - 1;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }
}