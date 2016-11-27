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
    public class CustomerReceiptPrinter
    {

        private static FrameworkElement GetVisual(ReceiptModel model)
        {

            var myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("pack://application:,,,/View/Receipts/CustomerReceiptView.xaml",
                        UriKind.RelativeOrAbsolute);

            ContentControl control = new ContentControl() { Content = model, ContentTemplate = (DataTemplate)myResourceDictionary["CustomerReceiptView"] };

            var _visual = new StackPanel();
            _visual.Children.Add(control);
          


            return _visual;
        }

    
        private static void Printmodel(ReceiptModel model)
        {

            var _visual = GetVisual(model);

            //If pinter is not setup receipt can be send to a file....
            var queue = new LocalPrintServer().DefaultPrintQueue;
            try
            {
                //Printer Assignment Fix by Amjad - 6/14
                queue = new LocalPrintServer().GetPrintQueue(BaseAppUI.Properties.Settings.Default.POSMainPrinter);
            }
            catch
            {
                MessageBox.Show("Printer not setup");
            //catch exception
            }
            var defaultpapersize = new PageMediaSize(279, 0);


                _visual.Measure(new Size(defaultpapersize.Width.Value, double.PositiveInfinity));
                _visual.Arrange(new Rect(_visual.DesiredSize));
                _visual.UpdateLayout();

                Size psize = new Size(defaultpapersize.Width.Value, _visual.ActualHeight);

                queue.UserPrintTicket.PageMediaSize = new PageMediaSize(psize.Width, psize.Height);
                var writer = PrintQueue.CreateXpsDocumentWriter(queue);

                writer.Write(_visual);
            
            

        }

        public static async void Print(OrderVM order)
        {
            await Task.Factory.StartNew(() =>
            { 
            
           

            Application.Current.Dispatcher.BeginInvoke(new Action(()=>{

               var culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

               var store = GConfig.POS_Setup;
               var h = order._orderHeader;
               decimal dServiceFee=0;
               bool IsDiscountSelected = false;
               decimal dTax;
               dTax = BaseAppUI.Properties.Settings.Default.Tax1_State + BaseAppUI.Properties.Settings.Default.Tax2_Village;
               var varTax1 = ((order.TaxSubtotal) * BaseAppUI.Properties.Settings.Default.Tax1_State/dTax).ToString("C", culture);
               var varTax2 = ((order.TaxSubtotal) * BaseAppUI.Properties.Settings.Default.Tax2_Village/dTax).ToString("C", culture);
               if (h.dOrderAmtDue > 0 || order.Discount>0)
                   IsDiscountSelected = true;
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
                   Gratuity = "$" + dServiceFee.ToString("#,#.00"),
                   GratuitySelected = order.ServiceFeeIsSelected,

                   DiscountLabel = "Discount",
                   DiscountAmt = "(" + order.Discount.ToString("C", culture) + ")",
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

                   var rline = new ReceiptLine { Title = title, Amount = line.Total.ToString("C", culture), Comments = line.Comments };
                   model.Lines.Add(rline);
               }


                if (h.iSplitCount.HasValue)
                {
                    model.HasSplit = true;

                    // var splits = Model.SplitPaymentInfo.FromText(h.vSplitPayment);
                    var splits = h.vSplitPayment.Split(new string[] { ";" }, StringSplitOptions.None);
                    model.Splits = new List<SplitLine>();
                    //foreach (var split in splits)
                    //{
                    //    model.Splits.Add(new SplitLine
                    //    {
                    //        Title = split[Model.SplitPaymentInfoFlags.id],
                    //        Amount = decimal.Parse(split[Model.SplitPaymentInfoFlags.amount]).ToString("C", culture),
                    //        Type = string.Format("({0})", split[Model.SplitPaymentInfoFlags.type])


                    //    });
                    //}
                    foreach (var split in splits)
                    {
                        var parts = split.Split(new string[] { "," }, StringSplitOptions.None);

                        model.Splits.Add(new SplitLine
                        {
                            //0 paymenttype
                            //1 cardnumber
                            //2 amount
                            //3 splitname
                            Amount = decimal.Parse(parts[2]).ToString("C", culture),
                            Title = string.Format("{0} / {1}", parts[3], h.iSplitCount.Value)

                        });
                    }
                }




                Printmodel(model);
           
           
           }));

            });
            
          
          

        }

    }
}
