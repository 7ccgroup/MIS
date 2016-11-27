using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace BaseAppUI.Sdk
{
   public class TouchKeyboard
    {
        //private static bool IsSurfaceKeyboardAttached()
        //{
        //    //search attached devices
        //    SelectQuery Sq = new SelectQuery("Win32_Keyboard");
        //    ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher(Sq);
        //    ManagementObjectCollection osDetailsCollection = objOSDetails.Get();
           
        //    return osDetailsCollection.Count <= 1 ? true : false;
        //}

        public static void OpenTouchKeyboard(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
                if (!File.Exists(path))
                {
                    // older windows versions
                    path = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\osk.exe";
                }
                Process.Start(path);
                textBox.BringIntoView();//SetFocus so u dont lose focused area
            }
        }
        [DllImport("user32.dll")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_CLOSE = 0xF060;
        private const int SC_MINIMIZE = 0xF020;

        public static void CloseTouchKeyboard()
        {
            // retrieve the handler of the window  
            int iHandle = FindWindow("IPTIP_Main_Window", "");
            if (iHandle > 0)
            {
                // close the window using API        
                SendMessage(iHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
        }
    }
}
