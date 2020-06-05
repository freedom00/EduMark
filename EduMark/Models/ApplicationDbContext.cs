using Microsoft.EntityFrameworkCore;

namespace EduMark.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Availability> Availabilities { get; set; }

        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Appointment>()
                .HasOne<User>(s => s.Student)
                .WithMany(ta => ta.Appointments)
                .HasForeignKey(u => u.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Availability>()
            //   .HasIndex(u => u.StartTime)
            //   .IsUnique();


            builder.Entity<User>(entity =>
        entity.HasCheckConstraint("CK_user_Role", " [Role] IN ('teacher', 'student')"));


        }
    }

}
