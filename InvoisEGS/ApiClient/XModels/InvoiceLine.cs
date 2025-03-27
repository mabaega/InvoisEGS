using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // InvoiceLine
    public class InvoiceLine
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        [JsonProperty("InvoicedQuantity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "InvoicedQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<InvoicedQuantity> InvoicedQuantity { get; set; }

        [JsonProperty("LineExtensionAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "LineExtensionAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> LineExtensionAmount { get; set; }

        [JsonProperty("AllowanceCharge", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AllowanceCharge> AllowanceCharge { get; set; }

        [JsonProperty("TaxTotal", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxTotal> TaxTotal { get; set; }

        [JsonProperty("Item", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Item", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Item> Item { get; set; }

        [JsonProperty("Price", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Price", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Price> Price { get; set; }

        [JsonProperty("ItemPriceExtension", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ItemPriceExtension", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<ItemPriceExtension> ItemPriceExtension { get; set; }

        public InvoiceLine()
        {
            ID = new List<ID>();
            InvoicedQuantity = new List<InvoicedQuantity>();
            LineExtensionAmount = new List<Amount>();
            AllowanceCharge = new List<AllowanceCharge>();
            TaxTotal = new List<TaxTotal>();
            Item = new List<Item>();
            Price = new List<Price>();
            ItemPriceExtension = new List<ItemPriceExtension>();
        }

        public void SetId(string id) => 
            ID = new List<ID> { new ID(id) };

        //public string GetId() => 
        //    ID?.FirstOrDefault()?.Value;

        public void SetInvoicedQuantity(decimal quantity, string unitCode) => 
            InvoicedQuantity = new List<InvoicedQuantity> { new InvoicedQuantity(quantity, unitCode) };

        //public InvoicedQuantity GetInvoicedQuantity() => 
        //    InvoicedQuantity?.FirstOrDefault();

        public void SetLineExtensionAmount(decimal amount, string currencyId) => 
            LineExtensionAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetLineExtensionAmount() => 
        //    LineExtensionAmount?.FirstOrDefault();

        public void SetItem(Item item) => 
            Item = new List<Item> { item };

        //public Item GetItem() => 
        //    Item?.FirstOrDefault();

        public void SetPrice(Price price) => 
            Price = new List<Price> { price };

        //public Price GetPrice() => 
        //    Price?.FirstOrDefault();

        public void SetTaxTotal(TaxTotal taxTotal) => 
            TaxTotal = new List<TaxTotal> { taxTotal };

        //public TaxTotal GetTaxTotal() => 
        //    TaxTotal?.FirstOrDefault();

        public void SetAllowanceCharge(AllowanceCharge allowanceCharge) => 
            AllowanceCharge = new List<AllowanceCharge> { allowanceCharge };

        //public AllowanceCharge GetAllowanceCharge() => 
        //    AllowanceCharge?.FirstOrDefault();

        public void SetItemPriceExtension(ItemPriceExtension priceExtension) => 
            ItemPriceExtension = new List<ItemPriceExtension> { priceExtension };

        //public ItemPriceExtension GetItemPriceExtension() => 
        //    ItemPriceExtension?.FirstOrDefault();
    }
}
