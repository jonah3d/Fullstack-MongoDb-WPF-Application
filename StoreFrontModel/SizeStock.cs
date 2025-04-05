using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class SizeStock
    {
        [BsonElement("size")]
        public string Size { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }
    }
}
