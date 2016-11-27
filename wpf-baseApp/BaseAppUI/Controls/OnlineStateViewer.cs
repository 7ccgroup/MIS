using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BaseAppUI.Controls
{
    public class OnlineStateViewer : Control
    {
        public static readonly DependencyProperty IsOnlineProperty;
        DispatcherTimer _timer;
        static OnlineStateViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OnlineStateViewer), new FrameworkPropertyMetadata(typeof(OnlineStateViewer)));

            IsOnlineProperty = DependencyProperty.Register("IsOnline", typeof(bool), typeof(OnlineStateViewer), new PropertyMetadata(false, null));

        }

       
        public OnlineStateViewer()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Loaded += (s, e) =>
                {

                    Window.GetWindow(this)
                          .Closing += (s1, e1) =>
                          {
                              if (_timer != null)
                                  _timer.Stop();
                          };
                };
            }

           
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Set();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
               
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(5);
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }
        }





        void _timer_Tick(object sender, EventArgs e)
        {
            Set();
        }

        public void Set()
        {

            bool newv = CheckNet();
            bool oldv = (bool)this.GetValue(IsOnlineProperty);
           if(oldv!=newv)
               this.SetValue(IsOnlineProperty, CheckNet());

        }

       

        private static bool CheckNet()
        {
          return  System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        public bool IsOnline
        {
            get
            {

                return (bool)GetValue(IsOnlineProperty);
            }
            set
            {
                SetValue(IsOnlineProperty, value);
            }
        }





    }
}
