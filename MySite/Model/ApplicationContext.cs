﻿using Microsoft.EntityFrameworkCore;

namespace MySite.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        public DbSet<Watched> Watched { get; set; }
    }
}
