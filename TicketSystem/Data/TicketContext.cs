using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;

namespace TicketSystem.Data
{
    public class TicketContext:DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {

        }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Severity> Severities { get; set; }
        public DbSet<ProblemCategory> ProblemCategories { get; set; }

        // protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role()
            {
                Id = 1, Name = "Admin"
            },new Role() {Id=2,Name="Rd" }, new Role() { Id = 3, Name = "Qa" }, new Role() { Id = 4, Name = "Pm" });
            modelBuilder.Entity<User>().HasData(
                new User(){Id = 1, Account="admin", Password="123", Name = "Admin",RoleId = 1 },
                new User() { Id = 2, Account = "Rd", Password = "123", Name = "Rd", RoleId = 2 },
                new User() { Id = 3, Account = "Qa", Password = "123", Name = "Qa", RoleId = 3 },
                new User() { Id = 4, Account = "Pm", Password = "123", Name = "Pm", RoleId = 4 }
                );
            modelBuilder.Entity<Priority>().HasData(new Priority() { Id = 1, Name = "優先" }, new Priority() { Id = 2, Name = "普通" },
                new Priority() { Id = 3, Name = "不急" });
            modelBuilder.Entity<Severity>().HasData(new Severity() { Id = 1, Name = "嚴重" }, new Severity() { Id = 2, Name = "普通" },
                new Severity() { Id = 3, Name = "輕微" });
            modelBuilder.Entity<ProblemCategory>().HasData(new ProblemCategory() { Id = 1, Name = "Normal" }, new ProblemCategory() { Id = 2, Name = "Feature Request" },
                new ProblemCategory() { Id = 3, Name = "Test Case" });
            
        }
    }
}
