using fertilizesop.BL.Bl;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using fertilizesop.Interfaces.DLinterfaces;
using fertilizesop.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            var mainform = ServiceProvider.GetRequiredService<Customersale>();
            Application.Run(mainform);
        }
        public static void configureServices(IServiceCollection services)
        {//DL Layer
            services.AddScoped<ICustomerDl, CustomerDl>();
            services.AddScoped<Isupplierdl, Supplierdl>();
            services.AddScoped<IBatchdetailsDl,BatchdetailsDl>();
            services.AddScoped<IBatchesDl, BatchesDl>();
            services.AddScoped<IProductsDl,ProductsDl>();
            services.AddScoped<ISupplierbillDl, SupplierbillDl>();
            services.AddScoped<ISbilldetailsDl, SbilldetailsDl>();


            //Bl Layer
            services.AddScoped<ICustomerBl, CustomerBl>();
            services.AddScoped<Isupplierbl, Supplierbl>();
            services.AddScoped<IbatchdetailsBl, BatchDetailsBl>();
            services.AddScoped<IBatchesBl, BatchesBl>();
            services.AddScoped<IOrderBl, OrderBl>();
            services.AddScoped<IProductBl, ProductsBl>();
            services.AddScoped<ISupplierBillBl, SupplierBillBl>();
            services.AddScoped<ISbilldetailsBl, SbilldetailsBl>();
            services.AddTransient<Supplierbillsform>();



            //UI Layer
            services.AddTransient<HomeContentform>();
            services.AddTransient<dashboardform>();
            services.AddTransient<AddCustomer>();
            services.AddTransient<CustomerForm>();
            services.AddTransient < supplierform>();
            services.AddTransient<OrdersMain>();
            services.AddTransient<PlacingOrder>();
            services.AddTransient<Addbatchdetailsform>();
            services.AddTransient<Batchform>();
            services.AddTransient<Addbatchform>();
            services.AddTransient<BatchDetailsform>();
            services.AddTransient<Addsupplier>();
            services.AddTransient<Productsform>();
            services.AddTransient<Addproductform>();
            services.AddTransient<Customersale>();





        }
    }    
}
