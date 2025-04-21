using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StoreFrontModel
{
    public  class Counter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")] // e.g., "invoice-2025"
        public string Name { get; set; }

        [BsonElement("value")] // last used number
        public int Value { get; set; }
    }
}
