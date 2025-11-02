using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Paftax.Pafta.Revit2026.Services
{
    internal class RevitWindowService(nint windowHandle) : IDisposable
    {
        private readonly IntPtr _revitHandle = windowHandle;
        private bool _disposed;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        private const int SW_RESTORE = 9;

        public Dispatcher GetDispatcher()
        {
            return Application.Current.Dispatcher;
        }

        public void DisableRevit()
        {
            EnableWindow(_revitHandle, false);
        }

        public void EnableRevit()
        {
            EnableWindow(_revitHandle, true);
        }

        public void RestoreRevit()
        {
            if (_disposed) return;

            EnableWindow(_revitHandle, true);
            ShowWindow(_revitHandle, SW_RESTORE);
            SetForegroundWindow(_revitHandle);
        }

        public void BlockRevitWhile(Window dialog)
        {
            ArgumentNullException.ThrowIfNull(dialog);

            DisableRevit();

            dialog.Closed += (s, e) => RestoreRevit();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            RestoreRevit();
        }
    }
}
