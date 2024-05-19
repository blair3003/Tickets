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
        private readonly ApplicationUser _testUser2;

        public UserManager<ApplicationUser> UserManager => _userManager;
        public ApplicationUser TestUser => _testUser;
        public ApplicationUser TestUser2 => _testUser2;

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

            _testUser = CreateUserAsync("test@user.com", "TestUser", "Abc!23").GetAwaiter().GetResult();
            _testUser2 = CreateUserAsync("test@user2.com", "TestUser2", "Abc!23").GetAwaiter().GetResult();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;
            return new ApplicationDbContext(options);
        }

        public async Task<ApplicationUser> CreateUserAsync(string email, string userName, string password)
        {
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
