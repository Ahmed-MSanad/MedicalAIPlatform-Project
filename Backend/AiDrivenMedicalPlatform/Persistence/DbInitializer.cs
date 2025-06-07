using MedicalProj.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MedicalDbContext _context;

        public DbInitializer(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!_context.Roles.Any())
                {
                    var rolesData = File.ReadAllText(@".\..\Persistence\Data\Seeding\roles.json");
                    var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesData);
                    if (roles is not null && roles.Any())
                    {
                        await _context.Roles.AddRangeAsync(roles);
                        await _context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
