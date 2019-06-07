using Microsoft.EntityFrameworkCore;
using System;
using UserVoice.Entity;
using UserVoice.Entity.IRepositories;

namespace UserVoice.Repository
{
    public class CategoryRepository : BaseRepository<CategoryEntity,int>,ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context) { }
    }
}
