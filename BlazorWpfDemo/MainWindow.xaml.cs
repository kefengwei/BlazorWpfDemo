using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace BlazorWpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Web.WebView2.Core.DevToolsProtocolExtension.DevToolsProtocolHelper _cdpHelper;
        Microsoft.Web.WebView2.Core.DevToolsProtocolExtension.DevToolsProtocolHelper cdpHelper
        {
            get
            {
                if (WebViewContainer.WebView == null || WebViewContainer.WebView.CoreWebView2 == null)
                {
                    throw new Exception("Initialize WebView before using DevToolsProtocolHelper.");
                }

                if (_cdpHelper == null)
                {
                    _cdpHelper = WebViewContainer.WebView.CoreWebView2.GetDevToolsProtocolHelper();
                }

                return _cdpHelper;
            }
        }

        private readonly AppState _appState;
        private IntPtr _windowHandle;
        public MainWindow()
        {
            this.MinWidth = 1400;
            this.MinHeight = 900;

            this.MaxHeight = SystemParameters.WorkArea.Height + 12;
            this.MaxWidth = SystemParameters.WorkArea.Width + 12;

            var startup = new Startup(_windowHandle);
            var provider = startup.BuildService();

            Resources.Add("services", provider);
            InitializeComponent();
            _windowHandle = new WindowInteropHelper(this).Handle;

            _appState = provider.GetService<AppState>();

            _appState?.Init(WebViewContainer);
            WebViewContainer.Loaded += WebViewContainer_Loaded;

        }

        private void WebViewContainer_Loaded(object sender, RoutedEventArgs e)
        {

            InitializeAsync();



        }

        async void InitializeAsync()
        {
            await WebViewContainer.WebView.EnsureCoreWebView2Async();
            WebViewContainer.WebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            WebViewContainer.WebView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
            WebViewContainer.WebView.CoreWebView2.AddWebResourceRequestedFilter("*://native/*", CoreWebView2WebResourceContext.All);
            WebViewContainer.WebView.CoreWebView2.AddWebResourceRequestedFilter("*://emmbeded/*", CoreWebView2WebResourceContext.All);

        }


        private void CoreWebView2_WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            var uri = new Uri(e.Request.Uri);
            if (uri.Host == "native")
            {
                var localPath = uri.LocalPath[1..];
                var fileInfo = new FileInfo(localPath);
                if (fileInfo.Exists)
                {
                    var response = new HttpResponseMessage();

                    e.Response = WebViewContainer.WebView.CoreWebView2.Environment.CreateWebResourceResponse(File.OpenRead(localPath), 200, "OK", $"Content-Type: {e.ResourceContext.ToString()}");
                }
                else
                {
                    e.Response = WebViewContainer.WebView.CoreWebView2.Environment.CreateWebResourceResponse(null, 404, "NOTFOUND", $"Content-Type: {e.ResourceContext.ToString()}");

                }
            }
            if (uri.Host == "emmbeded")
            {
                var localPath = uri.LocalPath[1..];
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{localPath.Replace("/", ".")}");
                    var response = new HttpResponseMessage();
                    e.Response = WebViewContainer.WebView.CoreWebView2.Environment.CreateWebResourceResponse(stream, 200, "OK", "");
                }
            }
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            System.Console.WriteLine(e);
        }


        private void WebViewContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                _appState.OpenDevtools();
            }
            if (e.Key == Key.F5)
            {
                WebViewContainer.WebView.Reload();
            }
        }
    }



    public partial class Main { }
}
