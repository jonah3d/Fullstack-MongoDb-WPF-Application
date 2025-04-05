using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id{ get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("parentIds")]
        public List<ObjectId> parentIds{ get; set; }
    }
}
