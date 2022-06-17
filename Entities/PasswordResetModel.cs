using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class PasswordResetModel
    {
        public int Id { get; set; }
        [Required]
        public string EmailToken { get; set; }
        [Required]
        public string TokenGenerated { get; set; }
    }
}
