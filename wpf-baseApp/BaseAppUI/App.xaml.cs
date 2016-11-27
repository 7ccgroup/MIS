using BaseAppUI.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BaseAppUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            if (BaseAppUI.Properties.Settings.Default.DatabaseLocation!="C:")
                AppDomain.CurrentDomain.SetData("DataDirectory", BaseAppUI.Properties.Settings.Default.DatabaseLocation);
            base.OnStartup(e);

            try { 

            GConfig.Initialize();
          
            var vm = new ViewModel.MainVM();


            var win = new MainWindow
            {
                DataContext=vm
            };

            win.Show();
                }
            catch(Exception ex){

                MessageBox.Show(ex.Message.ToString());
            
            }

            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent,
                            new RoutedEventHandler(GotFocus_Event), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.LostFocusEvent,
                                    new RoutedEventHandler(LostFocus_Event), true);

        }
        private static void GotFocus_Event(object sender, RoutedEventArgs e)
        {
            Sdk.TouchKeyboard.OpenTouchKeyboard(sender, e);
        }
        private void LostFocus_Event(object sender, EventArgs e)
        {
            Sdk.TouchKeyboard.CloseTouchKeyboard();
        }
    }
}
