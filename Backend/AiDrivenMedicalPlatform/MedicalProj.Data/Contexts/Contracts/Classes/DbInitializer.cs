using MedicalProj.Data.Contexts.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedicalProj.Data.Contexts.Contracts.Classes
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
				if(!_context.Roles.Any())
                {
                    var rolesData = File.ReadAllText(@".\..\MedicalProj.Data\Contexts\Seeding\roles.json");
                    var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesData);
                    if(roles is not null && roles.Any())
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
