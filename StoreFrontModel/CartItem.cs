using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace StoreFrontModel
{
    public class CartItem : INotifyPropertyChanged
    {
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ProductId { get; set; }

        [BsonElement("variantId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId VariantId { get; set; }

        [BsonElement("quantity")]
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        private int quantity;

        [BsonElement("size")]
        public string Size { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}