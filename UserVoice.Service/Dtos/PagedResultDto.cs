using System;
using System.Collections.Generic;
using System.Text;

namespace UserVoice.Service.Dtos
{
    public class PagedResultDto<TResult>
    {
        public List<TResult> Results { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}
