﻿using System;
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
            optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = F1DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
            base.OnConfiguring(optionsBuilder);
        }

        public AppDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
    }
}
