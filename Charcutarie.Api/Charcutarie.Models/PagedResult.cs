using System;
using System.Collections.Generic;
using System.Linq;

namespace Charcutarie.Models
{
    public class PagedResult<T>
    {
        public int RecordCount { get; set; }
        public int CurrentPage { get; set; }
        public int RecordsPerpage { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int TotalPages => Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(RecordCount) / RecordsPerpage));

        public PagedResult<U> As<U>()
        {
            return new PagedResult<U>
            {
                RecordCount = RecordCount,
                CurrentPage = CurrentPage,
                RecordsPerpage = RecordsPerpage,
                Data = Data.Cast<U>().ToList()
            };

        }
    }
}
