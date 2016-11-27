using BaseAppUI.Configuration;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BaseAppUI.Sdk.Receipts
{
    public class ReceiptPrinter
    {

        private static FrameworkElement GetVisual(ReceiptModel model)
        {

            var myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("pack://application:,,,/View/Receipts/ReceiptView.xaml",
                        UriKind.RelativeOrAbsolute);

            ContentControl control = new ContentControl() { Content = model, ContentTemplate = (DataTemplate)myResourceDictionary["ReceiptView"] };

            var _visual = new StackPanel();
            _visual.Children.Add(control);
          


            return _visual;
        }

        
        private static void _printmodel(ReceiptModel model)
        {

            var _visual = GetVisual(model);


            //var queue = new LocalPrintServer().DefaultPrintQueue;

            //Printer Assignment Fix by Amjad - 6/14
            var queue = new LocalPrintServer().GetPrintQueue(BaseAppUI.Properties.Settings.Default.POSMainPrinter);

            //queue.DefaultPrintTicket = new PrintTicket { PageMediaSize = new PageMediaSize(PageMediaSizeName.NorthAmericaNumber10Envelope) };

            //var defaultpapersize = queue.DefaultPrintTicket.PageMediaSize;
            var defaultpapersize = new PageMediaSize(279, 0);


            _visual.Measure(new Size(defaultpapersize.Width.Value, double.PositiveInfinity));
            _visual.Arrange(new Rect(_visual.DesiredSize));
            _visual.UpdateLayout();

            Size psize = new Size(defaultpapersize.Width.Value, _visual.ActualHeight);

            queue.UserPrintTicket.PageMediaSize = new PageMediaSize(psize.Width, psize.Height);
            //queue.UserPrintTicket.PageMediaSize = new PageMediaSize(279, 900);
            var writer = PrintQueue.CreateXpsDocumentWriter(queue);

            writer.Write(_visual);

            //with flow document
            //FlowDocument doc = new FlowDocument(new BlockUIContainer(_visual));
            //IDocumentPaginatorSource idpSource = doc;
            //writer.Write(idpSource.DocumentPaginator);
  
        }

        public static async void Print(OrderVM order)
        {
           await Application.Current.Dispatcher.BeginInvoke(new Action(()=>{

               var culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

               var store = GConfig.POS_Setup;
               var h = order._orderHeader;
               bool IsDiscountSelected = false;
               decimal dServiceFee;
               decimal dTax;
               if (h.dOrderAmtDue > 0 || order.Discount > 0)
                   IsDiscountSelected = true;
               dTax = BaseAppUI.Properties.Settings.Default.Tax1_State + BaseAppUI.Properties.Settings.Default.Tax2_Village;
               var varTax1 = ((order.TaxSubtotal) * BaseAppUI.Properties.Settings.Default.Tax1_State/dTax).ToString("C", culture);
               var varTax2 = ((order.TaxSubtotal) * BaseAppUI.Properties.Settings.Default.Tax2_Village/dTax).ToString("C", culture);
               if (h.dOrderServiceFee != null)
                   dServiceFee = (decimal)(h.dOrderServiceFee);
               else
                   dServiceFee = 0;
               ReceiptModel model = new ReceiptModel()
               {
                   StoreName = store.vCorpName,
                   Address1 = Properties.Settings.Default.BusinessAddress1,
                   Address2 = Properties.Settings.Default.BusinessAddress2,
                   Date = DateTime.Now.ToString("MMMM dd, yyyy", culture),
                   Time = DateTime.Now.ToString("h:mm tt", culture),
                   Phone = Properties.Settings.Default.BusinessTel,
                   CustomerName = order.CustomerName,
                   ReceiptNo = order.OrderNumber.ToString() + " -- " + h.vOrderType,
                   PaymentType = order.PaymentType,
                   PaymentRef = h.vOrderPaymentRef,
                   Authorization = "Authorization " + h.vApprovalCode,
                   TaxSubtotal = order.TaxSubtotal.ToString("C", culture),
                   Tax1 = varTax1.ToString(),
                   Tax2 = varTax2.ToString(),
                   Tax1Label = "Tax 1 (" + BaseAppUI.Properties.Settings.Default.Tax1_State + "%)",
                   Tax2Label = "Tax 2 (" + BaseAppUI.Properties.Settings.Default.Tax2_Village + "%)",

                   TaxText = "Tax 1 (" + varTax1.ToString() + ") + Tax 2 (" + varTax2.ToString() + ")",
                   Subtotal = order.Subtotal.ToString("C", culture),
                   Total = order.AllTotal.ToString("C", culture),


                   GratuityLabel = "Gratuity (" + BaseAppUI.Properties.Settings.Default.ServiceFee + "%)",
                   Gratuity = "$" + dServiceFee.ToString("#.00"),
                   GratuitySelected = order.ServiceFeeIsSelected,

                   DiscountLabel = "Discount",
                   DiscountAmt = "("+order.Discount.ToString("C", culture)+")",
                   DiscountSelected = IsDiscountSelected,


                   //   Tips = new List<TipLine>
                   //{
                   //    new TipLine{Title="15% (Tip $20.25, Total $155.55)"},
                   //    new TipLine{Title="20% (Tip $27.00, Total $162.00)"},
                   //    new TipLine{Title="25% (Tip $33.75, Total $168.75)"}
                   //}


               };

               model.Lines=new List<ReceiptLine>();
               foreach (var line in order.OrderLines)
               {
                   string title=null;
                   if(line.Quantity<2)
                       title=line.ItemName;
                   else title=string.Format("{0}  x {1}",line.ItemName,line.Quantity);

                   var rline = new ReceiptLine {Title=title,Amount=line.Total.ToString("C",culture) };
                   model.Lines.Add(rline);
               }



               _printmodel(model);
           
           
           }));

             
            
          
          

        }

    }
}
