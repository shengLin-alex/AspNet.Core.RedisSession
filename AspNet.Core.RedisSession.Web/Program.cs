using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AspNet.Core.RedisSession.Web
{
    /// <summary>
    /// dotnet core Main program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// constructor
        /// </summary>
        protected Program()
        {
        }

        /// <summary>
        /// project entry point
        /// </summary>
        /// <param name="args">program arguments</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// build <see cref="WebHost"/> and set <see cref="Startup"/>
        /// </summary>
        /// <param name="args">program arguments</param>
        /// <returns><see cref="IWebHostBuilder"/></returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}