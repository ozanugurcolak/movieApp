using Microsoft.EntityFrameworkCore;
using movieApp.web.Entity;
using movieApp.web.Entity;
using movieApp.web.Models;

namespace movieApp.web.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Person> People { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Admin> Admins { get; set; }  // Yeni eklendi
        public DbSet<User> Users { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }




        //protected override void onconfiguring(dbcontextoptionsbuilder optionsbuilder)
        //{
        //	optionsbuilder.usesqlite("data source=movies.db");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().Property(b => b.Title).IsRequired();
            modelBuilder.Entity<Movie>().Property(b => b.Title).HasMaxLength(500);

            modelBuilder.Entity<Genre>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Genre>().Property(b => b.Name).HasMaxLength(50);

        }
    }
}