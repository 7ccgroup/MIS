using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shapes;

namespace BaseAppUI.Controls
{
   public class SearchBox:InputControl
    {
       public override void OnApplyTemplate()
       {
           base.OnApplyTemplate();

           this.KeyDown += SearchBox_KeyDown;
       }

       void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
       {

           if (e.Key == System.Windows.Input.Key.Return)
           {
               var obj = (SearchBox)sender;
               obj.Command.Execute(obj.Text);
           }


       }
    }

  
}
