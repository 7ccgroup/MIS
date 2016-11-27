using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace BaseAppUI.ViewModel.Notifies
{
    public class QuickTip : NotifyProperty
    {
        public decimal? Value { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
   public class TipSignModifier:NotifyBase
    {
       OrderVM _order;
       private decimal? _tipRate;
       private ObservableCollection<QuickTip> _quickTips;
       private DelegateCommand<QuickTip> _selectTipCommand;
       private DelegateCommand _doneSigningCommand;
       private DelegateCommand _clearSigningCommand;

       



       public TipSignModifier(OrderVM order)
       {
           _order = order;

           Total = _order.AllTotal;

           Introduction = @"I, George Foe, agree to pay the above total amount to Restaurant Name according to my card issuer agreement. 
Visa card ending in 520. Authorzation code:dR3gEz.";

           SelectTipCommand.Execute(QuickTips[1]);
           
       }

       public decimal Total { get; set; }
      

       public decimal? TipRate
       {
           get { return _tipRate; }
           set { 
               _tipRate = value;
               CalculateValues();

           }
       }
       public decimal TipValue { get; set; }
       public decimal AllTotal { get; set; }

       public string Introduction { get; set; }
       private void CalculateValues()
       {

           TipValue = Total * TipRate?? 0;
           AllTotal = Total + TipValue;
           OnPropertyChanged("TipRate");
           OnPropertyChanged("TipValue");
           OnPropertyChanged("AllTotal");
       }

       private QuickTip SelectedQuickTip { get; set; }
       public ObservableCollection<QuickTip> QuickTips
       {
           get
           {
               return _quickTips ?? (_quickTips = new ObservableCollection<QuickTip>() { 
               new QuickTip{Value=null}, 
               new QuickTip{Value=.1m},
               new QuickTip{Value=.15m},
               new QuickTip{Value=.2m}});
           }
       }
      

       public DelegateCommand<QuickTip> SelectTipCommand
       {
           get
           {
               return _selectTipCommand ?? (_selectTipCommand = new DelegateCommand<QuickTip>((e) =>
               {

                   if (e == null) return;
                   if (SelectedQuickTip != e)
                   {

                       if (SelectedQuickTip != null)
                           SelectedQuickTip.IsSelected = false;


                       SelectedQuickTip = e;
                       SelectedQuickTip.IsSelected = true;

                       TipRate = SelectedQuickTip.Value;
                   }





               }));
           }
       }

       private InkCanvas _inkCanvas;

       public InkCanvas InkCanvas
       {
           get {
               if (_inkCanvas == null)
               {
                   var da = new DrawingAttributes();
                   da.Color = Colors.Black;
                   da.IsHighlighter = true;
                   da.IgnorePressure = true;
                   da.StylusTip = StylusTip.Ellipse;
                   da.Height = 10;
                   da.Width = 10;
                   _inkCanvas = new InkCanvas {  DefaultDrawingAttributes = da };
               }

               return _inkCanvas;}
       }

       public DelegateCommand DoneSigningCommand
       {
           get
           {
               return _doneSigningCommand ?? (_doneSigningCommand = new DelegateCommand(() => {

                   //

                   if(TipRate.HasValue)
                       _order._orderHeader.dOrderTip = TipValue;


                   this.CloseCommand.Execute(null);

               }));
           }
       }
       public DelegateCommand ClearSigningCommand
       {
           get
           {
               return _clearSigningCommand ?? (_clearSigningCommand = new DelegateCommand(() =>
               {
                   InkCanvas.Strokes.Clear();

               }));
           }
       }

    }
}
