using InvoisEGS.ApiClient.XModels;
using InvoisEGS.Models;

namespace InvoisEGS.Utilities
{
    public class MyInvoiceTransformer
    {
        private ManagerInvoice? _managerInvoice;
        private string? _invoiceCurrencyCode;
        private string? _taxCurrencyCode;

        public MyInvoice Transform(RelayData relayData)
        {
            if (relayData?.ManagerInvoice == null)
                throw new ArgumentNullException(nameof(relayData.ManagerInvoice));

            _managerInvoice = relayData.ManagerInvoice;
            _invoiceCurrencyCode = relayData.DocumentCurrencyCode;
            _taxCurrencyCode = relayData.TaxCurrencyCode;

            if (!DateTime.TryParseExact(relayData.LocalIssueDate, "yyyy-MM-dd HH:mm:ss", null,
                System.Globalization.DateTimeStyles.AdjustToUniversal, out DateTime utcDateTime))
            {
                throw new FormatException($"Invalid date format: {relayData.LocalIssueDate}");
            }

            var invoice = new MyInvoice
            {
                ID = new List<ID> { new ID { Value = relayData.ManagerInvoice.Reference } },
                IssueDate = new List<TextValue> { new TextValue { Value = utcDateTime.ToString("yyyy-MM-dd") } },
                IssueTime = new List<TextValue> { new TextValue { Value = utcDateTime.ToString("HH:mm:ssZ") } },
                DocumentCurrencyCode = new List<TextValue> { new TextValue { Value = relayData.DocumentCurrencyCode } },
                TaxCurrencyCode = new List<TextValue> { new TextValue { Value = relayData.TaxCurrencyCode } },
                InvoiceTypeCode = new List<InvoiceTypeCode> 
                { 
                    new InvoiceTypeCode 
                    { 
                        Value = ((int)relayData.InvoiceTypeCode).ToString("00"),
                        ListVersionID = relayData.ListVersionID 
                    } 
                },
                AccountingSupplierParty = new List<AccountingSupplierParty> { relayData.AppConfig.Supplier },
                AccountingCustomerParty = new List<AccountingCustomerParty> { relayData.Buyer }
            };

            if (invoice.AccountingSupplierParty[0].Party[0].IndustryClassificationCode[0].Value == "0000")
            {
                invoice.AccountingSupplierParty[0].Party[0].IndustryClassificationCode[0].Value = "00000";
                invoice.AccountingSupplierParty[0].Party[0].IndustryClassificationCode[0].Name = "Not Applicable";
            }

            // Add TaxExchangeRate if currencies are different
            if (_taxCurrencyCode != _invoiceCurrencyCode)
            {
                invoice.TaxExchangeRate = new TaxExchangeRate
                {
                    SourceCurrencyCode = _invoiceCurrencyCode,
                    TargetCurrencyCode = _taxCurrencyCode,
                    CalculationRate = _managerInvoice.ExchangeRate
                };
            }

            invoice.InvoiceLine = CreateInvoiceLines();
            invoice.AllowanceCharge = CalculateInvoiceAllowanceCharges(invoice.InvoiceLine);
            invoice.TaxTotal = CalculateTaxTotals(invoice.InvoiceLine);
            invoice.LegalMonetaryTotal = CalculateLegalMonetaryTotal(invoice.InvoiceLine);    // Add this line

            return invoice;
        }

        private List<AllowanceCharge> CalculateInvoiceAllowanceCharges(List<InvoiceLine> invoiceLines)
        {
            var invoiceAllowanceCharges = new List<AllowanceCharge>();

            // Sum all discounts (ChargeIndicator = false)
            decimal totalDiscounts = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => !ac.ChargeIndicator.FirstOrDefault()?.Value ?? false)
                    .Sum(ac => ac.Amount.FirstOrDefault()?.Value ?? 0) ?? 0);

            if (totalDiscounts > 0)
            {
                var discountCharge = new AllowanceCharge();
                discountCharge.ChargeIndicator = new List<BoolValue> { new BoolValue { Value = false } };
                discountCharge.AllowanceChargeReason = new List<TextValue> { new TextValue { Value = "Total Discounts" } };
                discountCharge.Amount = new List<Amount> { new Amount { Value = totalDiscounts, CurrencyID = _invoiceCurrencyCode } };
                invoiceAllowanceCharges.Add(discountCharge);
            }

            // Sum all charges (ChargeIndicator = true)
            decimal totalCharges = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => ac.ChargeIndicator.FirstOrDefault()?.Value ?? false)
                    .Sum(ac => ac.Amount.FirstOrDefault()?.Value ?? 0) ?? 0);

            if (totalCharges > 0)
            {
                var chargeCharge = new AllowanceCharge();
                chargeCharge.ChargeIndicator = new List<BoolValue> { new BoolValue { Value = true } };
                chargeCharge.AllowanceChargeReason = new List<TextValue> { new TextValue { Value = "Total Charges" } };
                chargeCharge.Amount = new List<Amount> { new Amount { Value = totalCharges, CurrencyID = _invoiceCurrencyCode } };
                invoiceAllowanceCharges.Add(chargeCharge);
            }

            return invoiceAllowanceCharges;
        }

        private List<InvoiceLine> CreateInvoiceLines()
        {
            var invoiceLines = new List<InvoiceLine>();
            int lineNumber = 0;

            foreach (var line in _managerInvoice.Lines)
            {
                // Basic calculations
                decimal quantity = Math.Round(line.Qty != 0 ? line.Qty : 1, 5);
                decimal taxRate = line.TaxCode?.Rate ?? 0;
                decimal priceAmount = _managerInvoice.AmountsIncludeTax && taxRate > 0
                    ? Math.Round(line.UnitPrice / (1 + (taxRate / 100)), 4)
                    : line.UnitPrice;
                decimal subtotal = Math.Round(quantity * priceAmount, 2);
                decimal lineExtensionAmount = subtotal;
                decimal taxableAmount = lineExtensionAmount;
                decimal taxAmount = Math.Round(taxableAmount * (taxRate / 100), 2);
                var uom = line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.UOMCodeGuid) ?? line.Item?.UnitName ?? "";
                var invoiceLine = new InvoiceLine();
                
                // Set basic elements
                invoiceLine.SetId((++lineNumber).ToString());
                invoiceLine.SetInvoicedQuantity(quantity, uom);
                invoiceLine.SetLineExtensionAmount(lineExtensionAmount, _invoiceCurrencyCode);

                // Set tax elements
                var taxScheme = new TaxScheme("OTH", "UN/ECE 5153", "6");
                var taxCategory = new TaxCategory();
                var txCat = line.TaxCode?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.TaxTypeGuid);
                taxCategory.SetId(txCat);

                if (txCat == "E")
                    taxCategory.SetTaxExemptionReason(line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.ExemptionReasonGuid));
                
                taxCategory.SetTaxScheme(taxScheme);

                var taxTotal = new TaxTotal();
                taxTotal.TaxAmount = new List<Amount> { new Amount(taxAmount, _invoiceCurrencyCode) };
                taxTotal.TaxSubtotal = new List<TaxSubtotal> 
                {
                    new TaxSubtotal 
                    {
                        TaxableAmount = new List<Amount> { new Amount(taxableAmount, _invoiceCurrencyCode) },
                        TaxAmount = new List<Amount> { new Amount(taxAmount, _invoiceCurrencyCode) },
                        Percent = new List<NumValue> { new NumValue(taxRate) },
                        TaxCategory = new List<TaxCategory> { taxCategory }
                    }
                };
                invoiceLine.SetTaxTotal(taxTotal);

                // Set item elements
                var classificationCode = new ItemClassificationCode(
                    line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.CommodityClassGuid) ?? string.Empty,
                    "CLASS"
                );
                var commodityClassification = new CommodityClassification();
                commodityClassification.ItemClassificationCode = new List<ItemClassificationCode> { classificationCode };

                var item = new Item(line.LineDescription ?? line.Item?.ItemName ?? line.Item?.Name ?? string.Empty);
                item.SetCommodityClassification(commodityClassification);

                //var originCountry = new OriginCountry("MYS");
                //item.OriginCountry = new List<OriginCountry> { originCountry };

                invoiceLine.SetItem(item);

                // Set price elements
                invoiceLine.SetPrice(new Price(priceAmount, _invoiceCurrencyCode));
                invoiceLine.SetItemPriceExtension(new ItemPriceExtension(subtotal, _invoiceCurrencyCode));

                // Handle discounts if needed
                if (_managerInvoice.Discount)
                {
                    decimal discountAmount = _managerInvoice.DiscountType == 1
                        ? (_managerInvoice.AmountsIncludeTax && taxRate > 0
                            ? Math.Round(line.DiscountAmount / (1 + (taxRate / 100)), 2)
                            : line.DiscountAmount)
                        : Math.Round(subtotal * (line.DiscountPercentage / 100), 2);

                    if (discountAmount != 0)
                    {
                        var allowanceCharge = new AllowanceCharge(
                            false,
                            _managerInvoice.DiscountType == 1 ? "Discount Amount" : "Discount Percentage",
                            _managerInvoice.DiscountType == 1 ? 0m : line.DiscountPercentage / 100,
                            discountAmount,
                            _invoiceCurrencyCode
                        );
                        invoiceLine.SetAllowanceCharge(allowanceCharge);
                        lineExtensionAmount -= discountAmount;
                    }
                }

                invoiceLines.Add(invoiceLine);
            }

            return invoiceLines;
        }

        private List<TaxTotal> CalculateTaxTotals(List<InvoiceLine> invoiceLines)
        {
            decimal exchangeRate = _managerInvoice.ExchangeRate == 0 ? 1 : _managerInvoice.ExchangeRate;

            // Group tax subtotals by rate
            var taxGroups = invoiceLines
                .SelectMany(line => line.TaxTotal)
                .SelectMany(total => total.TaxSubtotal)
                .GroupBy(sub => sub.Percent.FirstOrDefault()?.Value ?? 0)
                .Select(g => new TaxSubtotal
                {
                    TaxableAmount = new List<Amount> 
                    { 
                        new Amount 
                        { 
                            Value = g.Sum(x => x.TaxableAmount.FirstOrDefault()?.Value ?? 0),
                            CurrencyID = _invoiceCurrencyCode  // TaxableAmount stays in invoice currency
                        } 
                    },
                    TaxAmount = new List<Amount> 
                    { 
                        new Amount 
                        { 
                            Value = g.Sum(x => (x.TaxAmount.FirstOrDefault()?.Value ?? 0) * exchangeRate),
                            CurrencyID = _taxCurrencyCode  // TaxAmount in tax currency
                        } 
                    },
                    Percent = new List<NumValue> { new NumValue { Value = g.Key } },
                    TaxCategory = g.First().TaxCategory
                })
                .ToList();

            // Calculate total tax amount in tax currency
            decimal totalTaxAmount = taxGroups.Sum(g => g.TaxAmount.FirstOrDefault()?.Value ?? 0);

            return new List<TaxTotal>
            {
                new TaxTotal
                {
                    TaxAmount = new List<Amount> 
                    { 
                        new Amount 
                        { 
                            Value = totalTaxAmount,
                            CurrencyID = _taxCurrencyCode  // Document level TaxAmount in tax currency
                        } 
                    },
                    TaxSubtotal = taxGroups
                }
            };
        }

        private List<LegalMonetaryTotal> CalculateLegalMonetaryTotal(List<InvoiceLine> invoiceLines)
        {
            // Line Extension Amount (sum of all line amounts)
            decimal lineExtensionAmount = invoiceLines.Sum(line => 
                line.LineExtensionAmount?.FirstOrDefault()?.Value ?? 0);
        
            // Allowance and Charge Totals
            decimal allowanceTotalAmount = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => !ac.ChargeIndicator.FirstOrDefault()?.Value ?? false)
                    .Sum(ac => ac.Amount.FirstOrDefault()?.Value ?? 0) ?? 0);
        
            decimal chargeTotalAmount = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => ac.ChargeIndicator.FirstOrDefault()?.Value ?? false)
                    .Sum(ac => ac.Amount.FirstOrDefault()?.Value ?? 0) ?? 0);
        
            // Tax Exclusive Amount
            decimal taxExclusiveAmount = lineExtensionAmount - allowanceTotalAmount + chargeTotalAmount;
        
            // Tax Amount (should use exchangeRate if currencies differ)
            decimal exchangeRate = _managerInvoice.ExchangeRate == 0 ? 1 : _managerInvoice.ExchangeRate;
            decimal taxAmount = invoiceLines.Sum(line => 
                line.TaxTotal?.FirstOrDefault()?.TaxAmount?.FirstOrDefault()?.Value ?? 0) * 
                (_taxCurrencyCode != _invoiceCurrencyCode ? exchangeRate : 1);
        
            // Tax Inclusive Amount
            decimal taxInclusiveAmount = taxExclusiveAmount + taxAmount;
        
            // Payable Amount with rounding
            decimal payableRoundingAmount = Math.Round(Math.Ceiling(taxInclusiveAmount) - taxInclusiveAmount, 2);
            decimal payableAmount = Math.Ceiling(taxInclusiveAmount);
        
            var legalMonetaryTotal = new LegalMonetaryTotal();
            legalMonetaryTotal.SetLineExtensionAmount(lineExtensionAmount, _invoiceCurrencyCode);
            legalMonetaryTotal.SetTaxExclusiveAmount(taxExclusiveAmount, _invoiceCurrencyCode);
            legalMonetaryTotal.SetTaxInclusiveAmount(taxInclusiveAmount, _invoiceCurrencyCode);
            legalMonetaryTotal.SetAllowanceTotalAmount(allowanceTotalAmount, _invoiceCurrencyCode);
            legalMonetaryTotal.SetChargeTotalAmount(chargeTotalAmount, _invoiceCurrencyCode);
            legalMonetaryTotal.SetPayableRoundingAmount(payableRoundingAmount, _invoiceCurrencyCode);
            legalMonetaryTotal.SetPayableAmount(payableAmount, _invoiceCurrencyCode);
        
            return new List<LegalMonetaryTotal> { legalMonetaryTotal };
        }
    }
}