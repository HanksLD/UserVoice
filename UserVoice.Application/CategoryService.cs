using System;
using UserVoice.Service.Dtos;
using UserVoice.Service;
using System.Collections.Generic;
using UserVoice.Entity.IRepositories;
using System.Linq.Expressions;
using System.Linq;
using UserVoice.Entity;
using Microsoft.Extensions.Logging;

namespace UserVoice.Application
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository cateRepository;
        private readonly IMsgArticleRepository articleRepository;
        private readonly ILogger log;

        public CategoryService(ICategoryRepository cateRepository, 
            IMsgArticleRepository articleRepository,ILogger<CategoryService> log)
        {
            this.cateRepository = cateRepository;
            this.articleRepository = articleRepository;
            this.log = log;
        }

        public ResponseResultDto<PagedResultDto<CategoryDto>> GetList(string name, int page, int pageSize)
        {
            ResponseResultDto<PagedResultDto<CategoryDto>> result = new ResponseResultDto<PagedResultDto<CategoryDto>>();
            try
            {
                var query = this.cateRepository.Query();
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(p => p.Name.Contains(name));
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 || pageSize > 100 ? 10 : pageSize;
                result.Result = new PagedResultDto<CategoryDto>();
                result.Result.Total = query.Count();
                result.Result.Page = page;
                result.Result.PageSize = pageSize;
                result.Result.Results = query.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize).Select(p => new CategoryDto
                {
                                             Id = p.Id,
                                             Name = p.Name,
                                             Remark = p.Remark
                                         }).ToList();
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                log.LogError(ex,"发生错误");
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误，请稍后重试";
            }
            return result;
        }

        public ResponseResultDto<CategoryDto> GetById(int id)
        {
            ResponseResultDto<CategoryDto> result = new ResponseResultDto<CategoryDto>();
            try
            {
                var entity = this.cateRepository.Query(p => p.Id == id).FirstOrDefault();
                if (null == entity)
                    result.ErrorMessage = "记录不存在或已删除";
                else
                {
                    result.Result = new CategoryDto()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Remark = entity.Remark
                    };
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误";
            }
            return result;
        }

        public ResponseResultDto<bool> AddCategory(CategoryDto category)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                CategoryEntity entity = new CategoryEntity()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Remark = category.Remark
                };
                this.cateRepository.Insert(entity);
                result.IsSuccess = this.cateRepository.SaveChanged() > 0;
                result.Result = result.IsSuccess;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误";
            }
            return result;
        }

        public ResponseResultDto<bool> UpdateCategory(CategoryDto category)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                int count = this.cateRepository.Query(p => p.Id == category.Id).Count();
                if (count <= 0)
                    result.ErrorMessage = "记录不存在或已删除";
                else
                {
                    CategoryEntity entity = new CategoryEntity()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Remark = category.Remark
                    };
                    this.cateRepository.Update(entity);
                    result.IsSuccess = this.cateRepository.SaveChanged() > 0;
                    result.Result = result.IsSuccess;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误";
            }
            return result;
        }

        public ResponseResultDto<bool> DeleteCategory(int id)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                var entity = this.cateRepository.Query(p => p.Id == id).FirstOrDefault();
                if (null != entity)
                {
                    int count = this.articleRepository.Query(p => p.CategoryId == id).Count();
                    if (count > 0)
                    {
                        result.IsSuccess = false;
                        result.ErrorMessage = "该分类下尚存在留言，请先删除该分类下的所有留言";
                    }
                    else
                    {
                        this.cateRepository.Delete(entity);
                        result.IsSuccess = this.cateRepository.SaveChanged() > 0;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "记录不存在或已删除";
                }
                result.Result = result.IsSuccess;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误，请稍后重试";
            }
            return result;
        }

        public ResponseResultDto<bool> DeleteCategory(List<int> idList)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                var entities = this.cateRepository.Query(p => idList.Contains(p.Id)).ToList();
                if (null != entities && entities.Count > 0)
                {
                    int count = this.articleRepository.Query(p => idList.Contains(p.CategoryId)).Count();
                    if (count > 0)
                    {
                        result.IsSuccess = false;
                        result.ErrorMessage = "当前所选分类中，尚存在部分分类下有所属留言，请先删除所有相关联的留言";
                    }
                    else
                    {
                        this.cateRepository.Delete(entities);
                        result.IsSuccess = this.cateRepository.SaveChanged() > 0;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "记录不存在或已删除";
                }
                result.Result = result.IsSuccess;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误，请稍后重试";
            }
            return result;
        }
    }
}
