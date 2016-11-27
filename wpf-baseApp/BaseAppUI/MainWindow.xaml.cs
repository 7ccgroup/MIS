using BaseAppUI.Configuration;
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
using BaseAppUI.Common;
using BaseAppUI.ViewModel.Sections;
using BaseAppUI.ViewModel;

namespace BaseAppUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GConfig.Synchronizer.StartSync();
        }

        public string _trackdata;
        public void SwipeDetect(object sender, KeyEventArgs e)
        {
            if (MainVM.Main.Content.SectionType == Model.SectionType.Payment)
            {
                if (e.Key != Key.Return)
                {
                    _trackdata += KeyToChar.GetCharFromKey(e.Key);
                }

                if (e.Key == Key.Return)
                {
                    _trackdata = _trackdata.Replace(" ", string.Empty);
                    Payment p = new Payment(GConfig.GCorder);
                    p.SwipeTransfer(_trackdata);
                    _trackdata = string.Empty;
                }
            }
            
        }
    }
}
