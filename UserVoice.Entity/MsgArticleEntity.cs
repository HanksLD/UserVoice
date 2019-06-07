using System;

namespace UserVoice.Entity
{
    public class MsgArticleEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime CreatedTime { get; set; }

        public string MsgContent { get; set; }

        public int CategoryId { get; set; }

        public CategoryEntity Category { get; set; }
    }
}
