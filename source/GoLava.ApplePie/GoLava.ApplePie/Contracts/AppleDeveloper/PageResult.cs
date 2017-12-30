using System.Collections.Generic;
using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public abstract class PageResult : Result
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public abstract int RecordsCount { get; }
    }

    public class PageResult<TData> : PageResult
    {
        [JsonDataClassProperty]
        public List<TData> Data { get; set; }

        public override int RecordsCount
        {
            get => this.Data != null ? this.Data.Count : 0;
        }
    }
}