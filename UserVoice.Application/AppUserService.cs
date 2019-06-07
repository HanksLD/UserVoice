using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Entity.IRepositories;
using UserVoice.Service;
using UserVoice.Service.Dtos;
using System.Linq;

namespace UserVoice.Application
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository repository;
        private readonly ILogger log;

        public AppUserService(IAppUserRepository repository, ILogger<AppUserService> log)
        {
            this.repository = repository;
            this.log = log;
        }

        public ResponseResultDto<AppUserDto> Login(string email, string password)
        {
            ResponseResultDto<AppUserDto> result = new ResponseResultDto<AppUserDto>();
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    result.ErrorMessage = "用户名和密码不能为空";
                else
                {
                    var user = repository.Query(p => p.Email == email).FirstOrDefault();
                    if (null == user)
                        result.ErrorMessage = "用户名或者密码错误";
                    else
                    {
                        user.LastLoginTime = DateTime.Now;
                        repository.UpdateAsync(user);
                        result.IsSuccess = true;
                        result.Result = new AppUserDto()
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FullName = user.FullName
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "服务错误");
                result.IsSuccess = false;
                result.ErrorMessage = "服务错误，请稍后重试";
            }
            return result;
        }
    }
}
