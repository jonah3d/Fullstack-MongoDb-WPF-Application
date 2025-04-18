using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Cart 
    {
        [BsonId]
        public ObjectId CartId { get; set; }
        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("items")]
        public ObservableCollection<CartItem> Items { get; set; } = new ObservableCollection<CartItem>();

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        [BsonElement("purchased")]
        public bool Purchased { get; set; } = false;

        [BsonIgnore]
        public User User { get; set; }

        public void AddItem(CartItem newItem)
        {
            var existingItem = Items.FirstOrDefault(item =>
                item.ProductId == newItem.ProductId &&
                item.VariantId == newItem.VariantId &&
                item.Size == newItem.Size);

            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                Items.Add(newItem);
            }
        }
    }
}
