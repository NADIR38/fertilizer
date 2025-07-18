﻿using fertilizesop.BL.bl;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLinterfaces;
using fertilizesop.Interfaces.DLInterfaces;
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
        //public static IServiceProvider ServiceProvider { get; private set; }
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            configurationservices(services);
            ServiceProvider = services.BuildServiceProvider();
            var mainform = ServiceProvider.GetRequiredService<Addcustomer>();
            Application.Run(mainform);
        }

        public static void configurationservices (IServiceCollection services)
        {
            services.AddScoped<Icustomerbl, Customerbl>();

            services.AddScoped<Icustomerdl , custumerdl>();

            services.AddTransient<HomeContentform>();
            services.AddTransient<dashboardform>();
            services.AddTransient<Addcustomer>();

        }
    }    
}
