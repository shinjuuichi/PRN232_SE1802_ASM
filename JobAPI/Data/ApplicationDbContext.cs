﻿using JobAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; } = default!;
    }
}
