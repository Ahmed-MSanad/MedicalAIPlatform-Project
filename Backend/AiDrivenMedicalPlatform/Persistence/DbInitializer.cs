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
                    var roles = new List<IdentityRole>
                    {
                        new IdentityRole
                        {
                            Id = "1",
                            Name = "Admin",
                            NormalizedName = "ADMIN",
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        },
                        new IdentityRole
                        {
                            Id = "2",
                            Name = "Doctor",
                            NormalizedName = "DOCTOR",
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        },
                        new IdentityRole
                        {
                            Id = "3",
                            Name = "Patient",
                            NormalizedName = "PATIENT",
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        }
                    };

                    await _context.Roles.AddRangeAsync(roles);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Roles Failed: {ex}");
                throw;
            }
        }
    }
}
