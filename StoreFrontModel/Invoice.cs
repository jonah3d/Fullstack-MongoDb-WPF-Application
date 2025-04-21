using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Invoice
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id{ get; set; }

        [BsonElement("invoiceNumber")]
        public string InvoiceNumber { get; set; } 

        [BsonElement("invoiceDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;



        // -- Company Info --
        [BsonElement("companyName")]
        public string CompanyName { get; set; }

        [BsonElement("companyNIF")]
        public string CompanyNIF { get; set; }

        [BsonElement("companyRegistry")]
        public string CompanyRegistry { get; set; }

        [BsonElement("companyAddress")]
        public Address CompanyAddress { get; set; }

        [BsonElement("companyPhone")]
        public string CompanyPhone { get; set; }

        [BsonElement("companyEmail")]
        public string CompanyEmail { get; set; }

        // -- Customer Info --
        [BsonElement("customerNIF")]
        public string CustomerNIF { get; set; }

        [BsonElement("customerName")]
        public string CustomerName { get; set; }

        [BsonElement("customerAddress")]
        public Address CustomerAddress { get; set; }

        [BsonElement("customerPhone")]
        public string CustomerPhone { get; set; }

        [BsonElement("customerEmail")]
        public string CustomerEmail { get; set; }


        // -- Invoice Items --


        public List<InvoiceItems> Items { get; set; }


        // -- Summary

        [BsonElement("subTotal")]
        public decimal SubTotal { get; set; }
        [BsonElement("shippingCost")]
        public decimal ShippingCost { get; set; }

        [BsonElement("totalVAT")]
        public decimal TotalVAT { get; set; }

        [BsonElement("total")]
        public decimal Total { get; set; }

        [BsonElement("shippingMethod")]
        public string ShippingMethod { get; set; }

        public Invoice() { }
    }
}
