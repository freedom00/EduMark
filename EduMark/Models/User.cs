using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduMark.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }


        public string AboutMe { get; set; }

        public string PhotoFile { get; set; }

        [StringLength(20)]
        public string Role { get; set; }

        [StringLength(200)]
        public string TeachLanguages { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }




    }
}
