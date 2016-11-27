using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.Sdk.Receipts;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum ThanksNotifyCommands
    {
        None,
        Text,
        Email,
        Print

    }
    public class ThanksNotify : NotifyBase
    {
        OrderVM _order;
        private string _text;
        private string _email;

        public ThanksNotify(OrderVM order)
        {
            _order = order;
            EnableMask = true;
        }


        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("TextDisplay");
            }
        }

        public string TextDisplay
        {
            get
            {
                return FormatPhoneNumberDisplayText(_text);
            }
            set
            {
                Text = value.Replace("-", "");
            }
        }


        private string FormatPhoneNumberDisplayText(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            StringBuilder sb = new StringBuilder();
            var vals = value.ToCharArray();
            for (int i = 0; i < vals.Length; i++)
            {
                if (i == 3 || i == 6 || i == 10)
                    sb.Append("-");
                sb.Append(vals[i]);
            }


            return sb.ToString();
        }


        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private DelegateCommand<ThanksNotifyCommands> _thanksCommand;
        public DelegateCommand<ThanksNotifyCommands> ThanksCommand
        {
            get
            {
                return _thanksCommand ?? (_thanksCommand = new DelegateCommand<ThanksNotifyCommands>((e) =>
                {

                    switch (e)
                    {
                        case ThanksNotifyCommands.None:
                            /*if (_order.OrderStatus == OrderStatuses.Pending)
                                PendingSaveOrder();
                            else*/
                                //SaveOrder();
                            this.CloseCommand.Execute(null);
                            break;
                        case ThanksNotifyCommands.Text:
                            break;
                        case ThanksNotifyCommands.Email:
                            break;
                        case ThanksNotifyCommands.Print:
                            CustomerReceiptPrinter.Print(_order);
                            /*if (_order.PaidVia == OrderPaymentTypes.Card)
                            {
                                ReceiptPrinter.Print(_order);
                            }
                            else if (_order.PaidVia == OrderPaymentTypes.Cash)
                                CustomerReceiptPrinter.Print(_order);
                            */
                            /*if (_order.OrderStatus == OrderStatuses.Pending)
                                PendingSaveOrder();
                            else*/


                            if (_order.PaymentType != OrderPaymentTypes.Split)
                                SaveOrder();
                            this.CloseCommand.Execute(null);
                            break;
                    }

                }));
            }
        }

        private void PendingSaveOrder()
        {
            if (_order.OrderLines.Count > 0)
            {

                try
                {
                    var _orderHeader = _order._orderHeader;

                    //lines
                   
                    _orderHeader.vOrderStatus = OrderStatuses.Completed;
                    _orderHeader.dOrderAmount = _order.AllTotal;
                    _orderHeader.dOrderTax = _order.TaxSubtotal;

                    //time
                    var processdate = DateTime.Now;
                    _orderHeader.dModifiedDate = processdate;
                    _orderHeader.dCreatedDate = processdate;

                  
                  
                    //   GConfig.POS_Setup.OrderHeaders.Add(_orderHeader);
                   GConfig.DAL.SaveChanges();

                    //   MainVM.Main.Orders.Orders.Insert(0, _order);

                    
                    //MainVM.Main.Register.RemoveSelection();
                    //MainVM.Main.Register.Order = null;
          

                }
                catch (Exception ex)
                {
                    new Notifies.TransactionFailedNotify("100", ex.Message.ToString()).Show();
                }

            }
        }

        private void SaveOrder()
        {
                MainVM.Main.SaveOrderCommand.Execute(_order);
        }


        /*
        private void PrintCustomerReceipt()
        {
            PrintDialog printDlg = new PrintDialog();
            try
            {
                printDlg.PrintQueue = new PrintQueue(new PrintServer(),
                    BaseAppUI.Properties.Settings.Default.POSMainPrinter);
            }
            catch
            {
                MessageBox.Show("Printer Not Setup");
            }
            // Create a FlowDocument dynamically.
            FlowDocument doc = CreateCustomerFlowDocument(_order);
            doc.Name = "Receipt";

            // Create IDocumentPaginatorSource from FlowDocument
            IDocumentPaginatorSource idpSource = doc;

            // Call PrintDocument method to send document to printer
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Order Receipt");
        }

        private FlowDocument CreateCustomerFlowDocument(OrderVM PrintOrder)
        {
            //Receipt Header
            // Create a FlowDocument
            FlowDocument doc = new FlowDocument();

            // Create a Section
            Section sec = new Section();

            // Formatting
            Bold bld = new Bold();
            Italic itl = new Italic();
            Underline uline = new Underline();
            doc.FontFamily = new FontFamily("Arial Rounded MT Std");



            Paragraph header = new Paragraph(new Run("\n" + GConfig.POS_Setup.vCorpName));
            header.FontSize = 24;
            header.TextAlignment = TextAlignment.Left;
            header.Inlines.Add(bld);
            doc.Blocks.Add(header);


            Int16 ReceiptOrderLineCount, i;
            string ReceiptOrderLines = BaseAppUI.Properties.Settings.Default.RestaurantAddress1 +
                "\n" + BaseAppUI.Properties.Settings.Default.RestaurantAddress2 +
                "\n" + BaseAppUI.Properties.Settings.Default.RestaurantTel + "\n\n" +
                "Order #" + PrintOrder.OrderNumber + " -- " + PrintOrder.OrderType + " \n\n" +
                DateTime.Now.ToString("ddd MMM dd, yyyy  hh:mm tt") + "\n\n";
            //string ReceiptOrderLines = PrintOrder._orderHeader.dModifiedDate.ToString("MMM dd, yyyy hh:mm") + "\n\n";

            ReceiptOrderLineCount = (Int16)PrintOrder.OrderLines.Count();

            for (i = 0; i < ReceiptOrderLineCount; i++)
            {
                ReceiptOrderLines += PrintOrder.OrderLines[i].Quantity;
                ReceiptOrderLines += "    ";
                ReceiptOrderLines += PrintOrder.OrderLines[i].ItemName;
                ReceiptOrderLines += "    ";
                ReceiptOrderLines += PrintOrder.OrderLines[i].Total;
                ReceiptOrderLines += "\n\n";

                //ReceiptOrderLines += PrintOrder.OrderLines[i].Price;
                //ReceiptOrderLines += "\n";
                //ReceiptOrderLines += "\n";
            }

            string ReceiptSubTotal, ReceiptTaxTotal, ReceiptTotalAmount;
            ReceiptSubTotal = "Subtotal:\t" + PrintOrder.Subtotal.ToString("#,#.00") + "\n";
            ReceiptTaxTotal = "Tax:\t" + PrintOrder.TaxSubtotal.ToString("#.00") + "\n";
            ReceiptTotalAmount = "Total:\t" + PrintOrder.AllTotal.ToString("#,#.00");
            if (_order.PaymentType == OrderPaymentTypes.Card)
            {
                ReceiptTotalAmount = "Total:\t" + PrintOrder.AllTotal.ToString("#,#.00") + "\n" +
                    "\n Appr Code: " + GConfig.response2.Resp_ApprovalCode + "\n" +
                    "Credit Card \t" + GConfig.response2.Resp_CardNum + "\n " +
                    GConfig.response2.Resp_Msg.ToUpper() + " \n";
            }
            string ReceiptSignatureLine = "\n ", ReceiptSignatureWriteup = " ";
            //ReceiptSignatureLine = "\n\n\n\n___________________________________\n";
            //ReceiptSignatureWriteup = "I agree to pay the above\n total amount according to \n my card issuer agreement.\n";

            Paragraph orderLines = new Paragraph(new Run(ReceiptOrderLines
                + ReceiptSubTotal + ReceiptTaxTotal + ReceiptTotalAmount
                + ReceiptSignatureLine + ReceiptSignatureWriteup + "\n\n\n THANK YOU \n\n Customer Copy"));

            orderLines.FontSize = 13;
            orderLines.TextAlignment = TextAlignment.Left;
            orderLines.Inlines.Add(bld);
            doc.Blocks.Add(orderLines);

            return doc;
        }
        */
    }



}