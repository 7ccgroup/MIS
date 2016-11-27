using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;
using BaseAppUI.Common;
using System.Timers;

namespace BaseAppUI.ViewModel.Sections
{
    public class Login : NotifyProperty, ISection
    {


        MainVM _main;
        private string _userPassCode;
        public string vmUserCode
        { get { return _userPassCode; } set { _userPassCode = value; OnPropertyChanged("vmUserCode"); }
                   }
        private string _userPassCodeDisplay;
        public string vmUserCodeDisplay
        {
            get { return _userPassCodeDisplay; }
            set { _userPassCodeDisplay = value;  OnPropertyChanged("vmUserCodeDisplay");
            }
        }

        public Login(MainVM main)
        {

            _main = main;
            vmUserCode = "";
            vmUserCodeDisplay = "";
            //  _main.SwitchToView(Model.SectionType.Report);


        }

        DelegateCommand _closeWindowCommand;

        public DelegateCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new DelegateCommand(() =>
                {

                    Application.Current.Shutdown();

                }));
            }
        }



        private DelegateCommand<SectionType> _switchToSectionCommand;

        public DelegateCommand<SectionType> SwitchToSectionCommand
        {
            get
            {
                return _switchToSectionCommand ?? (_switchToSectionCommand = new DelegateCommand<SectionType>((e) =>
                {

                    _main.SwitchToView(e);


                }));
            }
        }

        private DelegateCommand _loginCommand;

        public DelegateCommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new DelegateCommand(() =>
                {

                    MessageBox.Show("Hello this is good start, have to complete this IA");
                     _main.SwitchToView(SectionType.Dashboard);

                }));
            }

        }

        private DelegateCommand<string> _numberPressCommand;

        public DelegateCommand<string> NumberPressCommand
        {
            get
            {
                string _EnterValue;
                return _numberPressCommand ?? (_numberPressCommand = new DelegateCommand<string>((e) =>
                {
                    _EnterValue = e.ToString();


                    if (_EnterValue == "X" && vmUserCodeDisplay.Length>0 && vmUserCode.Length>0 && vmUserCode != null && vmUserCodeDisplay != null)
                    {
                        vmUserCode = vmUserCode.Remove(vmUserCode.Length - 1);
                        vmUserCodeDisplay=vmUserCodeDisplay.Remove(vmUserCodeDisplay.Length - 1); ;
                    }
                    else
                    {
                        vmUserCode += e.ToString();
                        vmUserCodeDisplay += "*";
                    }
                    if (vmUserCode.Length == 4)  //logic to retreive user code and validate will be in a method......
                    {
                        if (vmUserCode == "1635" || vmUserCode=="2236" || vmUserCode=="2334" || vmUserCode=="9955")
                        {
                           
                            
                            GConfig.POSuser = vmUserCode; //Initialize the user.....
                            if (GConfig.POSuser == "9955")
                            {
                                GConfig.POSuserAccess = "ADMIN";
                                MainVM.Main.Head.UserAccess = "User Access: Full";
                            }
                            else
                            {
                                GConfig.POSuserAccess = "USER";
                                MainVM.Main.Head.UserAccess = "User Access: Restricted";
                            }

                            vmUserCodeDisplay = "";
                            vmUserCode = "";


                            _main.SwitchToView(SectionType.Dashboard);
                           
                        }
                        else
                        {
                            // MessageBox.Show("Invalid User Code");

                            vmUserCode = "";
                            vmUserCodeDisplay = "";
                            //var _timer = new Timer(10000);
                            //_timer.Start();
                            //_timer.Stop();
                            //vmUserCodeDisplay = "";

                        }
                    }
                    
                   // MessageBox.Show("Hello, have to complete this IA -" + e.ToString() + "    " + vmUserCode.ToString());

                }));
            }

        }

            private IList<LoginNumPadVM> _numPad;
              public IList<LoginNumPadVM> NumPad
              {
                  get
                  {
                      return _numPad ?? (_numPad = new List<LoginNumPadVM> {

                       new LoginNumPadVM("1",LoginNumPad.Key1),
                       new LoginNumPadVM("2",LoginNumPad.Key2),
                       new LoginNumPadVM("3",LoginNumPad.Key3),
                       new LoginNumPadVM("4",LoginNumPad.Key4),

                       new LoginNumPadVM("5",LoginNumPad.Key5),

                       new LoginNumPadVM("6",LoginNumPad.Key6),

                       new LoginNumPadVM("7",LoginNumPad.Key7),

                       new LoginNumPadVM("8",LoginNumPad.Key8),

                       new LoginNumPadVM("9",LoginNumPad.Key9),
                        
                          new LoginNumPadVM("*",LoginNumPad.KeyStar),
                          new LoginNumPadVM("0",LoginNumPad.Key0),
                          new LoginNumPadVM("X",LoginNumPad.KeyHash),


                     });
                  }

              }


        private string _licenseCompanyName;
      public string LicenseCompanyName
        {
            get { return GConfig.POS_Setup.vCorpName; }
            set { _licenseCompanyName = GConfig.POS_Setup.vCorpName; }
        }

      
        
      
        public void ManipulationBoundaryFeedbackHandler
  (object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        public SectionType PreviousSection { get; set; }
        public SectionType SectionType
        {
            get { return Model.SectionType.Login; }
        }
    }
}
