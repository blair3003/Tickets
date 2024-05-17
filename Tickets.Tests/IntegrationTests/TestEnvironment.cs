using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Tickets.Data.Models;
using Tickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tickets.Tests.IntegrationTests
{
    public class TestEnvironment : IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _identityDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUser _testUser;

        public UserManager<ApplicationUser> UserManager => _userManager;
        public ApplicationUser TestUser => _testUser;

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

            _testUser = CreateUserAsync().GetAwaiter().GetResult();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;
            return new ApplicationDbContext(options);
        }

        public async Task<ApplicationUser> CreateUserAsync()
        {
            var userName = "TestUser";
            var email = "test@user.com";
            var password = "Abc!23";

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            var createUserResult = await _userManager.CreateAsync(user, password);

            if (createUserResult.Succeeded)
            {
                return user;
            }
            else
            {
                throw new InvalidOperationException("Failed to create Test User.");
            }
        }

        public void Dispose()
        {
            _identityDbContext.Database.EnsureDeleted();
            _identityDbContext.Dispose();
            _sqliteConnection.Close();
        }
    }
}
