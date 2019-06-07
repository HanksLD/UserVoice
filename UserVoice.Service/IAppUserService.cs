using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Service.Dtos;

namespace UserVoice.Service
{
    public interface IAppUserService
    {
        ResponseResultDto<AppUserDto> Login(string email,string password);


    }
}
