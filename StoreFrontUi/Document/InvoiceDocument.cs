using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StoreFrontModel;

namespace StoreFrontUi.Document
{
    public class InvoiceDocument : IDocument
    {
        private readonly Invoice _invoice;

        public InvoiceDocument(Invoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text("Thank you for your business!");
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text(_invoice.CompanyName).Bold().FontSize(18);
                    stack.Item().Text(FormatAddress(_invoice.CompanyAddress));
                    stack.Item().Text($"Phone: {_invoice.CompanyPhone} | Email: {_invoice.CompanyEmail}");
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Stack(stack =>
            {
                stack.Item().Element(ComposeInvoiceInfo);
                stack.Item().Element(ComposeAddresses);
                stack.Item().Element(ComposeItemsTable);
                stack.Item().Element(ComposeTotals);
            });
        }

        void ComposeInvoiceInfo(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Text($"Invoice #: {_invoice.InvoiceNumber}");
                row.RelativeColumn().Text($"Date: {_invoice.InvoiceDate:yyyy-MM-dd}");
                row.RelativeColumn().Text($"NIF: {_invoice.CustomerNIF}");
            });
        }

        void ComposeAddresses(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text("From:").Bold();
                    stack.Item().Text(FormatAddress(_invoice.CompanyAddress));
                });

                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text("To:").Bold();
                    stack.Item().Text(_invoice.CustomerName);
                    stack.Item().Text(FormatAddress(_invoice.CustomerAddress));
                });
            });
        }

        void ComposeItemsTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4); // Description
                    columns.ConstantColumn(40); // Qty
                    columns.ConstantColumn(60); // Price
                    columns.ConstantColumn(40); // Discount
                    columns.ConstantColumn(40); // VAT
                    columns.ConstantColumn(60); // Line Total
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Description");
                    header.Cell().Element(CellStyle).Text("Qty");
                    header.Cell().Element(CellStyle).Text("Price");
                    header.Cell().Element(CellStyle).Text("Discount %");
                    header.Cell().Element(CellStyle).Text("VAT %");
                    header.Cell().Element(CellStyle).Text("Total");
                });

                foreach (var item in _invoice.Items)
                {
                    table.Cell().Element(CellStyle).Text(item.Description);
                    table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                    table.Cell().Element(CellStyle).Text($"{item.Price:C}");
                    table.Cell().Element(CellStyle).Text($"{item.DiscountPercent}%");
                    table.Cell().Element(CellStyle).Text($"{item.VatPercentage}%");
                    table.Cell().Element(CellStyle).Text($"{item.LineAmount:C}");
                }
            });
        }

        void ComposeTotals(IContainer container)
        {
            container.AlignRight().Stack(stack =>
            {
                stack.Item().Text($"Subtotal: {_invoice.SubTotal:C}");
                stack.Item().Text($"Shipping: {_invoice.ShippingCost:C}");
                stack.Item().Text($"VAT: {_invoice.TotalVAT:C}");
                stack.Item().Text($"Total: {_invoice.Total:C}").Bold().FontSize(14);
            });
        }

        private static IContainer CellStyle(IContainer container) =>
            container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);

        private static string FormatAddress(Address address)
        {
            if (address == null) return string.Empty;
            return $"{address.Street}, {address.City}, {address.Provincia}, {address.PostalCode}, {address.Country}";
        }
    }
}