using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Entity;
using UserVoice.Entity.IRepositories;
using System.Linq;

namespace UserVoice.Repository
{
    public class MsgArticleRepository : BaseRepository<MsgArticleEntity,int>,IMsgArticleRepository
    {
        public MsgArticleRepository(DbContext context) : base(context) { }

        public MsgArticleEntity GetById(int id,bool isIncludeForeign = false)
        {
            if (isIncludeForeign)
                return this.Table.Include(p => p.Category).Where(p => p.Id == id).FirstOrDefault();
            else
                return this.Table.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
