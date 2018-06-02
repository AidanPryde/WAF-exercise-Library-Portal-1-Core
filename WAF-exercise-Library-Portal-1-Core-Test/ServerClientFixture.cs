using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using WAF_exercise_Library_Portal_1_Core_Db;

namespace WAF_exercise_Library_Portal_1_Core_Test
{
    public class ServerClientFixture : IDisposable
    {
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public LibraryDbContext Context { get; private set; }

        public ServerClientFixture()
        {
            // Arrange
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>()
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false));

            Context = Server.Host.Services.GetRequiredService<LibraryDbContext>();
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}
