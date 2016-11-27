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
using System.Windows.Documents;

using System.Windows.Media;
using System.Windows.Controls;
using System.Printing;
using ConvergeAPI;
using System.Collections.ObjectModel;

namespace BaseAppUI.ViewModel.Sections
{
    public class ReportsMain : NotifyProperty, ISection
    {


        MainVM _main;


        public ReportsMain(MainVM main)
        {

            ReportDashboardVisible = Visibility.Visible;
            ReportEndOfDayVisible = Visibility.Collapsed;
            ReportSettleAllVisible = Visibility.Collapsed;
            ReportTrendsVisible = Visibility.Collapsed;

            AllTypes.First().IsSelected = true; //to select first report in list.
            SelectedReportType = AllTypes.First(); //to initialize first report in list.
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
                    //PrintZreport
                    PrintReport(CreateZreport(), "Zreport");
                    //MessageBox.Show("Hello this is good start, have to complete this IA");
                    

                }));
            }

        }

        private DelegateCommand _settleCommand;
        public DelegateCommand SettleCommand
        {
            get
            {
                return _settleCommand ?? (_settleCommand = new DelegateCommand(() =>
                {
                    //SettlewithDelay();

                    Settle NowSettling = new Settle();
                    NowSettling.Process();
                    //await Task.Delay(3000);

                    if (NowSettling.Resp_Msg == "Scheduled for Settlement")
                        MessageBox.Show("Success!\nAll open transactions have been settled");
                    else
                        MessageBox.Show(NowSettling.Resp_Msg);
              
                    //SaleKeyed.Set(CardNumber.Value, ExpiryDate.Value, CVV.Value, AllTotal.ToString("#.00"));
                    //_order._orderHeader.vOrderPaymentType = OrderPaymentTypes.Card;
                    //new AuthorizingNotify(Authorize, SaleKeyed).Show();

            }));
            }
        }

        //public async void SettlewithDelay()
        //{
        //    Settle NowSettling = new Settle();
        //    NowSettling.Process();
        //    await Task.Delay(3000);

        //    if (NowSettling.Resp_Msg == "Scheduled for Settlement")
        //        MessageBox.Show("AllTypes open transactions have been scheduled for settlement");
        //    else
        //        MessageBox.Show(NowSettling.Resp_Msg);
        //}

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

        private IList<ReportsMainTypeVM> _allTypes;
        public IList<ReportsMainTypeVM> AllTypes
        {
            get
            {
                return _allTypes ?? (_allTypes = new List<ReportsMainTypeVM> {

                 new ReportsMainTypeVM("DASHBOARD", ReportTypes.Dashboard,this),
                // new ReportsMainTypeVM("TRENDS", ReportTypes.Trends, this),
                 new ReportsMainTypeVM("END OF DAY", ReportTypes.TransactionList, this),
                 new ReportsMainTypeVM("SETTLE ALL", ReportTypes.SettleAll, this),
                 
               });


            }

        }



        private ReportsMainTypeVM _selectedReportType;
        public ReportsMainTypeVM SelectedReportType
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
        public string _ZreportDateSelection;
        public String ZreportDateSelection
        {
            get { return _ZreportDateSelection.ToString(); }
            set { _ZreportDateSelection = value;
                MessageBox.Show(_ZreportDateSelection.ToString());
            }
        }
        private FlowDocument _POSReportDocument;
        public FlowDocument POSReportDocument
        {
            get { return _POSReportDocument; }
            set
            {
                _POSReportDocument = value;
                OnPropertyChanged("POSreportDocument");
            }
        }
        private Visibility _reportTrendsVisible;
        public Visibility ReportTrendsVisible
        {
            get { return _reportTrendsVisible; }
            set
            {
                _reportTrendsVisible = value;
                OnPropertyChanged("ReportTrendsVisible");
            }
        }
        private Visibility _reportDashboardVisible;
        public Visibility ReportDashboardVisible
        {
            get { return _reportDashboardVisible; }
            set
            {
                _reportDashboardVisible = value;
                OnPropertyChanged("ReportDashboardVisible");
            }
        }
        private Visibility _reportEndOfDayVisible;
        public Visibility ReportEndOfDayVisible
        {
            get { return _reportEndOfDayVisible; }
            set
            {
                _reportEndOfDayVisible = value;
                OnPropertyChanged("ReportEndOfDayVisible");
            }
        }

        private Visibility _reportSettleAllVisible;
        public Visibility ReportSettleAllVisible
        {
            get { return _reportSettleAllVisible; }
            set
            {
                _reportSettleAllVisible = value;
                OnPropertyChanged("ReportSettleAllVisible");
            }
        }

        private DelegateCommand<ReportsMainTypeVM> _selectReportTypesCommand;
        public DelegateCommand<ReportsMainTypeVM> SelectReportTypesCommand
        {
            get
            {
                return _selectReportTypesCommand ?? (_selectReportTypesCommand = new DelegateCommand<ReportsMainTypeVM>((e) => {

                   
                    if (e == null) return;
                    if (SelectedReportType != e)
                    {
                        if (SelectedReportType != null)
                            SelectedReportType.IsSelected = false;

                        SelectedReportType = e;
                        SelectedReportType.IsSelected = true;

                        if (SelectedReportType.Name == "END OF DAY")
                        {
                            POSReportDocument = CreateZreport();
                            ReportEndOfDayVisible = Visibility.Visible;
                            ReportDashboardVisible = Visibility.Collapsed;
                            ReportSettleAllVisible = Visibility.Collapsed;
                            ReportTrendsVisible = Visibility.Collapsed;
                        }

                        if (SelectedReportType.Name == "SETTLE ALL")
                        {
                            ReportSettleAllVisible = Visibility.Visible;
                            ReportEndOfDayVisible = Visibility.Collapsed;
                            ReportDashboardVisible = Visibility.Collapsed;
                            ReportTrendsVisible = Visibility.Collapsed;
                        }

                        if (SelectedReportType.Name == "DASHBOARD")
                        {
                            ReportDashboardVisible = Visibility.Visible;
                            ReportEndOfDayVisible = Visibility.Collapsed;
                            ReportSettleAllVisible = Visibility.Collapsed;
                            ReportTrendsVisible = Visibility.Collapsed;
                        }
                        if (SelectedReportType.Name == "TRENDS")
                        {
                            ReportDashboardVisible = Visibility.Collapsed;
                            ReportEndOfDayVisible = Visibility.Collapsed;
                            ReportSettleAllVisible = Visibility.Collapsed;
                            ReportTrendsVisible = Visibility.Visible;
                        }

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
                {
                    _reportBuilder.CreportType = _selectedReportType.Name;
                   // MessageBox.Show(_reportBuilder.CreportType.ToString());
                    ReportDataTable = _reportBuilder.POSreports(_selectedReportType.Name);
                  
                }
                else
                    ReportDataTable = _reportBuilder.POSreports("Items");
                return ReportDataTable;
            }
            set
            {
                ReportDataTable = value;
                OnPropertyChanged("PosReports");
            }
        }
        
        public ObservableCollection<SalesForDataVisual> DailySalesVisual
        {
            get
            {
                return _reportBuilder.SalesForDataVisual();
            }
            set { }
        }

        public ObservableCollection<SalesForDataVisual> MonthlySalesVisual
        {
            get
            {
                return _reportBuilder.MonthlySalesForDataVisual();
            }
            set { }
        }
        public DataTable TopSellingItems
        {
            get
            {
                return _reportBuilder.POSreports("TopSellingItems");
            }
        }
        public DataTable TopSellingDays
        {
            get
            {
                return _reportBuilder.POSreports("TopSellingDays");
            }
        }
        public string SalesToday
        {

            get
            {
                return _reportBuilder.POSmainreports("SalesToday");

            }
        }
        public string SalesTipsToday
        {

            get
            {
                return _reportBuilder.POSmainreports("SalesTipsToday");

            }
        }
        public string SalesGratuityToday //Gratuity is implemented by dOrderServiceFee column
        {

            get
            {
                return _reportBuilder.POSmainreports("SalesGratuityToday");

            }
        }
        public string RefundSalesToday //Added Refunds ZC 7/19/16
        {

            get
            {
                return _reportBuilder.POSmainreports("RefundSalesToday");

            }
        }
        public string CompletedSalesToday
        {

            get
            {
                return _reportBuilder.POSmainreports("CompletedSalesToday");

            }
        }
        public string CashSalesToday
        {

            get
            {
                return _reportBuilder.POSmainreports("CashSalesToday");

            }
        }
        public string CreditSalesToday
        {

            get
            {
                return _reportBuilder.POSmainreports("CreditSalesToday");

            }
        }
        public string PendingOrdersToday
        {

            get
            {
                return _reportBuilder.POSmainreports("PendingOrdersToday");

            }
        }
        public string CompletedOrdersToday
        {
            get
            {
                return _reportBuilder.POSmainreports("OrdersToday");
            }
        }
        public string OrdersToday
        {

            get
            {
                int totalOrdersToday;
                if (BaseAppUI.Properties.Settings.Default.DashboardIncludePendingOrders == "Yes")
                {
                    totalOrdersToday = int.Parse(CompletedOrdersToday) + int.Parse(PendingOrdersToday);
                    return totalOrdersToday.ToString();
                }
                else
                    return _reportBuilder.POSmainreports("OrdersToday");

            }
        }
        public string ItemsCount
        {

            get
            {
                return _reportBuilder.POSmainreports("ItemsCount");

            }
        }
        public string CustomersCount
        {

            get
            {
                return _reportBuilder.POSmainreports("CustomersCount");

            }
        }
        public string MTDsales
        {

            get
            {
                return _reportBuilder.POSmainreports("MTDsales");

            }
        }
        public string YTDSales
        {

            get
            {
                return _reportBuilder.POSmainreports("YTDSales");

            }
        }

        private void PrintReport(FlowDocument reportDoc,string reportName)
        {
            PrintDialog printDlg = new PrintDialog();
            try {
                printDlg.PrintQueue = new PrintQueue(new PrintServer(),
                    BaseAppUI.Properties.Settings.Default.POSMainPrinter);
            }
            catch
            {
                MessageBox.Show("Printer not setup");
            }
            // Create a FlowDocument dynamically.
            FlowDocument doc = reportDoc;
            doc.Name = reportName;

            // Create IDocumentPaginatorSource from FlowDocument
            IDocumentPaginatorSource idpSource = doc;

            // Call PrintDocument method to send document to printer
            printDlg.PrintDocument(idpSource.DocumentPaginator, reportName);
        }

        private FlowDocument CreateZreport()
        {
            //Receipt Header
            // Create a FlowDocument
            FlowDocument doc = new FlowDocument();
            string ReportTitle = "Z-Report";
            // Create a Section
            Section sec = new Section();

            // Formatting
            Bold bld = new Bold();
            Italic itl = new Italic();
            Underline uline = new Underline();
            doc.FontFamily = new FontFamily("Arial Rounded MT Std");



            Paragraph header = new Paragraph(new Run("\n\n\n\n" + GConfig.POS_Setup.vCorpName));
            header.FontSize = 24;
            header.TextAlignment = TextAlignment.Left;
            header.Inlines.Add(bld);
            doc.Blocks.Add(header);

            Paragraph paraReportTitle = new Paragraph(new Run(ReportTitle));
            paraReportTitle.FontSize = 18;
            paraReportTitle.TextAlignment = TextAlignment.Left;
           
            //Int16 ReceiptOrderLineCount, i;
            string ReceiptOrderLines = "\n" + BaseAppUI.Properties.Settings.Default.BusinessAddress1 +
                "\n" + BaseAppUI.Properties.Settings.Default.BusinessAddress2 +
                "\n" + BaseAppUI.Properties.Settings.Default.BusinessTel + "\n\n" +
               
                DateTime.Now.ToString("ddd MMM dd, yyyy  hh:mm tt") + "\n\n" 
                ;

            string SalesCashTotal =   "Cash Sales:" + "\t " + CashSalesToday;
            string SalesCreditTotal = "Credit Sales:" + "\t " + CreditSalesToday;
            string RefundSalesAmount = "Total Refunds:" + "\t-" + RefundSalesToday;
            string SalesTotalAmount= "Total Sales:" + "\t " + CompletedSalesToday;
            string SalesTipsAmount = "Total Tips:" + "\t " + SalesTipsToday; 
            string SalesGratuityAmount = "Total Gratuity:" + "\t " + SalesGratuityToday;
            string CompletedOrderCountToday = "Completed Orders:" + "\t " + CompletedOrdersToday.ToString(); //property is used to get orders.
            string PendingOrderCountToday = "Pending Orders:" + "\t\t " + PendingOrdersToday.ToString(); //property is used to get orders.
            
           
            Paragraph paraCorpDetails = new Paragraph(new Run(ReceiptOrderLines));
            Paragraph parasalesTotals = new Paragraph(new Run(SalesCashTotal));
            Paragraph paraCCsalesTotals = new Paragraph(new Run(SalesCreditTotal));
            Paragraph pararefundsTotal = new Paragraph(new Run(RefundSalesAmount));
            Paragraph parasalesGrossTotal = new Paragraph(new Run(SalesTotalAmount));
            Paragraph parasalesTipsTotal = new Paragraph(new Run(SalesTipsAmount));
            Paragraph parasalesGratuityTotal = new Paragraph(new Run(SalesGratuityAmount));


            Paragraph paraCompletedOrderCount = new Paragraph(new Run("\n"+ CompletedOrderCountToday));
            Paragraph paraPendingOrderCount = new Paragraph(new Run( PendingOrderCountToday));
            doc.Blocks.Add(paraCorpDetails);
            doc.Blocks.Add(paraReportTitle);
            doc.Blocks.Add(parasalesTotals);
            doc.Blocks.Add(paraCCsalesTotals);
            doc.Blocks.Add(pararefundsTotal);
            doc.Blocks.Add(parasalesGrossTotal);
            doc.Blocks.Add(parasalesTipsTotal);
            doc.Blocks.Add(parasalesGratuityTotal);


            doc.Blocks.Add(paraCompletedOrderCount);
            doc.Blocks.Add(paraPendingOrderCount);

            return doc;
        }
        public void ManipulationBoundaryFeedbackHandle (
            object sender, ManipulationBoundaryFeedbackEventArgs e)
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
