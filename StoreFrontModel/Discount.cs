using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Discount
    {

        [BsonElement("percentage")]
        public decimal Percentage { get; set; }

        [BsonElement("validFrom")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ValidFrom { get; set; }

        [BsonElement("validTo")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ValidTo { get; set; }
    }
}
