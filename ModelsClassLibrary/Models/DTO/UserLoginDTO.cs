using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models.DTO
{
    public class UserLoginDTO
    {
        [Required]
        public required string UserName { get; set; }

        [DataType(DataType.Password), Length(minimumLength: 4, maximumLength: 16), Required]
        public required string Password { get; set; }
    }
}
