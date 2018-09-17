using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StartupLoader.Models;
using StartupLoader.ViewModels;
using System.Threading;
using NLog;


namespace StartupLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Mutex mutex;
        private static string appGuid = "70a603d1-7737-4f7f-ad18-8face3092c26";
        private LoaderViewModel lm;

        public MainWindow()
        {
            InitializeComponent();
            string mutex_id = @"Global\" + appGuid;
            mutex = new Mutex(false, mutex_id);
            lm = new LoaderViewModel(new Loader());
            DataContext = lm;
            Loaded += ToolWindow_Loaded;
            Loaded += CheckMutex;
            this.Closing += this.MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (btn_close.IsEnabled == false)
            {
                e.Cancel = true;
            }
        }

        // Prep stuff needed to remove close button on window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void CheckMutex(object sender, RoutedEventArgs e)
        {
            if (!mutex.WaitOne(0, false))
            {
                Application.Current.Shutdown();
            }
            else
            {
                lm.Load();
            }
        }
    }
}
