using System;
using UserVoice.Service.Dtos;
using System.Collections.Generic;

namespace UserVoice.Service
{
    public interface ICategoryService
    {
        ResponseResultDto<PagedResultDto<CategoryDto>> GetList(string name, int page, int pageSize);

        ResponseResultDto<CategoryDto> GetById(int id);

        ResponseResultDto<bool> AddCategory(CategoryDto category);

        ResponseResultDto<bool> UpdateCategory(CategoryDto category);

        ResponseResultDto<bool> DeleteCategory(int id);

        ResponseResultDto<bool> DeleteCategory(List<int> idList);
    }
}
