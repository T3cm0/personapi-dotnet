using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;

namespace personapi_dotnet.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _db;
        public PersonaRepository(PersonaDbContext db) => _db = db;

        public Task<List<Persona>> GetAllAsync() =>
            _db.Personas.AsNoTracking().ToListAsync();

        public Task<Persona?> GetByIdAsync(int cc) =>
            _db.Personas.AsNoTracking().FirstOrDefaultAsync(x => x.Cc == cc);

        public Task<List<Persona>> SearchByApellidoAsync(string apellido) =>
            _db.Personas.AsNoTracking()
               .Where(x => x.Apellido.Contains(apellido))
               .ToListAsync();

        public async Task<Persona> AddAsync(Persona p)
        {
            _db.Personas.Add(p);
            await _db.SaveChangesAsync();
            return p;
        }

        public async Task<bool> UpdateAsync(int cc, Persona p)
        {
            if (cc != p.Cc) return false;
            _db.Personas.Update(p);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int cc)
        {
            var entity = await _db.Personas.FindAsync(cc);
            if (entity is null) return false;
            _db.Personas.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
