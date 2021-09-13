using BlazorWpfDemo.Interfaces;
using BlazorWpfDemo.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWpfDemo
{
    public class Startup
    {

        private readonly ServiceCollection services;
        public Startup(IntPtr handle)
        {
            services = new ServiceCollection();
            ConfigureServices();
        }
        private void ConfigureServices()
        {
            services.AddBlazorWebView();
            services.AddSingleton<AppState>();
            services.AddSingleton<IErrorBoundaryLogger, MyErrorBoundaryLogger>();
            services.AddSingleton<IWindowManager, WindowManager>();
        }

        public class MyErrorBoundaryLogger : IErrorBoundaryLogger
        {
            private readonly ILogger<MyErrorBoundaryLogger> logger;

            public MyErrorBoundaryLogger(ILogger<MyErrorBoundaryLogger> logger)
            {
                this.logger = logger;
            }
            public  async ValueTask LogErrorAsync(Exception exception)
            {
                await Task.CompletedTask;
                logger.LogError("ErrorBoundary", exception);
            }
        }

        public ServiceProvider BuildService()
        {
            return services.BuildServiceProvider();
        }
    }
}
