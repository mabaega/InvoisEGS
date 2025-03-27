namespace InvoisEGS.Models
{
    public class ManagerInvoice
    {
        public DateTime IssueDate { get; set; }
        public DateTime DueDateDate { get; set; }
        public string Reference { get; set; }
        public RefInvoice RefInvoice { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
        public string Description { get; set; }
        public List<Line> Lines { get; set; }
        public bool HasLineNumber { get; set; } = false;
        public bool HasLineDescription { get; set; } = false;
        public bool Discount { get; set; } = false;
        public int DiscountType { get; set; } = 0;
        public bool AmountsIncludeTax { get; set; } = false;
        public bool WithholdingTax { get; set; } = false;
        public int WithholdingTaxType { get; set; } = 0;
        public decimal WithholdingTaxPercentage { get; set; } = 0;
        public decimal WithholdingTaxAmount { get; set; } = 0;
    }
    public class Line
    {
        public LineItem Item { get; set; }
        public string LineDescription { get; set; }
        public decimal Qty { get; set; } = 0;
        public decimal UnitPrice { get; set; } = 0;
        public decimal DiscountPercentage { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public TaxCode TaxCode { get; set; }
        public CustomFields2 CustomFields2 { get; set; }
    }
    public class LineItem
    {
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public bool HasDefaultLineDescription { get; set; } = false;
        public string DefaultLineDescription { get; set; }
        public CustomFields2 CustomFields2 { get; set; }
    }

    public class CustomFields2
    {
        public Dictionary<string, string> Strings { get; set; } = [];
        public Dictionary<string, decimal> Decimals { get; set; } = [];
        public Dictionary<string, DateTime?> Dates { get; set; } = [];
        public Dictionary<string, bool> Booleans { get; set; } = [];
        public Dictionary<string, List<string>> StringArrays { get; set; } = [];
    }

    public class TaxCode
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public decimal TaxRate { get; set; }
        public int Type { get; set; } // type 1 then get Tax from component
        public decimal Rate { get; set; }
        public List<Components> Components { get; set; }
        public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
        public CustomFields2 CustomFields2 { get; set; }
    }

    public class Components
    {
        public string Name { get; set; }
        public decimal ComponentRate { get; set; } = 0;
    }

    public class RefInvoice
    {
        public string Reference { get; set; }
        public DateTime IssueDate { get; set; }
    }
}