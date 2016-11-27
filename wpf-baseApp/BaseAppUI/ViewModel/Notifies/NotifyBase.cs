using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Notifies
{
   public abstract class NotifyBase:NotifyProperty
    {
        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value;
            OnPropertyChanged("IsOpen");
            if (!value)
                EnableMask = false;
            }
        }

        private bool _enableMask;

        public bool EnableMask
        {
            get { return _enableMask; }
            set { _enableMask = value;
            OnPropertyChanged("EnableMask");
            }
        }

        private bool _closing;

        public bool Closing
        {
            get { return _closing; }
            set { _closing = value;

            OnPropertyChanged("Closing");
            }
        }

        Action _action;
        public void Show(Action action=null)
        {

            _action = action;
            MainVM.Main.Notify = this;
           this.IsOpen = true;
       }

        public bool IsPopup { get; set; }
      

       private DelegateCommand _closeCommand;

       public DelegateCommand CloseCommand
       {
           get { return _closeCommand ?? (_closeCommand = new DelegateCommand(() => {

               this.Closing = true;

               if (this.EnableMask)
                   this.EnableMask = false;

               System.Threading.CancellationToken token = new System.Threading.CancellationToken();
               Task.Factory.StartNew(() => {
                 
                   token.WaitHandle.WaitOne(TimeSpan.FromSeconds(.3));
                   this.IsOpen = false;

                   Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                       MainVM.Main.Notify = null;
                       if (_action != null)
                           _action.Invoke();
                   }));
                 

               },token);

            

           })); }
       }

      
    
    }
}
