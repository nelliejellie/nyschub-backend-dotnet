using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace nyschub.Entities
{
    public class Corper : IdentityUser
    {
        public Corper()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public override string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string NyscRegNumber { get; set; }
        public string CorperPhoto { get; set; }
        public string StateOfDeployment { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
