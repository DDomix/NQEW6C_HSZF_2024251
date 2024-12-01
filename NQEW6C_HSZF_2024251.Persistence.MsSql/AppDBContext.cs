using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NQEW6C_HSZF_2024251.Model;

namespace NQEW6C_HSZF_2024251.Persistence.MsSql
{
    public class AppDBContext : DbContext
    {
        public DbSet<TeamsEntity> Teams { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=F1DB ;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }


        public AppDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
    }
}
