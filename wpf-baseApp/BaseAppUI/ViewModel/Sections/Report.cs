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

namespace BaseAppUI.ViewModel.Sections
{
    public class Report : NotifyProperty, ISection
    {
        

        MainVM _main;
        

        public Report(MainVM main)
        {
            
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

        private DelegateCommand _reportCommand;

        public DelegateCommand ReportCommand
        {
            get
            {
                return _reportCommand ?? (_reportCommand = new DelegateCommand(() =>
                {

                   MessageBox.Show("Hello this is good start, have to complete this IA");
                 //   _main.SwitchToView(SectionType.Dashboard);

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

                    //    MessageBox.Show("Hello this is good start, have to complete this IA");
                    _main.SwitchToView(SectionType.Dashboard);

                }));
            }

        }

        private IList<ReportTypeVM> _allTypes;
        public IList<ReportTypeVM> AllTypes
        {
            get
            {
                return _allTypes ?? (_allTypes = new List<ReportTypeVM> {

                 new ReportTypeVM("All Reports",ReportTypes.None,this),
                  new ReportTypeVM("Sales", ReportTypes.Sales, this),
                  new ReportTypeVM("Customer", ReportTypes.Customers, this),
                  new ReportTypeVM("Items", ReportTypes.Items, this),
                 new ReportTypeVM("Pricing", ReportTypes.Pricing, this),
                 new ReportTypeVM("Inventory", ReportTypes.Inventory, this)
               });
            }

        }

        

        private ReportTypeVM _selectedReportType;

        public ReportTypeVM SelectedReportType
        {
            get { return _selectedReportType; }
            set
            {
                if (value != _selectedReportType)
                {
                    _selectedReportType = value;
                    OnPropertyChanged("PosReports"); //This is property name that will trigger refresh......saa 4/2/2016
                    

                }

            }
        }

        private DelegateCommand<ReportTypeVM> _selectReportTypesCommand;

        public DelegateCommand<ReportTypeVM> SelectReportTypesCommand
        {
            get
            {
                return _selectReportTypesCommand ?? (_selectReportTypesCommand = new DelegateCommand<ReportTypeVM>((e) => {


                    if (e == null) return;
                    if (SelectedReportType != e)
                    {

                        if (SelectedReportType != null)
                            SelectedReportType.IsSelected = false;


                        SelectedReportType = e;
                        SelectedReportType.IsSelected = true;

                       

                    }


                }));
            }
        }
        
        
        ////////////////Report Command......
       
        /// <summary>
        /// Add by SAA 4/2 to bind report on reportview.
        /// </summary>
        private ReportsBuilder _reportBuilder = new ReportsBuilder();
        private DataTable ReportDataTable;
        public DataTable PosReports
        {
           
            get
            {
                if (_selectedReportType != null)
                    ReportDataTable = _reportBuilder.POSreports(_selectedReportType.Name);
                else
                    ReportDataTable = _reportBuilder.POSreports("Items");
                return ReportDataTable;
            }
            set { ReportDataTable = value;
                OnPropertyChanged("PosReports");
            }
        }
        public string ReportSelectedDisplay {

            get {
                if (SelectedReportType != null)
                    return SelectedReportType.Name;
                else
                    return "";
            }
        }
        public void ManipulationBoundaryFeedbackHandler
  (object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        public SectionType PreviousSection { get; set; }
        public SectionType SectionType
        {
            get { return Model.SectionType.Report; }
        }
    }
}
