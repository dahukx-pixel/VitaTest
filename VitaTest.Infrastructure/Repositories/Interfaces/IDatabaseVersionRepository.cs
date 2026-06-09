using VitaTest.Domain.Models;

namespace VitaTest.Infrastructure.Repositories.Interfaces
{
    public interface IDatabaseVersionRepository
    {
        Task<DataBaseVersion?> GetDataBaseVersionAsync();
    }
}
