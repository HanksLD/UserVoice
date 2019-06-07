using System;
using System.Collections.Generic;
using System.Text;

namespace UserVoice.Service.Dtos
{
    public class AppUserDto
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }
    }
}
