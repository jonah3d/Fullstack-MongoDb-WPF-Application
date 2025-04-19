using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontModel
{
    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("province")]
        public string Provincia { get; set; }

        [BsonElement("postalcode")]
        public string PostalCode { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }
    }
}
