using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlazorWpfDemo.Interfaces
{
    public interface IWindowManager
    {
        void Close();
        void EndMove();
        WindowState GetWindowState();
        void Maximize();
        void Minimize();
        void MouseDownDrag();
    }
}
