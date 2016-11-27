using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BaseAppUI.Controls
{
   public class DateTimeViewer:Control
    {
       public static readonly DependencyProperty DateTimeValueProperty;
       DispatcherTimer _timer; 
       static DateTimeViewer()
       {
           DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimeViewer), new FrameworkPropertyMetadata(typeof(DateTimeViewer)));

           DateTimeValueProperty = DependencyProperty.Register("DateTimeValue", typeof(string), typeof(DateTimeViewer), new PropertyMetadata(null, null));

       }

       public DateTimeViewer()
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

           SetTime();

           if (!DesignerProperties.GetIsInDesignMode(this))
           {
             

               _timer = new DispatcherTimer();
               _timer.Interval = TimeSpan.FromMinutes(1);
               _timer.Tick += _timer_Tick;
               _timer.Start();
           }
       }

      

    
     
       void _timer_Tick(object sender, EventArgs e)
       {
           SetTime();
       }

       public void SetTime()
       {
           this.SetValue(DateTimeValueProperty, DateTime.Now.ToString("MMMM dd, yyyy | h:mm tt", CultureInfo.CreateSpecificCulture("en-US")));

       }



       public string DateTimeValue
       {
           get
           {
               return (string)GetValue(DateTimeValueProperty);
           }
           set
           {
               SetValue(DateTimeValueProperty, value);
           }
       }

      
    

      
    }
}
