using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models.DTO
{
    public class AuthTokenDTO
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
