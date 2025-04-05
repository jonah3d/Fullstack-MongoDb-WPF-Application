using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Vat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id{ get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("percentage")]
        public int Percentage { get; set; }


        [BsonElement("description")]
        public string Description { get; set; }
    }
}
