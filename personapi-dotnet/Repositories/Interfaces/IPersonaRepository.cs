using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories.Interfaces
{
    public interface IPersonaRepository
    {
        Task<List<Persona>> GetAllAsync();
        Task<Persona?> GetByIdAsync(int cc);
        Task<List<Persona>> SearchByApellidoAsync(string apellido);
        Task<Persona> AddAsync(Persona p);
        Task<bool> UpdateAsync(int cc, Persona p);
        Task<bool> DeleteAsync(int cc);
    }
}
