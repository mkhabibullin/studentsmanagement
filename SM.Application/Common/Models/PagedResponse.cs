namespace SM.Application.Common.Models
{
    public class PagedResponse<T> : Response<T>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public long Total { get; set; }

        public PagedResponse(T data, int? page, int? pageSize, long total)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.Total = total;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
