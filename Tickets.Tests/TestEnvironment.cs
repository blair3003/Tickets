using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Tickets.Data.Models;
using Tickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Tickets.Tests
{
    public class TestEnvironment : IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationDbContext ApplicationDbContext => _applicationDbContext;
        public UserManager<ApplicationUser> UserManager => _userManager;

        public TestEnvironment()
        {
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");

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

        public async Task AddUsers()
        {
            var users = new List<ApplicationUser>
            {
                new() { UserName = "alice", Email = "user1@test.com" },
                new() { UserName = "bob", Email = "user2@test.com" },
                new() { UserName = "charlie", Email = "user3@test.com" }
            };

            foreach (var user in users)
            {
                await _userManager.CreateAsync(user, "Abc!23");
            }
        }

        public async Task AddAdminUser()
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@test.com"
            };

            await _userManager.CreateAsync(admin, "Abc!23");
            await _userManager.AddClaimAsync(admin, new Claim("IsAdmin", "true"));
        }
    }
}
