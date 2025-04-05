using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class ProductVariant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id{ get; set; }

        [BsonElement("color")]
        public string Color { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("discount")]
        public Discount Discount { get; set; }

        [BsonElement("photos")]
        public List<Photo> Photos { get; set; }

        [BsonElement("sizes")]
        public List<SizeStock> Sizes { get; set; }



    }
}
