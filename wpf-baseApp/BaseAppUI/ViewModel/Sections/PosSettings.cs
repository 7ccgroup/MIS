using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using BaseAppUI.ViewModel.Sections.Partials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;
using BaseAppUI.Common;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.View.Notifies;
using System.ComponentModel;
using System.Windows.Controls;
using BaseAppUI.Configuration.Sync;
using System.Threading;

namespace BaseAppUI.ViewModel.Sections
{
    public class PosSettings : NotifyProperty, ISection
    {

        
        MainVM _main;
        SettingsListForDisplay _selectedCategory;
        
        private IList<SettingsListForDisplay> SelectedSetting { get; set; }
        private IList<SettingsListForDisplay> _settingscategory;
        private IList<SettingScreen> _settingScreen;
        private SettingScreenEdit _settingScreenEdit; 

        public PosSettings(MainVM main)
        {

           
            SettingApplicationVisible = Visibility.Visible;
            SettingSyncVisible = Visibility.Collapsed;

           

            _main = main;
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

  

        private DelegateCommand _returnCommand;

        public DelegateCommand ReturnCommand
        {
            get
            {
                return _returnCommand ?? (_returnCommand = new DelegateCommand(() =>
                {

                    _main.SwitchToView(SectionType.Dashboard);

                }));
            }

        }
    

        private SettingsBuilder _settingBuilder = new SettingsBuilder();
        
        private IList<SettingsListForDisplay> _settings;
        public IList<SettingsListForDisplay> SettingsDisplay
        {
            get
            {
                _settings = _settingBuilder.SettingsListDisplayAdvance();
                return _settings;
            }
        }

      

        public IList<SettingsListForDisplay> SettingsCategory
        {
            get
            {


                _settingscategory = new List<SettingsListForDisplay> {
                
                new SettingsListForDisplay("Application",false),
                //new SettingsListForDisplay("System",false),
                //new SettingsListForDisplay("User",false),
                new SettingsListForDisplay("Sync",false),
                //new SettingsListForDisplay("Codes",false),
                };

                _settingscategory.First().IsSelected = true;
                SelectedCategory = _settingscategory.First();
                SelectedCategory.IsSelected = true;
                //         _settingscategory = _settingBuilder.SettingsCategoryListDisplay();

                return _settingscategory;

            }
            set { _settingscategory = value; }
        }

     
        //This property will enable "Edit" Button if Item is selected.
        public bool RecordSelected
        {
            get
            {
                if (SelectMasterSetting != null)
                    return true;
                else
                    return false;
            }

        }

      
        //private DelegateCommand<SettingScreen> _selectMasterSettingsCommand;

        //public DelegateCommand<SettingScreen> SelectMasterSettingsCommand
        //{
        //    get
        //    {
        //        return _selectMasterSettingsCommand ?? (_selectMasterSettingsCommand = new DelegateCommand<SettingScreen>((e) =>
        //        {


        //            SelectMasterSetting = e; //select the item on click from screen SAA.
        //           // SelectMasterSetting.IsSelected = true;

        //            //  OnPropertyChanged("SelectMasterItem101");
        //            //  MessageBox.Show(_selectMasterItem.itemID.ToString());///


        //        }));
        //    }
        //}
        private string _syncLog;
        public string DisplaySyncLog
        {
            get { return _syncLog;   }
           set {
                _syncLog = value;
                OnPropertyChanged("DisplaySyncLog");
            }
        }
        private SettingScreenEdit _selectMasterSetting;
        public SettingScreenEdit SelectMasterSetting
        {
            get { return _selectMasterSetting; }
            set
            {
                _selectMasterSetting = value;
                //    _main.ItemEntry.SelctedItemForEdit = value;
             //   SelectMasterSetting.IsSelected = true;
                OnPropertyChanged("SelectMasterSetting");
                OnPropertyChanged("RecordSelected");
            }
        }

        private DelegateCommand<TextBox> _syncItemsCommand;

        public DelegateCommand<TextBox> SyncItemsCommand
        {
            get
            {
               
                return _syncItemsCommand ?? (_syncItemsCommand = new DelegateCommand<TextBox>((e) =>
                {
                    TextBoxOutputter outputter;
                    outputter = new TextBoxOutputter(e);
                    Console.SetOut(outputter);
                    Console.WriteLine("Sync Started");

                    GConfig.SyncRequest = true;
                    GConfig.Synchronizer.StartSync();
                    _syncLog = GConfig.SyncLog;
                    //  OnPropertyChanged("SelectMasterItem101");
                    //  MessageBox.Show(_selectMasterItem.itemID.ToString());


                }));
            }
        }

        private string _logDisplay = "";
        public string LogDisplay
        {
            get
            {
                return _logDisplay;
            }
            set
            {
                _logDisplay = value;
                OnPropertyChanged("LogDisplay");
            }
        }
        void TimerTick(object state)
        {
            var who = state as string;
            Console.WriteLine(who);
      //      LogDisplay = DisplayTextBox.Text;
            
        }
        private TextBox DisplayTextBox = new TextBox();
        private DelegateCommand<TextBox> _syncShowLogCommand;
        public DelegateCommand<TextBox> SyncShowLogCommand
        {
            get
            {

                return _syncShowLogCommand ?? (_syncShowLogCommand = new DelegateCommand<TextBox>((e) =>
                {
                    TextBoxOutputter outputter;
                    outputter = new TextBoxOutputter(e);
                    Console.SetOut(outputter);
                    Console.WriteLine("Sync Started");

                    var timer1 = new Timer(TimerTick, "Timer1", 0, 1000);
                    var timer2 = new Timer(TimerTick, "Timer2", 0, 500);
                    DisplaySyncLog = "" ;
                    //  OnPropertyChanged("SelectMasterItem101");
                    //  MessageBox.Show(_selectMasterItem.itemID.ToString());


                }));
            }
        }
        private ObservableCollection<SettingsCategoryListForDisplay> _categories;
        private DelegateCommand<string> _masterSettingsSearchCommand;



        public ObservableCollection<SettingsCategoryListForDisplay> CategoriesList
        {
            get
            {

                if (_categories == null)
                {
                    _categories = _settingBuilder.SettingsCategoriesListDisplay();
                    //new ObservableCollection<CategoryVM>(GConfig.POS_Setup.ItemCategories.Where(n => n.vCategoryCode != "all-items").Select(n => new CategoryVM(n, this)));
                    CategoriesInitialized = true;
                }
                return _categories;
            }
            set
            {
                _categories = value;

                OnPropertyChanged("Categories");
            }
        }

        public bool CategoriesInitialized { get; set; }


        public SettingsListForDisplay SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;

                OnPropertyChanged("SettingsDisplayByCategory"); //saa changed to ItemsDisplayByCategory it was SelectedCategory
            }
        }
        private Visibility _settingSyncVisible;
        public Visibility SettingSyncVisible
        {
            get { return _settingSyncVisible; }
            set
            {
                _settingSyncVisible = value;
                OnPropertyChanged("SettingSyncVisible");
            }
        }
        private Visibility _settingApplicationVisible;
        public Visibility SettingApplicationVisible
        {
            get { return _settingApplicationVisible; }
            set
            {
                _settingApplicationVisible = value;
                OnPropertyChanged("SettingApplicationVisible");
            }
        }
        private Visibility _settingSystemVisible;
        public Visibility SettingSystemVisible
        {
            get { return _settingSystemVisible; }
            set
            {
                _settingSystemVisible = value;
                OnPropertyChanged("SettingSystemVisible");
            }
        }

        private Visibility _settingUserVisible;
        public Visibility SettingUserVisible
        {
            get { return _settingUserVisible; }
            set
            {
                _settingUserVisible = value;
                OnPropertyChanged("SettingSystemVisible");
            }
        }
        private DelegateCommand<SettingsListForDisplay> _selectCategoryCommand;

        public DelegateCommand<SettingsListForDisplay> SelectCategoryCommand
        {
            get
            {
                return _selectCategoryCommand ?? (_selectCategoryCommand = new DelegateCommand<SettingsListForDisplay>((e) => {

                    if (e == null) return;
                    if (SelectedCategory != e)
                    {

                        if (SelectedCategory != null)
                            SelectedCategory.IsSelected = false;


                        SelectedCategory = e;
                        SelectedCategory.IsSelected = true;
                        SettingSyncVisible = Visibility.Collapsed;
                        SettingApplicationVisible = Visibility.Collapsed;
                        SettingSystemVisible = Visibility.Collapsed;
                        SettingUserVisible = Visibility.Collapsed;

                        if (SelectedCategory.Name == "Application")
                        {
                            //POSReportDocument = CreateZreport();
                            SettingSyncVisible = Visibility.Collapsed;
                            SettingApplicationVisible = Visibility.Visible;
                            SettingSystemVisible = Visibility.Collapsed;
                            _settingScreen = new List<SettingScreen> {
                                 new SettingScreen("Application1","Value222","V123"),
                                 new SettingScreen("Application2","Value333","V123"),

                                  };
                           


                        }
                        if (SelectedCategory.Name == "System")
                        {
                            //POSReportDocument = CreateZreport();
                            SettingSyncVisible = Visibility.Collapsed;
                            SettingApplicationVisible = Visibility.Collapsed;
                            SettingSystemVisible = Visibility.Visible;

                        }
                        if (SelectedCategory.Name == "Sync")
                        {
                            //POSReportDocument = CreateZreport();
                            SettingSyncVisible = Visibility.Visible;
                            SettingApplicationVisible = Visibility.Collapsed;
                            SettingSystemVisible = Visibility.Collapsed;
                            
                        }
                        if (SelectedCategory.Name == "User")
                        {
                            //POSReportDocument = CreateZreport();
                            SettingUserVisible = Visibility.Visible;
                            SettingApplicationVisible = Visibility.Collapsed;
                            SettingSystemVisible = Visibility.Collapsed;
                            SettingUserVisible = Visibility.Collapsed;

                        }
                    }

                }));
            }
        }



        /// <summary>
        /// This is his for properties taken from Register View Model
        public void ManipulationBoundaryFeedbackHandler
  (object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        public SectionType PreviousSection { get; set; }
        public SectionType SectionType
        {
            get { return Model.SectionType.Settings; }
        }


        //private DelegateCommand<SettingsListForDisplay> _selectLineCommand;

        //public DelegateCommand<SettingsListForDisplay> SelectLineCommand
        //{
        //    get
        //    {
        //        return _selectLineCommand ?? (_selectLineCommand = new DelegateCommand<SettingsListForDisplay>((e) => {

        //            new SettingEdit(e).Show(null);

        //            //    new ItemEntryView(e).Show(null);


        //        }));
        //    }

        //}
        
        private DelegateCommand<SettingScreenEdit> _selectSettingEditComman;

        public DelegateCommand<SettingScreenEdit> SelectSettingEditCommand
        {
            get
            {
                return _selectSettingEditComman ?? (_selectSettingEditComman = new DelegateCommand<SettingScreenEdit>((e) => {
                  //  MessageBox.Show(e.SettingGroupid);

                    new SettingEdit(e).Show(null);
                    SettingDisplay(e.SettingGroupid);
                }));
            }

        }

        private DelegateCommand<string> _settingsCommand;

        public DelegateCommand<string> SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new DelegateCommand<string>((e) => {
                    if (SelectedCategory.Name == "Application")
                    {
                        SettingDisplay(e);
                    }

                 

                }));
            }

        }
        public void SettingDisplay(string PropertyGroupId)
        {

            _settingScreenEdit = new SettingScreenEdit();
            _settingScreenEdit.SettingGroupid = PropertyGroupId;
            if (PropertyGroupId == "Name")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "BusinessName";
                _settingScreenEdit.SettingProperty02Name = "BusinessAddress1";
                _settingScreenEdit.SettingProperty03Name = "BusinessAddress2";
                _settingScreenEdit.SettingProperty04Name = "BusinessTel";
                _settingScreenEdit.SettingProperty05Name = "";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = "";

                _settingScreenEdit.SettingDisplay01Label = "Business Name";
                _settingScreenEdit.SettingDisplay02Label = "Business Address1";
                _settingScreenEdit.SettingDisplay03Label = "Business Address2";
                _settingScreenEdit.SettingDisplay04Label = "Business Telephone";
                _settingScreenEdit.SettingDisplay05Label = "";

            }

            if (PropertyGroupId == "Tax")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "Tax1_State";
                _settingScreenEdit.SettingProperty02Name = "Tax2_Village";
                _settingScreenEdit.SettingProperty03Name = "Tax_overall";
                _settingScreenEdit.SettingProperty04Name = "";
                _settingScreenEdit.SettingProperty05Name = "";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = "";

                _settingScreenEdit.SettingDisplay01Label = "State Tax";
                _settingScreenEdit.SettingDisplay02Label = "Village Tax";
                _settingScreenEdit.SettingDisplay03Label = "Overall Tax";
                _settingScreenEdit.SettingDisplay04Label = "";
                _settingScreenEdit.SettingDisplay05Label = "";
            }

            if (PropertyGroupId == "ServiceFee")

            {
                _settingScreenEdit.SettingProperty01Name = "ServiceFee";
                _settingScreenEdit.SettingProperty02Name = "";
                _settingScreenEdit.SettingProperty03Name = "";
                _settingScreenEdit.SettingProperty04Name = "";
                _settingScreenEdit.SettingProperty05Name = "";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = "";
                //prepare for display of property



                _settingScreenEdit.SettingDisplay01Label = "Service Fee";
                _settingScreenEdit.SettingDisplay02Label = "";
                _settingScreenEdit.SettingDisplay03Label = "";
                _settingScreenEdit.SettingDisplay04Label = "";
                _settingScreenEdit.SettingDisplay05Label = "";
            }
            if (PropertyGroupId == "Addon")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "Addon201";
                _settingScreenEdit.SettingProperty02Name = "Addon202";
                _settingScreenEdit.SettingProperty03Name = "Addon203";
                _settingScreenEdit.SettingProperty04Name = "Addon204";
                _settingScreenEdit.SettingProperty05Name = "Addon205";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty05Name);
                //prepare for display of property

                _settingScreenEdit.SettingDisplay01Label = "Add-on 01";
                _settingScreenEdit.SettingDisplay02Label = "Add-on 02";
                _settingScreenEdit.SettingDisplay03Label = "Add-on 03";
                _settingScreenEdit.SettingDisplay04Label = "Add-on 04";
                _settingScreenEdit.SettingDisplay05Label = "Add-on 05";
            }

            if (PropertyGroupId == "CardKey")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "MerchantID";
                _settingScreenEdit.SettingProperty02Name = "UserID";
                _settingScreenEdit.SettingProperty03Name = "Pin";
                _settingScreenEdit.SettingProperty04Name = "";
                _settingScreenEdit.SettingProperty05Name = "";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = "";
                _settingScreenEdit.SettingDisplay05Value = "";
                //prepare for display of property


                _settingScreenEdit.SettingDisplay01Label = "Merchant ID";
                _settingScreenEdit.SettingDisplay02Label = "User ID";
                _settingScreenEdit.SettingDisplay03Label = "Pin";

            }
            if (PropertyGroupId == "Printer")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "POSMainPrinter";
                _settingScreenEdit.SettingProperty02Name = "POSKitchenPrinter";
                _settingScreenEdit.SettingProperty03Name = "POSExtraPrinterCount";
                _settingScreenEdit.SettingProperty04Name = "POSExtraPrinter01";
                _settingScreenEdit.SettingProperty05Name = "";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = "";

                _settingScreenEdit.SettingDisplay01Label = "Main Printer";
                _settingScreenEdit.SettingDisplay02Label = "Kitchen Printer";
                _settingScreenEdit.SettingDisplay03Label = "Extra Printer Count";
                _settingScreenEdit.SettingDisplay04Label = "2nd Kitchen Printer";

            }
            if (PropertyGroupId == "DatabaseLocation")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "DatabaseLocation";
                _settingScreenEdit.SettingProperty02Name = "DBstring";
                _settingScreenEdit.SettingProperty03Name = "EnableFrequentItemUpdate";
                _settingScreenEdit.SettingProperty04Name = "";
                _settingScreenEdit.SettingProperty05Name = "";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = "";

                _settingScreenEdit.SettingDisplay01Label = "Database Location";
                _settingScreenEdit.SettingDisplay02Label = "Database Filename";
                _settingScreenEdit.SettingDisplay03Label = "Frequent Item Tracker";

            }

            if (PropertyGroupId == "Workflow")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "TipSignModifierSwitch";
                _settingScreenEdit.SettingProperty02Name = "DashboardIncludePendingOrders";
                _settingScreenEdit.SettingProperty03Name = "LoginScreen";
                _settingScreenEdit.SettingProperty04Name = "EnableTableNumber";
                _settingScreenEdit.SettingProperty05Name = "EnableItemAdd";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty05Name);
                //prepare for display of property

                _settingScreenEdit.SettingDisplay01Label = "Tip Sign Modifier";
                _settingScreenEdit.SettingDisplay02Label = "Pending Orders on Dashboard";
                _settingScreenEdit.SettingDisplay03Label = "Login Screen";
                _settingScreenEdit.SettingDisplay04Label = "Enable Table Number";
                _settingScreenEdit.SettingDisplay05Label = "Enable Item Add";
            }

            if (PropertyGroupId == "Dataflow")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "Url";
                _settingScreenEdit.SettingProperty02Name = "CKey";
                _settingScreenEdit.SettingProperty03Name = "CSecret";
                _settingScreenEdit.SettingProperty04Name = "SyncMode";
                _settingScreenEdit.SettingProperty05Name = "AddonCount";

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                _settingScreenEdit.SettingDisplay05Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty05Name);
                //prepare for display of property

                _settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig("Url");
                _settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig("CKey");
                _settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig("CSecret");
                _settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig("SyncMode");
                _settingScreenEdit.SettingDisplay05Value = GlobalPosSettings.GetConfig("AddonCount");

                _settingScreenEdit.SettingDisplay01Label = "URL for Dataflow";
                _settingScreenEdit.SettingDisplay02Label = "Consumer Key";
                _settingScreenEdit.SettingDisplay03Label = "Consumer Secret";
                _settingScreenEdit.SettingDisplay04Label = "Sync Mode";
                _settingScreenEdit.SettingDisplay05Label = "Add-on Count";
            }
            if (PropertyGroupId == "Optimize")

            {
                //prepare for display of property
                _settingScreenEdit.SettingProperty01Name = "Frequency";
                _settingScreenEdit.SettingProperty02Name = "";
                _settingScreenEdit.SettingProperty03Name = "";
                _settingScreenEdit.SettingProperty04Name = "";
                _settingScreenEdit.SettingProperty05Name = "";

                //_settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty01Name);
                //_settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty02Name);
                //_settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty03Name);
                //_settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty04Name);
                //_settingScreenEdit.SettingDisplay05Value = GlobalPosSettings.GetConfig(_settingScreenEdit.SettingProperty05Name);
                //prepare for display of property

                //_settingScreenEdit.SettingDisplay01Value = GlobalPosSettings.GetConfig("Url");
                //_settingScreenEdit.SettingDisplay02Value = GlobalPosSettings.GetConfig("CKey");
                //_settingScreenEdit.SettingDisplay03Value = GlobalPosSettings.GetConfig("CSecret");
                //_settingScreenEdit.SettingDisplay04Value = GlobalPosSettings.GetConfig("SyncMode");
                //_settingScreenEdit.SettingDisplay05Value = GlobalPosSettings.GetConfig("AddonCount");

                _settingScreenEdit.SettingDisplay01Label = "Frequency";
                _settingScreenEdit.SettingDisplay02Label = "";
                _settingScreenEdit.SettingDisplay03Label = "";
                _settingScreenEdit.SettingDisplay04Label = "";
                _settingScreenEdit.SettingDisplay05Label = "";
            }
            SelectMasterSetting = _settingScreenEdit;



        }

    }
  




}
