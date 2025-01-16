using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsClassLibrary.Models.Enums;

namespace ModelsClassLibrary.Models.DTO
{
    public class ZeptoUserDTO
    {
        [Required]
        public required string UserName { get; set; }

        [DataType(DataType.Password), Length(minimumLength: 4, maximumLength: 16), Required]
        public required string Password { get; set; }

        [DataType(DataType.EmailAddress), Required]
        public required string Email { get; set; }

        [Required, MinLength(3)]
        public required string FullName { get; set; }

        public GenderEnum Gender { get; set; }

        public string? Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public long Phone { get; set; }

        public UserTypeEnum UserType { get; set; }
    }
}
