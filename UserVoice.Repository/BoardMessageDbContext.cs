using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Repository.EntityMaps;

namespace UserVoice.Repository
{
    public class BoardMessageDbContext : DbContext
    {
        public BoardMessageDbContext() : base()
        {
        }

        public BoardMessageDbContext(DbContextOptions<BoardMessageDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityMap());
            modelBuilder.ApplyConfiguration(new MsgArticleEntityMap());
            modelBuilder.ApplyConfiguration(new AppUserEntityMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
