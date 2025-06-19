using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.AppLicationDbContext
{
    public class AppDbContexFactory:IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Yahan apni actual connection string de do
            optionsBuilder.UseSqlServer("Server=ASHWANI;Database=HospitalDB;User Id=Ashu;Password=11410@ashu;TrustServerCertificate =true", x => x.MigrationsAssembly("Hospital.Auth"));
            

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
