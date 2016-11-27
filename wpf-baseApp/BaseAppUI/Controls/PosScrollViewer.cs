using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BaseAppUI.Controls
{
   public class PosScrollViewer:ScrollViewer
    {
       public PosScrollViewer()
       {
           PanningMode = System.Windows.Controls.PanningMode.VerticalOnly;
           VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
       }
       public override void OnApplyTemplate()
       {
           base.OnApplyTemplate();
           this.ManipulationBoundaryFeedback += PosScrollViewer_ManipulationBoundaryFeedback;
       }

       void PosScrollViewer_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
       {
           e.Handled = true;
       }
    }
}
