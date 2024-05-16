using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Tickets.Data.Models;
using Tickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tickets.Tests
{
    public class TestEnvironment : IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _identityDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManager<ApplicationUser> UserManager => _userManager;

        public TestEnvironment()
        {
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection
                .AddLogging()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_sqliteConnection))
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _identityDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _identityDbContext.Database.OpenConnection();
            _identityDbContext.Database.EnsureCreated();

            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;
            return new ApplicationDbContext(options);
        }

        public async Task<IdentityResult> CreateUser(string id)
        {

            string userName = "Test";
            string email = "test@test.com";
            string password = "Abc!23";

            var user = new ApplicationUser
            {
                Id = id,
                UserName = userName,
                Email = email
            };

            var createUser = await _userManager.CreateAsync(user, password);
            return createUser;
        }

        public void Dispose()
        {
            _identityDbContext.Database.EnsureDeleted();
            _identityDbContext.Dispose();
            _sqliteConnection.Close();
        }
    }
}
