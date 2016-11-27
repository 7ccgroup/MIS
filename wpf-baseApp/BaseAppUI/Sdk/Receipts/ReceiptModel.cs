using System.Collections.Generic;

namespace BaseAppUI.Sdk.Receipts
{
    public class TipLine
    {
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }

    public class SplitLine
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }

    }
    public class ReceiptLine
    {
        public string Title { get; set; }
        public string Comments { get; set; }
        public string Amount { get; set; }
    }
    public class ReceiptModel
    {
        public string StoreName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        public string CustomerName { get; set; }
        public string ReceiptNo { get; set; }

        public string PaymentRef { get; set; }

        public string PaymentType { get; set; }

        public string Authorization { get; set; }
        public IList<ReceiptLine> Lines { get; set; }

        public string TaxSubtotal { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public string Tax1 { get; set; } //SAA
        public string Tax2 { get; set; } //SAA
        public string TaxText { get; set; }//SAA
        public string Tax1Label { get; set; }//SAA
        public string Tax2Label { get; set; }//SAA
        public string Discount { get; set; } //SAA

        public bool GratuitySelected { get; set; } //SAA
        public string GratuityLabel { get; set; } //SAA
        public string Gratuity { get; set; } //SAA

        public string DiscountLabel { get; set; } // Amjad
        public string DiscountAmt { get; set; } // Amjad
        public bool DiscountSelected { get; set; } //SAA
        public IList<TipLine> Tips { get; set; }
        public IList<SplitLine> Splits { get; set; }
        public bool HasSplit { get; set; }

        public string SplitLabel { get; set; }
        public string SplitAmount { get; set; }


    }
}
