using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Paftax.Pafta.Revit2026.Services
{
    public sealed partial class RevitWindowService(nint windowHandle) : IDisposable
    {
        private readonly nint _revitHandle = windowHandle;
        private bool _disposed;

        private const int SW_RESTORE = 9;
        private const uint GW_HWNDNEXT = 2;

        #region Win32 Library Imports

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetForegroundWindow(nint hWnd);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool ShowWindow(nint hWnd, int nCmdShow);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool EnableWindow(nint hWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

        [LibraryImport("user32.dll")]
        private static partial nint GetTopWindow(nint hWndParent);

        [LibraryImport("user32.dll")]
        private static partial nint GetWindow(nint hWnd, uint uCmd);

        #endregion

        #region Public Methods

        public static Dispatcher GetDispatcher()
        {
            return Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        public void DisableRevit()
        {
            EnableWindow(_revitHandle, false);
        }

        public void EnableRevit()
        {
            EnableWindow(_revitHandle, true);
        }

        /// <summary>
        /// Restore Revit window and bring to foreground only if it is topmost in all Windows.
        /// </summary>
        public void RestoreRevit()
        {
            if (_disposed) return;

            EnableWindow(_revitHandle, true);
            ShowWindow(_revitHandle, SW_RESTORE);

            nint topWindow = GetTopWindow(0);
            nint secondWindow = GetWindow(topWindow, GW_HWNDNEXT);

            if (_revitHandle == topWindow)
            {
                SetForegroundWindow(_revitHandle);
            }
            else if (_revitHandle == secondWindow)
            {
                // Only restore without forcing topmost
                ShowWindow(_revitHandle, SW_RESTORE);
            }
            else
            {
                ShowWindow(_revitHandle, SW_RESTORE);
            }
        }

        /// <summary>
        /// Block Revit while the given dialog window is open.
        /// </summary>
        public void BlockRevitWhile(Window dialog)
        {
            ArgumentNullException.ThrowIfNull(dialog);

            DisableRevit();
            dialog.Closed += (_, __) => RestoreRevit();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            RestoreRevit();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
