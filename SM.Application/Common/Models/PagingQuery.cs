using Sieve.Models;

namespace SM.Application.Common.Models
{
    public class PagingQuery : SieveModel
    {
        public PagingQuery()
        {
            this.Page = 1;
            this.PageSize = 10;
        }
        public PagingQuery(int page, int pageSize)
        {
            this.Page = page < 1 ? 1 : page;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
