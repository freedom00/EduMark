using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduMark.Models
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        public virtual User Teacher { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

    }
}
