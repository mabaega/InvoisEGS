using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.ApiClient.ApiModels.EInvoice;
using InvoisEGS.ApiClient.XModels;
using InvoisEGS.Models;

namespace InvoisEGS.Utilities
{
    public class InvoiceTransformer
    {
        private ManagerInvoice? _managerInvoice;
        private string? _invoiceCurrencyCode;
        private string? _taxCurrencyCode;

        public Invoice Transform(RelayData relayData)
        {
            _managerInvoice = relayData.ManagerInvoice;
            _invoiceCurrencyCode = relayData.DocumentCurrencyCode;
            _taxCurrencyCode = relayData.TaxCurrencyCode;

            if (!DateTime.TryParseExact(relayData.LocalIssueDate, "yyyy-MM-dd HH:mm:ss", null,
                System.Globalization.DateTimeStyles.AdjustToUniversal, out DateTime utcDateTime))
            {
                throw new FormatException($"Invalid date format: {relayData.LocalIssueDate}");
            }

            Invoice invoice = new()
            {
                ID = new ID { Text = relayData.ManagerInvoice.Reference },
                IssueDate = utcDateTime.ToString("yyyy-MM-dd"),
                IssueTime = utcDateTime.ToString("HH:mm:ssZ"),
                DocumentCurrencyCode = relayData.DocumentCurrencyCode,
                TaxCurrencyCode = relayData.TaxCurrencyCode,
                InvoiceTypeCode = new InvoiceTypeCode
                {
                    ListVersionID = relayData.ListVersionID,
                    Text = ((int)relayData.InvoiceTypeCode).ToString("00")
                }
            };

            if (relayData.InvoiceTypeCode is InvoiceTypeCodeEnum.CreditNote or
                InvoiceTypeCodeEnum.DebitNote)
            {
                invoice.BillingReference = new BillingReference
                {
                    AdditionalDocumentReference = new AdditionalDocumentReference
                    {
                        ID = new ID { Text = relayData.CNDNReferences[0] },
                        UUID = relayData.CNDNReferences[1]
                    }
                };
            }

            invoice.AccountingSupplierParty = relayData.AppConfig.Supplier;
            invoice.AccountingCustomerParty = relayData.Buyer;

            invoice.InvoiceLine = [.. CreateInvoiceLines()];

            invoice.AllowanceCharge = CalculateInvoiceAllowanceCharges(invoice.InvoiceLine);

            if (_taxCurrencyCode != _invoiceCurrencyCode)
            {
                invoice.TaxExchangeRate = new TaxExchangeRate
                {
                    SourceCurrencyCode = _invoiceCurrencyCode,
                    TargetCurrencyCode = _taxCurrencyCode,
                    CalculationRate = _managerInvoice.ExchangeRate
                };
            }

            invoice.TaxTotal = CalculateTaxTotals(invoice.InvoiceLine);
            invoice.LegalMonetaryTotal = CalculateLegalMonetaryTotal(invoice.InvoiceLine);

            return invoice;
        }

        private List<InvoiceLine> CreateInvoiceLines()
        {
            List<Line> lines = _managerInvoice.Lines;
            List<InvoiceLine> invoiceLines = new();
            bool amountsIncludeTax = _managerInvoice.AmountsIncludeTax;
            bool hasDiscount = _managerInvoice.Discount;
            int discountType = _managerInvoice.DiscountType;

            int i = 0;

            foreach (Line line in lines)
            {
                // Basic calculations with proper decimal places
                decimal invoicedQuantity = Math.Round(line.Qty != 0 ? line.Qty : 1, 5); // Up to 5 decimal places as recommended
                decimal unitPrice = line.UnitPrice;
                decimal taxRate = line.TaxCode?.Rate ?? 0;

                // Price calculations
                decimal priceAmount = amountsIncludeTax && taxRate > 0
                    ? Math.Round(unitPrice / (1 + (taxRate / 100)), 4)
                    : unitPrice;

                decimal subtotal = Math.Round(invoicedQuantity * priceAmount, 2);
                decimal lineExtensionAmount = subtotal;

                // Handle discounts
                List<AllowanceCharge> allowanceCharges = new();
                if (hasDiscount)
                {
                    decimal discountAmount;
                    if (discountType == 1)
                    {
                        discountAmount = amountsIncludeTax && taxRate > 0
                            ? Math.Round(line.DiscountAmount / (1 + (taxRate / 100)), 2)
                            : line.DiscountAmount;
                    }
                    else
                    {
                        discountAmount = Math.Round(subtotal * (line.DiscountPercentage / 100), 2);
                    }

                    if (discountAmount != 0){
                        allowanceCharges.Add(new AllowanceCharge
                        {
                            ChargeIndicator = false,
                            AllowanceChargeReason = discountType == 1 ? "Discount Amount" : "Discount Percentage",
                            MultiplierFactorNumericValue = discountType == 1 ? 0 : line.DiscountPercentage / 100,
                            Amount = new Amount(_invoiceCurrencyCode, discountAmount)
                        });
                    }
                    lineExtensionAmount -= discountAmount;
                }

                // Tax calculations
                decimal taxableAmount = lineExtensionAmount;
                decimal taxAmount = Math.Round(taxableAmount * (taxRate / 100), 2);

                string itemTaxType = line.TaxCode?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.TaxTypeGuid) ?? string.Empty;
                string itemExemptionReason = line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.ExemptionReasonGuid) ?? string.Empty;
                string commodityClass = line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.CommodityClassGuid) ?? string.Empty;

                string taxCategoryId = itemTaxType;
                string exemptionReason = taxRate == 0 || taxCategoryId == "E" ? itemExemptionReason : null;

                InvoiceLine invoiceLine = new()
                {
                    ID = new ID((++i).ToString()),
                    InvoicedQuantity = new InvoicedQuantity(line.Item?.UnitName ?? "*", invoicedQuantity),
                    LineExtensionAmount = new Amount(_invoiceCurrencyCode, lineExtensionAmount),
                    AllowanceCharge = allowanceCharges.ToArray(),
                    // In CreateInvoiceLines method, update the TaxTotal creation:
                    TaxTotal = new TaxTotal
                    {
                        TaxAmount = new Amount(_invoiceCurrencyCode, taxAmount),
                        TaxSubtotal = new[]
                        {
                            new TaxSubtotal
                            {
                                TaxableAmount = new Amount(_invoiceCurrencyCode, taxableAmount),
                                TaxAmount = new Amount(_invoiceCurrencyCode, taxAmount ),
                                Percent = taxRate,
                                TaxCategory = new TaxCategory
                                {
                                    ID = new ID(taxCategoryId),
                                    TaxExemptionReason = exemptionReason,
                                    TaxScheme = new TaxScheme
                                    {
                                        ID = new ID { SchemeID = "UN/ECE 5153", SchemeAgencyID = "6", Text = "OTH" }
                                    }
                                }
                            }
                        }
                    },
                    Item = new Item
                    {
                        Description = line.Item?.ItemName ?? line.Item?.Name ?? line.LineDescription ?? "",
                        CommodityClassification = new[]
                        {
                            new CommodityClassification
                            {
                                ItemClassificationCode = new ItemClassificationCode
                                {
                                    ListID = "CLASS",
                                    Text = commodityClass // Mandatory 3-char classification code
                                }
                            }
                        }
                    },
                    Price = new Price
                    {
                        PriceAmount = new Amount(_invoiceCurrencyCode, priceAmount)
                    },
                    ItemPriceExtension = new ItemPriceExtension
                    {
                        Amount = new Amount(_invoiceCurrencyCode, subtotal)
                    }
                };

                invoiceLines.Add(invoiceLine);
            }

            return invoiceLines;
        }

        private TaxTotal CalculateTaxTotals(IEnumerable<InvoiceLine> invoiceLines)
        {
            decimal exchangeRate = _managerInvoice.ExchangeRate == 0 ? 1 : _managerInvoice.ExchangeRate;

            // Group tax subtotals by rate
            IEnumerable<TaxSubtotal> taxGroups = invoiceLines
                .SelectMany(line => line.TaxTotal.TaxSubtotal)
                .GroupBy(sub => sub.Percent)
                .Select(g => new TaxSubtotal
                {
                    TaxableAmount = new Amount(_invoiceCurrencyCode, g.Sum(x => x.TaxableAmount.NumericValue)),
                    TaxAmount = new Amount(_taxCurrencyCode, g.Sum(x => x.TaxAmount.NumericValue * exchangeRate)),
                    Percent = g.Key,
                    TaxCategory = g.First().TaxCategory
                });

            decimal totalTaxAmount = taxGroups.Sum(g => g.TaxAmount.NumericValue);

            // Create single TaxTotal with all subtotals
            return new TaxTotal
            {
                TaxAmount = new Amount(_taxCurrencyCode, totalTaxAmount),
                TaxSubtotal = taxGroups.ToArray()
            };
        }

        private LegalMonetaryTotal CalculateLegalMonetaryTotal(IEnumerable<InvoiceLine> invoiceLines)
        {
            // Line Extension Amount (sum of all line amounts)
            decimal lineExtensionAmount = invoiceLines.Sum(line => line.LineExtensionAmount.NumericValue);

            // Allowance and Charge Totals
            decimal allowanceTotalAmount = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => !ac.ChargeIndicator).Sum(ac => ac.Amount.NumericValue) ?? 0);
            decimal chargeTotalAmount = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => ac.ChargeIndicator).Sum(ac => ac.Amount.NumericValue) ?? 0);

            // Tax Amount
            decimal taxAmount = invoiceLines.Sum(line => line.TaxTotal.TaxAmount.NumericValue);

            // Tax Exclusive Amount
            decimal taxExclusiveAmount = lineExtensionAmount;

            // Tax Inclusive Amount
            decimal taxInclusiveAmount = taxExclusiveAmount + taxAmount;

            // Payable Amount with rounding
            decimal payableRoundingAmount = Math.Round(Math.Ceiling(taxInclusiveAmount) - taxInclusiveAmount, 2);
            decimal payableAmount = Math.Ceiling(taxInclusiveAmount);

            return new LegalMonetaryTotal
            {
                LineExtensionAmount = new Amount(_invoiceCurrencyCode, lineExtensionAmount),
                TaxExclusiveAmount = new Amount(_invoiceCurrencyCode, taxExclusiveAmount),
                TaxInclusiveAmount = new Amount(_invoiceCurrencyCode, taxInclusiveAmount),
                AllowanceTotalAmount = new Amount(_invoiceCurrencyCode, allowanceTotalAmount),
                ChargeTotalAmount = new Amount(_invoiceCurrencyCode, chargeTotalAmount),
                PayableRoundingAmount = new Amount(_invoiceCurrencyCode, payableRoundingAmount),
                PayableAmount = new Amount(_invoiceCurrencyCode, Math.Ceiling(taxInclusiveAmount))
            };
        }

        private AllowanceCharge[] CalculateInvoiceAllowanceCharges(IEnumerable<InvoiceLine> invoiceLines)
        {
            List<AllowanceCharge> invoiceAllowanceCharges = new();

            // Sum all discounts (ChargeIndicator = false)
            decimal totalDiscounts = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => !ac.ChargeIndicator).Sum(ac => ac.Amount.NumericValue) ?? 0);
            if (totalDiscounts > 0)
            {
                invoiceAllowanceCharges.Add(new AllowanceCharge
                {
                    ChargeIndicator = false,
                    AllowanceChargeReason = "Total Discount",
                    Amount = new Amount(_invoiceCurrencyCode, totalDiscounts)
                });
            }

            // Sum all charges (ChargeIndicator = true)
            decimal totalCharges = invoiceLines.Sum(line =>
                line.AllowanceCharge?.Where(ac => ac.ChargeIndicator).Sum(ac => ac.Amount.NumericValue) ?? 0);
            if (totalCharges > 0)
            {
                invoiceAllowanceCharges.Add(new AllowanceCharge
                {
                    ChargeIndicator = true,
                    AllowanceChargeReason = "Total Charges",
                    Amount = new Amount(_invoiceCurrencyCode, totalCharges)
                });
            }

            return invoiceAllowanceCharges.ToArray();
        }

    }

}