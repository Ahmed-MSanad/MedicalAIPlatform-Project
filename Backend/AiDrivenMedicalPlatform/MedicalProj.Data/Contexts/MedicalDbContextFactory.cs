using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MedicalProj.Data.Contexts
{
    public class MedicalDbContextFactory : IDesignTimeDbContextFactory<MedicalDbContext>
    {
        public MedicalDbContext CreateDbContext(string[] args)
        {
            var connectionString = "server=.;database=MedicalPlatformDB;Trusted_Connection=true;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<MedicalDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MedicalDbContext(optionsBuilder.Options);
        }
    }
}
