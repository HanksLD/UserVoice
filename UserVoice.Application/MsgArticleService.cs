using System;
using UserVoice.Service.Dtos;
using UserVoice.Service;
using System.Collections.Generic;
using UserVoice.Entity.IRepositories;
using System.Linq.Expressions;
using System.Linq;
using UserVoice.Entity;

namespace UserVoice.Application
{
    public class MsgArticleService : IMsgArticleService
    {
        private readonly IMsgArticleRepository articleRepository;
        private readonly ICategoryRepository cateRepository;

        public MsgArticleService(IMsgArticleRepository articleRepository, ICategoryRepository cateRepository)
        {
            this.articleRepository = articleRepository;
            this.cateRepository = cateRepository;
        }

        public ResponseResultDto<PagedResultDto<MsgArticleDto>> GetList(string title, int page, int pageSize)
        {
            ResponseResultDto<PagedResultDto<MsgArticleDto>> result = new ResponseResultDto<PagedResultDto<MsgArticleDto>>();
            try
            {
                var query = this.articleRepository.Query();
                if (!string.IsNullOrWhiteSpace(title))
                    query = query.Where(p => p.Title.Contains(title));
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 || pageSize > 100 ? 10 : pageSize;
                result.Result = new PagedResultDto<MsgArticleDto>();
                result.Result.Total = query.Count();
                result.Result.Page = page;
                result.Result.PageSize = pageSize;
                result.Result.Results = (from a in query
                                         join b in cateRepository.Query() on a.CategoryId equals b.Id
                                         select new MsgArticleDto
                                         {
                                             Id = a.Id,
                                             Author = a.Author,
                                             CategoryId = a.CategoryId,
                                             CategoryName = b.Name,
                                             CreatedTime = a.CreatedTime,
                                             Title = a.Title
                                         }).ToList();
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误，请稍后重试";
            }
            return result;
        }

        public ResponseResultDto<MsgArticleDto> GetById(int id)
        {
            ResponseResultDto<MsgArticleDto> result = new ResponseResultDto<MsgArticleDto>();
            try
            {
                var entity = this.articleRepository.GetById(id,true);
                if (null == entity)
                    result.ErrorMessage = "记录不存在或已删除";
                else
                {
                    result.Result = new MsgArticleDto()
                    {
                        Id = entity.Id,
                        Author = entity.Author,
                        CategoryId = entity.CategoryId,
                        CategoryName = entity.Category.Name,
                        CreatedTime = entity.CreatedTime,
                        MsgContent = entity.MsgContent,
                        Title = entity.Title
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

        public ResponseResultDto<bool> AddArticle(MsgArticleDto article)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                MsgArticleEntity entity = new MsgArticleEntity()
                {
                    Id = article.Id,
                    Author = article.Author,
                    CategoryId = article.CategoryId,
                    CreatedTime = article.CreatedTime,
                    MsgContent = article.MsgContent,
                    Title = article.Title
                };
                this.articleRepository.Insert(entity);
                result.IsSuccess = this.articleRepository.SaveChanged() > 0;
                result.Result = result.IsSuccess;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误";
            }
            return result;
        }

        public ResponseResultDto<bool> UpdateArticle(MsgArticleDto article)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                int count = this.articleRepository.Query(p => p.Id == article.Id).Count();
                if (count <= 0)
                    result.ErrorMessage = "记录不存在或已删除";
                else
                {
                    MsgArticleEntity entity = new MsgArticleEntity()
                    {
                        Id = article.Id,
                        Author = article.Author,
                        CategoryId = article.CategoryId,
                        CreatedTime = article.CreatedTime,
                        MsgContent = article.MsgContent,
                        Title = article.Title
                    };
                    this.articleRepository.Update(entity);
                    result.IsSuccess = this.articleRepository.SaveChanged() > 0;
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

        public ResponseResultDto<bool> DeleteArticle(int id)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                var entity = this.articleRepository.Query(p => p.Id == id).FirstOrDefault();
                if (null != entity)
                {
                    this.articleRepository.Delete(entity);
                    result.IsSuccess = this.articleRepository.SaveChanged() > 0;
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

        public ResponseResultDto<bool> DeleteArticle(List<int> idList)
        {
            ResponseResultDto<bool> result = new ResponseResultDto<bool>();
            try
            {
                var entities = this.articleRepository.Query(p => idList.Contains(p.Id)).ToList();
                if(entities?.Count > 0)
                {
                    this.articleRepository.Delete(entities);
                    result.IsSuccess = this.articleRepository.SaveChanged() > 0;
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
