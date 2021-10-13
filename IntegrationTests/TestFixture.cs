using AuthenticationService;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;

namespace IntegrationTests
{
    public static class TestFixture
    {
        public static TestServer TestServer { get; }
        public static HttpClient Client { get; }
        private static RoleManager<IdentityRole> _roleManager;
        private static AuthenticationDbContext _dbContext;

        static TestFixture()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            var webBuilder = new WebHostBuilder()
                .UseConfiguration(configurationBuilder.Build())
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            TestServer = new TestServer(webBuilder);
            Client = TestServer.CreateClient();
            _roleManager = TestServer.Host.Services.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;
            _dbContext = TestServer.Host.Services.GetService(typeof(AuthenticationDbContext)) as AuthenticationDbContext;

            FillingDatabase();
        }

        public static void Dispose()
        {
            Client.Dispose();
            TestServer.Dispose();
        }

        private static void FillingDatabase()
        {
            _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            _roleManager.CreateAsync(new IdentityRole { Name = "Moderator" });
            _roleManager.CreateAsync(new IdentityRole { Name = "Administrator" });
            _dbContext.SaveChanges();
        }
    }
}
