using Microsoft.EntityFrameworkCore;
using PersonRegister.Data;
using PersonRegister.Data.Models;

namespace PersonRegister
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            var contextFactory = new PersonRegisterDbContextFactory();

            var dbContext = contextFactory.CreateDbContext(args);

            await dbContext.Database.MigrateAsync();
        }
    }
}
