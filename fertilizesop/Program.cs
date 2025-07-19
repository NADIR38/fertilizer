using fertilizesop.BL.Bl;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
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
        {//DL Layer
            services.AddScoped<ICustomerDl,CustomerDl>();

            //Bl Layer
            services.AddScoped<ICustomerBl, CustomerBl>();
            //UI Layer
            services.AddTransient<HomeContentform>();
            services.AddTransient<dashboardform>();
            services.AddTransient<AddCustomer>();
            services.AddTransient<CustomerForm>();

        }
    }
}
