using fertilizesop.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace fertilizesop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            configureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            Application.Run(ServiceProvider.GetRequiredService<dashboardform>());
        }
        public static void configureServices(IServiceCollection services)
        {
            services.AddTransient<HomeContentform>();
            services.AddTransient<dashboardform>();
        }
    }
}
