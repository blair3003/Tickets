using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Tests
{
    public class IdentityTests : IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityTests()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            _sqliteConnection = new SqliteConnection("DataSource=:memory:");

            serviceCollection
                .AddLogging()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_sqliteConnection))
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _applicationDbContext.Database.OpenConnection();
            _applicationDbContext.Database.EnsureCreated();

            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(); 
        }

        public void Dispose()
        {
            _applicationDbContext.Database.EnsureDeleted();
            _applicationDbContext.Dispose();
            _sqliteConnection.Close();
        }

        [Fact]
        public async Task CanRegisterUser()
        {
            string userName = "Test";
            string email = "test@test.com";
            string password = "Abc!23";

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            Assert.True(result.Succeeded);

            user = await _userManager.FindByNameAsync(userName);

            Assert.NotNull(user);
            Assert.Equal(userName, user.UserName);
            Assert.Equal(email, user.Email);
        }
    }
}
