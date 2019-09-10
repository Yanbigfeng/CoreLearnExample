using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreLearnExample.EData
{
    public partial class testContext : DbContext
    {
        public testContext()
        {
            id = Guid.NewGuid().ToString();
        }

        public testContext(DbContextOptions<testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TableTest> TableTest { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=YBF;Database=CoreEF;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TableTest>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Describe).HasMaxLength(50);

                entity.Property(e => e.Introduce).HasMaxLength(50);
            });
        }
    }
    public partial class testContext
    {
        private static testContext instance;
        public string id;
        public testContext _db;

        private testContext([FromServices] testContext context)
        {
            if (_db == null)
            {
                _db = context;
            }
        }
        public static testContext GetInstance(testContext testContext)
        {

            if (instance == null)
            {
                instance = testContext;
            }

            return instance;
        }
    }
}
