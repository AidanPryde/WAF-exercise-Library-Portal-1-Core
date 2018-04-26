using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class LibraryDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookAuthor> BookAuthor { get; set; }
        public virtual DbSet<Cover> Cover { get; set; }
        public virtual DbSet<Lending> Lending { get; set; }
        public virtual DbSet<ApplicationUser> Visitor { get; set; }
        public virtual DbSet<Volume> Volume { get; set; }
    }
}
