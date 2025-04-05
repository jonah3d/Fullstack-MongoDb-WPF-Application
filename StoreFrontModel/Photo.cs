using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Photo
    {
        [BsonElement("url")]
        public string Url { get; set; }
    }
}
