using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserVoice.Service;
using UserVoice.Service.Dtos;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace UserVoice.WebApp.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMsgArticleService articleService;

        public MessageController(IMsgArticleService articleService)
        {
            this.articleService = articleService;
        }

        public IActionResult ModelView(int id)
        {
            if (id > 0)
            {
                var model = articleService.GetById(id);
                if (model.IsSuccess && model.Result != null)
                    return View("~/Views/Article/View.cshtml", model.Result);
                else
                    return Content("记录不存在或已删除", "text/html");
            }
            return View("~/Views/Article/View.cshtml",new MsgArticleDto());
        }

        [HttpPost]
        public IActionResult DeleteRows(string ids)
        {
            object result = null;
            try
            {
                List<int> idList = new List<int>();
                int id = 0;
                string[] idArray = ids.Split(',');
                foreach(string idstr in idArray)
                {
                    if (int.TryParse(idstr, out id))
                        idList.Add(id);
                }
                if(idList.Count > 0)
                {
                   ResponseResultDto<bool> serverResponse =  articleService.DeleteArticle(idList);
                    if (serverResponse.IsSuccess)
                        result = new { success = true };
                    else
                        result = new { success = false, error = serverResponse.ErrorMessage };
                }
                else
                {
                    result = new { success = true, error = "参数错误" };
                }
            }
            catch(Exception ex)
            {
                result = new { success=false,error="服务错误，请稍候重试" };
            }
            return Json(result,new JsonSerializerSettings() { DateFormatString="yyyy-MM-dd HH:mm:ss"});
        }

        [HttpPost]
        public IActionResult InsertOrUpdate(MsgArticleDto article)
        {
            object result = null;
            ResponseResultDto<bool> response = null;
            if (article.Id <= 0)
            {
                response = articleService.AddArticle(article);
            }
            else
            {
                response = articleService.UpdateArticle(article);
            }
            result = new { success = response.IsSuccess, error = response.ErrorMessage };
            return Json(result);
        }
    }
}
