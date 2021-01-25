using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace BeautyAtHome.Utils
{
    public interface IPagingSupport<T>
    {
        ///<summary>
        /// Get the total entity count.
        ///</summary>
        int Count { get; }

        ///<summary>
        /// Get a range of persited entities.
        ///</summary>
        PagingSupport<T> GetRange(int pageIndex, int pageSize, Expression<Func<T, object>> orderLambda);

        ///<summary>
        /// Include child entity.
        ///</summary>
        PagingSupport<T> Include(Expression<Func<T, object>> selector);

        ///<summary>
        /// Get paginated result.
        ///</summary>
        Paged<T> Paginate();
    }

    public class PagingSupport<T> : IPagingSupport<T>
    {
        private IEnumerable<T> _source;
        private int _pageIndex;
        private int _pageSize;

        private static PagingSupport<T> _instance;
        private static readonly object _lock = new object();
        public PagingSupport(IEnumerable<T> source)
        {
            this._source = source;
        }

        public static PagingSupport<T> Instance(IEnumerable<T> source)
        {
            if(_instance == null)
            {
                lock (_lock)
                {
                    if(_instance == null)
                    {
                        _instance = new PagingSupport<T>(source);
                    }
                }
            }
            return _instance;
        }

        public int Count
        {
            get { return _source.Count(); }
        }

        public PagingSupport<T> GetRange(int pageIndex, int pageSize, Expression<Func<T, object>> orderLambda)
        {
            _pageIndex = pageIndex;
            _pageSize = pageSize;
            _source = _source.AsQueryable().OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return this;
        }

        public Paged<T> Paginate()
        {
            int count = Count;
            var pagingVM = new Paged<T>()
            {
                TotalCount = Count,
                PageSize = _pageSize,
                TotalPage = (int)Math.Ceiling((double)Count / _pageSize),
                CurrentPage = _pageIndex,
                Content = _source
            };
            if (_pageIndex > 1)
            {
                pagingVM.PreviousPage = _pageIndex - 1;
            }

            if (_pageIndex < count)
            {
                pagingVM.NextPage = _pageIndex + 1;
            }

            return pagingVM;
        }

        public PagingSupport<T> Include(Expression<Func<T, object>> selector)
        {
            return this;
        }
    }

    public class Paged<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}
