using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BeautyAtHome.Utils
{
    public interface IPagingSupport<T> : IEnumerable<T>
    {
        ///<summary>
        /// Get the total entity count.
        ///</summary>
        int Count { get; }

        ///<summary>
        /// Get a range of persited entities.
        ///</summary>
        PagingSupport<T> GetRange(int pageIndex, int pageSize, Expression<Func<T, int>> orderLambda);

        PagingViewModel<T> ToPagingViewModel();

    }

    public class PagingSupport<T> : IPagingSupport<T>
    {
        private IQueryable<T> _source;
        private int _pageIndex;
        private int _pageSize;

        public PagingSupport(IQueryable<T> source)
        {
            this._source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _source.Count(); }
        }


        public PagingSupport<T> GetRange(int pageIndex, int pageSize, Expression<Func<T, int>> orderLambda)
        {
            _pageIndex = pageIndex;
            _pageSize = pageSize;
            _source = _source.OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return this;
        }

        public PagingViewModel<T> ToPagingViewModel()
        {
            int count = Count;
            var pagingVM = new PagingViewModel<T>()
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
    }

    public class PagingViewModel<T>
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
