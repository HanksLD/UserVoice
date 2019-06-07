using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Service.Dtos;

namespace UserVoice.Service
{
    public interface IMsgArticleService
    {
        ResponseResultDto<PagedResultDto<MsgArticleDto>> GetList(string title, int page, int pageSize);

        ResponseResultDto<MsgArticleDto> GetById(int id);

        ResponseResultDto<bool> AddArticle(MsgArticleDto article);

        ResponseResultDto<bool> UpdateArticle(MsgArticleDto article);

        ResponseResultDto<bool> DeleteArticle(int id);

        ResponseResultDto<bool> DeleteArticle(List<int> idList);
    }
}
