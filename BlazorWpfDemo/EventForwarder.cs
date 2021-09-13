﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWpfDemo
{
#pragma warning disable CS0618 // Type or member is obsolete
    [ClassInterface(ClassInterfaceType.AutoDual)]
#pragma warning restore CS0618 // Type or member is obsolete
    [ComVisible(true)]
    public class EventForwarder
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        readonly IntPtr target;

        public EventForwarder(IntPtr target)
        {
            this.target = target;
        }

        public void MouseDownDrag()
        {
            ReleaseCapture();
            SendMessage(target, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }
}
