using ModelsClassLibrary.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string UserName { get; set; }

        [DataType(DataType.Password), Length(minimumLength:4, maximumLength:16), Required]
        public required string Password { get; set; }

        [DataType(DataType.EmailAddress), Required]
        public required string Email { get; set; }
        
        [Required, MinLength(3)]
        public required string FullName { get; set; }

        public GenderEnum Gender { get; set; }

        public string? Address { get; set; }

        [MinLength(10), MaxLength(10), DataType(DataType.PhoneNumber)]
        public int Phone { get; set; }

        public UserTypeEnum UserType { get; set; }

        public virtual Cart? Cart {  get; set; }

    }

}
