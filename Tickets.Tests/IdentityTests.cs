using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Tests
{
    public class IdentityTests(TestEnvironment env) : IClassFixture<TestEnvironment>
    {
        private readonly TestEnvironment _env = env;

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

            var result = await _env.UserManager.CreateAsync(user, password);
            Assert.True(result.Succeeded);

            using var verificationContext = _env.CreateContext();
            var verifiedUser = await verificationContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            Assert.NotNull(verifiedUser);
            Assert.Equal(userName, verifiedUser.UserName);
            Assert.Equal(email, verifiedUser.Email);
        }

        [Fact]
        public async Task CannotRegisterUser_WhenEmailExistsAlready()
        {
            string userName1 = "User1";
            string userName2 = "User2";
            string email = "duplicate@email.com";
            string password = "Abc!23";

            var user1 = new ApplicationUser
            {
                UserName = userName1,
                Email = email
            };

            var user2 = new ApplicationUser
            {
                UserName = userName2,
                Email = email
            };

            var createUser1 = await _env.UserManager.CreateAsync(user1, password);
            Assert.True(createUser1.Succeeded);

            var createUser2 = await _env.UserManager.CreateAsync(user2, password);
            Assert.False(createUser2.Succeeded);
        }

        [Fact]
        public async Task AdminUserHasClaim()
        {
            string userName = "admin";
            string email = "admin@test.com";
            string password = "Abc!23";

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            var createUser = await _env.UserManager.CreateAsync(user, password);
            Assert.True(createUser.Succeeded);

            var userClaims = await _env.UserManager.GetClaimsAsync(user);
            Assert.DoesNotContain(userClaims, c => c.Type == "IsAdmin" && bool.Parse(c.Value) == true);

            var addClaim = await _env.UserManager.AddClaimAsync(user, new Claim("IsAdmin", "true"));
            Assert.True(addClaim.Succeeded);

            using var verificationContext = _env.CreateContext();
            var adminUser = await verificationContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            Assert.NotNull(adminUser);

            var adminUserClaims = await _env.UserManager.GetClaimsAsync((ApplicationUser) adminUser);
            Assert.Contains(adminUserClaims, c => c.Type == "IsAdmin" && bool.Parse(c.Value) == true);
        }
    }
}
