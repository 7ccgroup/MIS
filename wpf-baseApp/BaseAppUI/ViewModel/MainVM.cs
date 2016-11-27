using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.Sdk.Receipts;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.ViewModel.Sections;
using BaseAppUI.ViewModel.Sections.Partial;
using BaseAppUI.ViewModel.Sections.Partials;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Printing;
using System.Threading.Tasks;
using System.Globalization;
//this is main vM
namespace BaseAppUI.ViewModel
{
   public class MainVM:NotifyProperty
   {
       public bool IsKitchenClicked = false;

       public MainVM()
       {
           
            if (BaseAppUI.Properties.Settings.Default.LoginScreen=="Show")
                SwitchToView(Model.SectionType.Login); //by SAA 4/25/16
            else
                SwitchToView(Model.SectionType.Dashboard);
            
            
                GConfig.Synchronizer.SyncStateChangeEvent += Synchronizer_SyncStateChangeEvent;
                GConfig.Synchronizer.SyncItemCategoryChangeEvent += Synchronizer_SyncItemCategoryChangeEvent;
                GConfig.Synchronizer.SyncItemMasterChangeEvent += Synchronizer_SyncItemMasterChangeEvent;
                GConfig.Synchronizer.SyncTaxRateChangeEvent += Synchronizer_SyncTaxRateChangeEvent;
            
       }

       void Synchronizer_SyncTaxRateChangeEvent(decimal rate)
       {
           GConfig.POS_Setup.vSalesTax = rate;
           GConfig.DAL.SaveChanges();
    
           Register.Order.SalexTaxRate=rate;
       }
  
     
       #region Sections

        private Head _head;
        private DashBoard _dashbord;
        private Register _register;
        private OrdersHistory _orders;
        private Report _report;
        private Items _items;
        private Customers _customer;
        private Login _Login;
        private ItemEntry _itemEntry;
        private ReportsMain _reportsmain;
        private PosSettings _settings;

        public Head Head
        {
            get { return _head ?? (_head = new Head()); }   
        }

        public Login Login
        {
            get { return _Login ?? (_Login = new Login(this)); }
        }

        public DashBoard Dashbord
        {
            get { return _dashbord ?? (_dashbord = new DashBoard(this)); }
        }


        public Register Register
        {
            get { return _register??(_register= new Register(this)); }
        }

        public OrdersHistory Orders
        {
            get { return _orders ?? (_orders = new OrdersHistory()); }
        }

        public Report Report
        {
            get { return _report ?? (_report = new Report(this)); }
        }

        public ReportsMain ReportsMain
        {
            get { return _reportsmain ?? (_reportsmain = new ReportsMain(this)); }
        }

        public Items Items
        {
            get { return _items ?? (_items = new Items(this)); }
        }

        public ItemEntry ItemEntry
        {
            get { return _itemEntry ?? (_itemEntry = new ItemEntry(this)); }
        }

        public Customers Customers
        {
            get { return _customer ?? (_customer = new Customers(this)); }
        }

        public PosSettings Settings
        {
            get { return _settings ?? (_settings = new PosSettings(this)); }
        }
        #endregion


        private ISection _content;
        public ISection Content
        {
            get { return _content; }
            set { _content = value;
            OnPropertyChanged("Content");
            }
        }
    

        private NotifyBase _notify;
        public NotifyBase Notify
        {
            get { return _notify; }
            set { _notify = value;
            OnPropertyChanged("Notify");
            }
        }


        void Synchronizer_SyncStateChangeEvent(Configuration.Sync.SyncState e)
        {
            if (e == Configuration.Sync.SyncState.Start)
                Head.IsSync = true;
            else Head.IsSync = false;

        }
     

       void Synchronizer_SyncItemCategoryChangeEvent(BaseAppServerCom.Models.Category rcat, Configuration.Sync.SyncType e)
       {
           if (e == Configuration.Sync.SyncType.Add)
           {
               try
               {
                   //TODO:
                   // verify string lenghts
                   POS_ItemCategory lcat = new POS_ItemCategory
                   {
                       CatID = rcat.id,
                       vCategoryCode = rcat.slug,
                       vCategoryDesc = rcat.description,
                       vCategoryShortDesc = rcat.name
                   };

                   GConfig.POS_Setup.ItemCategories.Add(lcat);
                   //GConfig.DAL.StateChange(GConfig.POS_Setup, System.Data.Entity.EntityState.Modified)
                   GConfig.DAL.SaveChanges();

                   if (lcat.vCategoryCode != "all-items")
                   {
                       if (Register.CategoriesInitialized)
                       {
                           var cat = GConfig.POS_Setup.ItemCategories.FirstOrDefault(n => n.CatID == lcat.CatID);

                           if (cat != null)
                           {
                               Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                               {
                                   Register.Categories.Add(new CategoryVM(cat, Register));

                               }));
                           }
                       }               
                   }
               }

               catch(Exception ex)
               {
                   //var error= ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                   var m = ex;
               }
           }
       }

       void Synchronizer_SyncItemMasterChangeEvent(BaseAppServerCom.Models.Product p, Configuration.Sync.SyncType e)
       {
           if (e == Configuration.Sync.SyncType.Add)
           {
               try {

                   Func<string, bool> compare = (key) => {

                      return p.categories.Any(m => m == key);
                   };
                

                   var cat = GConfig.POS_Setup.ItemCategories.Where(n => n.vCategoryCode != "all-items").FirstOrDefault(n => p.categories.Any(m => m == n.vCategoryShortDesc));

                   if (cat != null && !cat.POS_ItemMasters.Any(n=>n.itemID==p.id))
                   {
                       POS_ItemMaster item = new POS_ItemMaster
                       { 
                      
                           itemID=p.id,
                           dStartDate=p.created_at,
                           dEndDate=p.updated_at,
                           vItemAvailability=p.purchaseable.ToString(),
                           vItemDesc1=p.title,
                           vItemDesc2=p.description,
                           vItemStatus=p.status,
                           POS_ItemCategoryId=cat.CatID,

                           vItemMinPrice=(decimal?)p.regular_price,
                           vItemPrice=(decimal)p.price
                       };

                       cat.POS_ItemMasters.Add(item);

                       GConfig.DAL.SaveChanges();


                      

                         

                       var catvm = Register.Categories.Where(n => n.CategoryType==CategoryType.None).FirstOrDefault(n => n.CatID == cat.CatID);

                       if (catvm!=null && catvm.ItemsInitialized)
                       {
                           Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                           {
                               catvm.Items.Add(new ItemMasterVM(item));

                           }));
                       }
                      
                     

                   }

               
               }
               catch (Exception ex) {
                   var m = ex;

               }

           }
           
       }

    
        public void SwitchToView(SectionType sectiontype)
        {
          

            switch (sectiontype)
            {
                case SectionType.Dashboard:
                    SwitchToView(Dashbord);
                    break;
                case SectionType.Register:
                    SwitchToView(Register);
                    break;
                case SectionType.Orders:
                    SwitchToView(Orders);
                    break;
                case SectionType.Report:
                    SwitchToView(Report);
                    break;
                case SectionType.ReportsMain:
                    SwitchToView(ReportsMain);
                    break;
                case SectionType.Items:
                    SwitchToView(Items);
                    break;
                case SectionType.Customers:
                    SwitchToView(Customers);
                    break;
                case SectionType.Login:
                    SwitchToView(Login);
                    break;
                case SectionType.Settings:
                    if (GConfig.POSuser=="2236")
                    {

                    }
                    else
                    SwitchToView(Settings);
                    break;
                default: break;

            }


           
        }
        private void SwitchToView(ISection section)
        {

            //previous screen
            if (Content != null && Content.SectionType!=SectionType.Payment)
            {
                section.PreviousSection = Content.SectionType;
            }


            Content = section;
        }

        public static MainVM Main
        {
            get
            {
                return (MainVM)Application.Current.MainWindow.DataContext;
            }
        }


        private DelegateCommand<OrderVM> _saveOrderCommand;
        public DelegateCommand<OrderVM> SaveOrderCommand
        {
            get
            {
                return _saveOrderCommand ?? (_saveOrderCommand = new DelegateCommand<OrderVM>((e) =>
                {
                    if (e.OrderLines.Count <1)
                        return;

                    if (e.Id == 0)
                        CreateOrder(e);
                    else
                        EditOrder(e);
                }));
            }
        }


        private DelegateCommand<OrderVM> _kitchenPrintCommand;
        public DelegateCommand<OrderVM> KitchenPrintCommand
        {
            get
            {
                return _kitchenPrintCommand ?? (_kitchenPrintCommand = new DelegateCommand<OrderVM>((e) =>
                {
                    IsKitchenClicked = true;
                    GConfig.ShouldKitchenCancel = true;
                    if (e.OrderLines.Count < 1) return;
                    if (e.Id == 0)
                        CreateOrder(e);
                    else EditOrder(e);
                    PrintKitchenReceipt(e);
                }));
            }
        }

        
        private void PrintKitchenReceipt(OrderVM e)
        {
            PrintDialog printDlg = new PrintDialog();
            try
            {
                printDlg.PrintQueue = new PrintQueue(new PrintServer(),
                Properties.Settings.Default.POSKitchenPrinter);
            }
            catch
            {
                MessageBox.Show("Printer not setup");
            }
            // Create a FlowDocument dynamically.
            FlowDocument doc = CreateFlowDocument(e);
            doc.Name = "Receipt";

            // Create IDocumentPaginatorSource from FlowDocument
            IDocumentPaginatorSource idpSource = doc;

            // Call PrintDocument method to send document to printer
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Order Receipt");

            ////Printer to number of printers.
            if (Properties.Settings.Default.POSExtraPrinterCount>0)
            {
                for(int i=1;i<= Properties.Settings.Default.POSExtraPrinterCount;i++)
                {
                    if (i==1)
                    {
                        try
                        {
                            printDlg.PrintQueue = new PrintQueue(new PrintServer(),
                            Properties.Settings.Default.POSExtraPrinter01);
                        }
                        catch
                        {
                            MessageBox.Show("Printer not setup");
                        }
                        // Call PrintDocument method to send document to printer
                        printDlg.PrintDocument(idpSource.DocumentPaginator, "Order Receipt");

                    }
                    if (i == 2)
                    {
                        try
                        {
                            printDlg.PrintQueue = new PrintQueue(new PrintServer(),
                            Properties.Settings.Default.POSExtraPrinter02);
                        }
                        catch
                        {
                            MessageBox.Show("Printer not setup");
                        }
                        // Call PrintDocument method to send document to printer
                        printDlg.PrintDocument(idpSource.DocumentPaginator, "Order Receipt");

                    }
                }
            }


        }

        private FlowDocument CreateFlowDocument(OrderVM PrintOrder)
        {
            // Receipt Header
            // Create a FlowDocument
            FlowDocument doc = new FlowDocument();

            // Create a Section
            Section sec = new Section();

            // Formatting
            Bold bld = new Bold();
            Italic itl = new Italic();
            Underline uline = new Underline();
            doc.FontFamily = new FontFamily("FontA12");



            Paragraph header = new Paragraph(new Run("Order #" + PrintOrder.OrderNumber + " -- " + PrintOrder.OrderType));
            header.FontSize = 24;
            header.TextAlignment = TextAlignment.Left;
            header.Inlines.Add(bld);
            doc.Blocks.Add(header);

            Int16 ReceiptOrderLineCount, i;
            string ReceiptOrderLines = DateTime.Now.ToString("ddd MMM dd, yyyy  hh:mm tt") + "\n\n";
            ReceiptOrderLineCount = (Int16)PrintOrder.OrderLines.Count();

            for (i = 0; i < ReceiptOrderLineCount; i++)
            {
                ReceiptOrderLines += PrintOrder.OrderLines[i].Quantity;
                ReceiptOrderLines += "    ";
                ReceiptOrderLines += PrintOrder.OrderLines[i].ItemName;
                ReceiptOrderLines += "\n";
                ReceiptOrderLines += "     - " + PrintOrder.OrderLines[i].Comments; // Added comments / modifers to kitchen receipt - Amjad 5/20
                ReceiptOrderLines += "\n\n";
            }
            Paragraph orderLines = new Paragraph(new Run(ReceiptOrderLines + "\n\n\n\n Order #" + PrintOrder.OrderNumber + " - " + PrintOrder.OrderType + "\n\n"));
            orderLines.FontSize = 16;
            orderLines.TextAlignment = TextAlignment.Left;
            orderLines.Inlines.Add(bld);
            doc.Blocks.Add(orderLines);
            
            return doc;
        }

        private void CreateOrder(OrderVM e) {

            try
            {
                var _orderHeader = e._orderHeader;

                //lines
                var lines = e.OrderLines.Select(n => n._line).ToList();
                _orderHeader.POS_OrderDetails = lines;

                _orderHeader.vOrderStatus =e.OrderStatus;
                _orderHeader.dOrderAmount = e.AllTotal;
                _orderHeader.dOrderTax = e.TaxSubtotal;

                //order number
                _orderHeader.vOrderNumber = GConfig.GenerateLocalOrderNumber();

                //time
                var processdate = DateTime.Now;

                _orderHeader.dModifiedDate = processdate;
                _orderHeader.dCreatedDate = processdate;

                foreach (var n in _orderHeader.POS_OrderDetails)
                {
                    n.dModifiedDate = processdate;
                    n.dCreatedDate = processdate;
                }

                //customer
                if (_orderHeader.CustomerId == 0)
                {
                    var custo = GConfig.POS_Setup.Customers.FirstOrDefault((n) => n.vCustContactNme == "Guest Customer");
                    _orderHeader.CustomerId = custo.Id;
                }
                _orderHeader.POS_SetupId = GConfig.POS_Setup.Id;

                GConfig.POS_Setup.OrderHeaders.Add(_orderHeader);
                GConfig.DAL.SaveChanges();


                if(e.OrderStatus!=Model.OrderStatuses.Completed)
                if (IsKitchenClicked == false)
                {
                    SwitchToView(Orders);
                }

                IsKitchenClicked = false;

                    //SwitchToView(Orders);

                if (Orders.SelectedOrderType == Orders.AllTypes[0])
                {

                    Orders.Orders.Insert(0, e);
                    Orders.SelectOrderCommand.Execute(e);
                }

                else
                {
                    Orders.SelectedOrder = e;

                    Orders.SelectOrderTypesCommand.Execute(Orders.AllTypes[0]);

                }

               // IsKitchenClicked = true; ///5/8/2016 order refersh

                if (IsKitchenClicked == false)
                {
                    Register.RemoveSelection();
                    Register.Order = null;
                }

                IsKitchenClicked = false;

            }
            catch (Exception ex)
            {
                new Notifies.TransactionFailedNotify("100", ex.Message.ToString()).Show();
            }
        }

        private void EditOrder(OrderVM e)
        {
            try
            {
                var _orderHeader = e._orderHeader;

                //lines
                var lines = e.OrderLines.Select(n => n._line).ToList();
                _orderHeader.POS_OrderDetails = lines;

                _orderHeader.dOrderAmount = e.AllTotal;
                _orderHeader.dOrderTax = e.TaxSubtotal;
                _orderHeader.vOrderStatus = e.OrderStatus;
                _orderHeader.vOrderPaymentType = e.PaidVia; // changed from PaymentType 5/27

                //time
                var processdate = DateTime.Now;

                _orderHeader.dModifiedDate = processdate;

                foreach (var n in _orderHeader.POS_OrderDetails)
                {
                    if (n.Id == 0)
                        n.dCreatedDate = processdate;

                    n.dModifiedDate = processdate;
                }

                GConfig.DAL.StateChange(_orderHeader,System.Data.Entity.EntityState.Modified);
                GConfig.DAL.SaveChanges();

                e.ReservedLines.Clear();

                Register.Order = null;

                if (e.OrderStatus != Model.OrderStatuses.Completed)
                SwitchToView(Orders);

                Register.RemoveSelection();
            }

            catch (Exception ex)
            {
                new Notifies.TransactionFailedNotify("101", ex.Message.ToString()).Show();
            }
        }

        private DelegateCommand<OrderVM> _payOrderCommand;
        public DelegateCommand<OrderVM> PayOrderCommand
        {
            get
            {
                return _payOrderCommand ?? (_payOrderCommand = new DelegateCommand<OrderVM>((e) =>
                {
                    var section= new Payment(e);
                    section.PreviousSection = SectionType.Orders;
                    GConfig.GCorder = e;  ///check this is a problem.......
                   GConfig.total_amount = section.CardPay.AllTotal; ///check this is a problem.......
                    if (e.OrderLines.Count < 1) return;
                    else SwitchToView(section);

                }));
            }
        }

        private DelegateCommand<OrderVM> _printOrderCommand;

        public DelegateCommand<OrderVM> PrintOrderCommand
        {
            get
            {
                return _printOrderCommand ?? (_printOrderCommand = new DelegateCommand<OrderVM>((e) =>
                {
                    CustomerReceiptPrinter.Print(e);
                }));
            }
        }

        // Cancel for cash transactions only
        private DelegateCommand<OrderVM> _cancelOrderCommand;
        public DelegateCommand<OrderVM> CancelOrderCommand
        {
            get
            {
                return _cancelOrderCommand ?? (_cancelOrderCommand = new DelegateCommand<OrderVM>((e) =>
                {
                    e.OrderStatus = OrderStatuses.Cancelled;
                    SaveOrderCommand.Execute(e);
                    // to-do add toast confirmation (Issue # 21 on Github)
                }));
            }
        }

        
        private DelegateCommand<OrderVM> _editOrderCommand;
        public DelegateCommand<OrderVM> EditOrderCommand
        {
            get
            {
                return _editOrderCommand ?? (_editOrderCommand = new DelegateCommand<OrderVM>((e) =>
                {
                    Register.RemoveSelection();
                    e.ReservedLines = e.OrderLines.Select(n => n._line).ToList();
                    Register.Order = e;


                    foreach (var itemid in e.OrderLines.Select(n => n._item.Id))
                    {
                        var item = Register.Categories.SelectMany(k => k.Items.Where(m => m._item.Id == itemid)).FirstOrDefault();
                        if (item != null)
                        {
                            Register.SetItemVisual(item);
                        }
                    }

                    SwitchToView(Register);

                }));
            }

        }

        // Refund / Void Completed Orders - Amjad 5/13

        private DelegateCommand<OrderVM> _refundCommand;
        public DelegateCommand<OrderVM> RefundCommand
        {
            get

            {
                return _refundCommand ?? (_refundCommand = new DelegateCommand<OrderVM>((e) => {

                    if (e.PaymentType == OrderPaymentTypes.Cash) // Cash Refund - Amjad 5/18
                    {
                        e.OrderStatus = OrderStatuses.Refunded;
                        SaveOrderCommand.Execute(e);
                    }
                    else
                        RefundOrder(e, e.AllTotal); // Credit Card Refund - First do void if failed then do refund 

                }));
            }

        }



        // Refund / Void Completed Orders - Amjad 5/13

        private async void RefundOrder(OrderVM order, decimal refundAmt)
        {

            await Task.Factory.StartNew(() => {

                // Prepare parameters for void/refund
                string amount = refundAmt.ToString(CultureInfo.CreateSpecificCulture("en-US"));
                string txn_id = order._orderHeader.TxnId;

                ccSale.ccReturn refund = new ccSale.ccReturn();
                refund.SetReturnDetails(txn_id, amount);

                ccSale.ccVoid voider = new ccSale.ccVoid();
                voider.SetVoidDetails(txn_id);

                voider.Process();
                var voidResult = voider.Resp_Msg == "APPROVAL" ? true : false;

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (voidResult) // Try a void first ... (unsettled batches)
                    {
                        new Notifies.TransactionSuccessNotify("Transaction VOIDED successfully.").Show(() => {
                            order.OrderStatus = OrderStatuses.Refunded;
                            SaveOrderCommand.Execute(order);
                        });
                    }
                    else  // If failed try a refund ... (settled batches)
                    {
                        refund.Process();
                        var refundResult = refund.Resp_Msg == "APPROVAL" ? true : false;
                        if(refundResult)
                        {
                            new Notifies.TransactionSuccessNotify("REFUND processed successfully.").Show(() => {
                                order.OrderStatus = OrderStatuses.Refunded;
                                SaveOrderCommand.Execute(order);
                            });
                        }
                        else new Notifies.TransactionFailedNotify("This transaction cannot be refunded.", null).Show();
                    }  
                   
                }));

            });

        }


        // Add Tips to existing order - Amjad

        private DelegateCommand<OrderVM> _addTipCommand;

        public DelegateCommand<OrderVM> AddTipCommand
        {
            get
            {
                return _addTipCommand ?? (_addTipCommand = new DelegateCommand<OrderVM>((e) => {

                    //new Notifies.AddTipModifier(AddTipToOrder).Show();

                    new Notifies.DiscountModifier(e.Subtotal, e.TipAmount ?? 0m, (tip_amt) =>
                    {

                        AddTipToOrder(e, tip_amt);

                    }) {Title="Add Tip",IsAsingnedToTip=true }.Show();

                }));
            }

        }

        // Add Tips to existing order - Amjad
        private async void AddTipToOrder(OrderVM order, decimal tip_amt)
        {


           await Task.Factory.StartNew(() => {

               ccSale.ccUpdateTip update = new ccSale.ccUpdateTip();
               string amount = tip_amt.ToString(CultureInfo.CreateSpecificCulture("en-US"));
               string txn_id = order._orderHeader.TxnId;

               update.SetTipDetails(txn_id, amount);

               update.Process();


               var result = update.Resp_Msg == "SUCCESS" ? true : false;


               Application.Current.Dispatcher.BeginInvoke(new Action(() =>
               {
                   if (result){

                       new Notifies.TransactionSuccessNotify("Tip ADDED successfully.").Show(() => {
                           order.TipAmount = tip_amt;
                           SaveOrderCommand.Execute(order);
                       });
                   }
                   else new Notifies.TransactionFailedNotify("FAILED to add tip.",null).Show();
               }));
            
            
            });


        }



        private DelegateCommand _returnCommand;
        public DelegateCommand ReturnCommand
        {
            get
            {
                return _returnCommand ?? (_returnCommand = new DelegateCommand(() =>
                {
                    SwitchToView(Model.SectionType.Dashboard);
                }));
            }
        }
    }
}
