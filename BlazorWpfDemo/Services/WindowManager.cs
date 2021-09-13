using BlazorWpfDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

namespace BlazorWpfDemo.Services
{
    public class WindowManager : IWindowManager
    {
        public WindowManager()
        {
        }

        public WindowState GetWindowState()
        {
            return Application.Current.MainWindow.WindowState;
        }

        public void Maximize()
        {
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        public void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }

        public void MouseDownDrag()
        {
           EventForwarder eventForwarder = new EventForwarder(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            eventForwarder.MouseDownDrag();
        }
        public void EndMove()
        {
            
        }
      
    }
}
