using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Entity;
using UserVoice.Entity.IRepositories;

namespace UserVoice.Repository
{
    public class AppUserRepository : BaseRepository<AppUserEntity, int>,IAppUserRepository
    {
        public AppUserRepository(DbContext context) : base(context) { }
    }
}
