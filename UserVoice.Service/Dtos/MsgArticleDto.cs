using System;
using System.Collections.Generic;
using System.Text;

namespace UserVoice.Service.Dtos
{
    public class MsgArticleDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime CreatedTime { get; set; }

        public string MsgContent { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }


    }
}
