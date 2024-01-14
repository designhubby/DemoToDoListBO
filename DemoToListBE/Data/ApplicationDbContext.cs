using DemoToListBE.Configuration;
using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.Entity;
using DemoToListBE.Data.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Options;
using System.Xml;

namespace DemoToListBE.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ConnectionStrings _connectionStrings;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptionsMonitor<ConnectionStrings> optionsMonitor)
    : base(options)
        {
            _connectionStrings = optionsMonitor.CurrentValue;
        }
        // Update the connection string to use LocalDB
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionStrings.SQLLocalDb);
            }
        }


        //entities

        DbSet<ToDoList> ToDoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ToDoListConfiguration());

        }
        


    }
}
