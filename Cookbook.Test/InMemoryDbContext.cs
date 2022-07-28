using Cookbook.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Test
{
    public class InMemoryDbContext
    {
        private readonly SqliteConnection conn;
        private readonly DbContextOptions<ApplicationDbContext> options;

        public InMemoryDbContext()
        {
            conn = new SqliteConnection("Filename=:memory:");
            conn.Open();

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(conn)
                .Options;

            using var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();
        }

        public ApplicationDbContext CreateContext() => new ApplicationDbContext(options);

        public void Dispose() => conn.Dispose();
    }
}
