using System;
using System.Collections.Generic;
using System.Text;

namespace UserVoice.Service.Dtos
{
    public class ResponseResultDto<TResult>
    {
        public bool IsSuccess { get; set; }

        public TResult Result { get; set; }

        public string ErrorMessage { get; set; }
    }
}
