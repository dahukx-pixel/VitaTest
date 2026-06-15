using Microsoft.EntityFrameworkCore;
using VitaTest.AppCore;
using VitaTest.Infrastructure.Interfaces;

namespace VitaTest.Infrastructure.Database
{
    public class VitaDatabaseFactory : IDbContextFactory<VitaDatabase>
    {
        private readonly ISettingsService _settingsService;
        private readonly Func<AppSettings, string> _connectionStringBuilder;

        public VitaDatabaseFactory(ISettingsService settingsService,
                                   Func<AppSettings, string> connectionStringBuilder)
        {
            _settingsService = settingsService;
            _connectionStringBuilder = connectionStringBuilder;
        }

        public VitaDatabase CreateDbContext()
        {
            var connectionString = _connectionStringBuilder(_settingsService.Current);
            var optionsBuilder = new DbContextOptionsBuilder<VitaDatabase>();
            optionsBuilder.UseSqlServer(connectionString);
            return new VitaDatabase(optionsBuilder.Options);
        }
    }
}
