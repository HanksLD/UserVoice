using System;
using System.Collections.Generic;
using System.Text;

namespace UserVoice.Entity.IRepositories
{
    public interface IMsgArticleRepository : IBaseRepository<MsgArticleEntity,int>
    {
        MsgArticleEntity GetById(int id, bool isIncludeForeign = false);
    }
}
