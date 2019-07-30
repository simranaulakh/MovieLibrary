using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MovieLibrary.Models;

namespace MovieLibrary.DAL
{
    public class mySQLdbContext : DbContext
    {
        public mySQLdbContext(DbContextOptions<mySQLdbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movie>().ToTable("movielib");
            builder.Entity<Movie>().Property(t => t.MovieID).HasColumnName("MovieID");
            builder.Entity<Movie>().Property(t => t.MovieTitle).HasColumnName("MovieTitle");
            builder.Entity<Movie>().Property(t => t.MovieLanguage).HasColumnName("MovieLanguage");
            builder.Entity<Movie>().Property(t => t.MovieCategory).HasColumnName("MovieCategory");
        }

        public virtual DbSet<Movie> Movies { get; set; }
    }

}
