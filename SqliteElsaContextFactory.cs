using Elsa.Persistence.EntityFramework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsaQuickstarts.ConsoleApp.HelloWorld
{
    public class SqliteElsaContextFactory : IDesignTimeDbContextFactory<ElsaContext>
    {
        public ElsaContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder();

            dbContextBuilder.UseSqlite("Data Source=elsa.sqlite.db;Cache=Shared;", sqlite => sqlite.MigrationsAssembly(typeof(Elsa.Persistence.EntityFramework.Sqlite.SqliteElsaContextFactory).Assembly.FullName));

            return new ElsaContext(dbContextBuilder.Options);
        }
    }
}
