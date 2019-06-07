using System;
using System.Collections.Generic;

namespace UserVoice.Entity
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public IEnumerable<MsgArticleEntity> Articles { get; set; }
    }
}
