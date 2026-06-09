using Microsoft.EntityFrameworkCore;
using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Repositories
{
    public class DatabaseVersionRepository : IDatabaseVersionRepository
    {
        private readonly VitaDatabase _dataBase;

        public DatabaseVersionRepository(VitaDatabase dataBase)
        {
            _dataBase = dataBase;
        }

        public async Task<DataBaseVersion?> GetDataBaseVersionAsync()
        {
            return await _dataBase.DataBaseVersion.FirstOrDefaultAsync();
        }
    }
}
