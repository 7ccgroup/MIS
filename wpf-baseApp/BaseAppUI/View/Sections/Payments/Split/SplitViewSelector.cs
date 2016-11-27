using BaseAppUI.Controls;
using BaseAppUI.ViewModel.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BaseAppUI.View.Sections.Payments.Split
{
   public class SplitViewSelector : DataTemplateSelector
    {
       public DataTemplate EvenlySplitView { get; set; }
        public DataTemplate AmountBySplitView { get; set; }
    

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var m = (SplitType)((ToggleItem)item).Value;
            if (m == SplitType.Evenly)
                return EvenlySplitView;
            else
                if (m== SplitType.ByAmount)
                    return AmountBySplitView;

            return base.SelectTemplate(item, container);
        }
    }
}
