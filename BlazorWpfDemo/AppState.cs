using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWpfDemo
{
    public class AppState
    {

        public BlazorWebView BlazorWebView { get; private set; }

        public AppState()
        {

        }

        public void Init(BlazorWebView webView)
        {
            BlazorWebView = webView;
        }

        public void OpenDevtools()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            BlazorWebView.WebView.CoreWebView2.OpenDevToolsWindow();
#pragma warning restore CA1416 // Validate platform compatibility
        }



    }
}
