using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class PasswordResetTokenDto
    {
        [Required]
        public string Email { get; set; }
    }
}
