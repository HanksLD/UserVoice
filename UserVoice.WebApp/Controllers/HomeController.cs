using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserVoice.Service;
using Newtonsoft.Json;
using UserVoice.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace UserVoice.WebApp.Controllers
{
    [UserAuthorize]
    public class HomeController : Controller
    {
        private readonly IMsgArticleService articleService;
        private readonly ICategoryService cateService;

        public HomeController(IMsgArticleService articleService ,ICategoryService cateService)
        {
            this.articleService = articleService;
            this.cateService = cateService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Default()
        {
            return View();
        }

        public IActionResult ArticleIndex()
        {
            return View("~/Views/Article/Index.cshtml");
        }

        [HttpPost]
        public IActionResult ArticleList(string title,int page,int pageSize)
        {
            object result = null;
            var listResult = articleService.GetList(title, page, pageSize);
            if (listResult.IsSuccess)
                result = new { success = true, rows = listResult.Result.Results, total = listResult.Result.Total };
            else
                result = new { success = false, rows = new List<object>(), total = 0, error = listResult.ErrorMessage };
            return this.Json(result, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }

        public IActionResult CategoryIndex()
        {
            return View("~/Views/Category/Index.cshtml"); ;
        }

        public IActionResult CategoryList(string name,int page,int pageSize)
        {
            object result = null;
            var response = this.cateService.GetList(name, page, pageSize);
            if (response.IsSuccess)
            {
                result = new { success = true, rows = response.Result.Results, total = response.Result.Total };
            }
            else
            {
                result = new { success = false, error = response.ErrorMessage, rows = new List<object>(), total = 0 };
            }
            return Json(result,new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss"});
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
