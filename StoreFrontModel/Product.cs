using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Product
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id{ get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }


        [BsonElement("categoryIds")]
      
        public List<ObjectId> CategoryIds { get; set; }

        [BsonElement("IVATypeId")]
        public ObjectId IvaTypeId { get; set; }

        [BsonElement("TAG")]
        public ObjectId TagId { get; set; }


        [BsonElement("variants")]
        public List<ProductVariant> Variants { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        public List<Category> Categories { get; set; }

        [BsonIgnore]
        public Vat IvaType { get; set; }

        [BsonIgnore]
        public ProductTag Tag { get; set; }

        [BsonIgnore]
        public decimal Price
        {
            get
            {
               
                return Variants?.FirstOrDefault()?.Price ?? 0;
            }
        }

        [BsonIgnore]
        public string FirstImageUrl
        {
            get
            {
                try
                {
                    if (Variants != null && Variants.Count > 0 &&
                        Variants[0].Photos != null && Variants[0].Photos.Count > 0)
                    {
                        return Variants[0].Photos[0].Url;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
