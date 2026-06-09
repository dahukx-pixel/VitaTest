using Microsoft.EntityFrameworkCore;

namespace VitaTest.Infrastructure.Database
{
    public class VitaDatabaseFactory : IDbContextFactory<VitaDatabase>
    {
        private readonly string _connectionString;

        public VitaDatabaseFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public VitaDatabase CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<VitaDatabase>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new VitaDatabase(optionsBuilder.Options);
        }
    }
}
