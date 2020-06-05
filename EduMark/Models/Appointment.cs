using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduMark.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }

        [Required]
        public string Language { get; set; }

      [Required]
        public int AvailabilityId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual User Student {get; set; }

        public virtual Availability Availability { get; set; }

    }
}
