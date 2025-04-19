using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace StoreFrontModel
{
    public class CartItem : INotifyPropertyChanged
    {
        // Private fields
        private ObjectId productId;
        private ObjectId variantId;
        private int quantity;
        private string size;
        private decimal basePrice;
        private decimal discountAmount;
        private decimal discountPercentage;
        private decimal lineAmount;
        private decimal vatPercentage;
        private decimal vatAmount;
        private decimal netAmount;

        // Bson Properties
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ProductId
        {
            get => productId;
            set { if (productId != value) { productId = value; OnPropertyChanged(nameof(ProductId)); } }
        }

        [BsonElement("variantId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId VariantId
        {
            get => variantId;
            set { if (variantId != value) { variantId = value; OnPropertyChanged(nameof(VariantId)); } }
        }

        [BsonElement("productname")]
        public string ProductName { get; set; }

        [BsonElement("color")]
        public string Color { get; set; }

        [BsonElement("quantity")]
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    RecalculateAll();
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        [BsonElement("size")]
        public string Size
        {
            get => size;
            set { if (size != value) { size = value; OnPropertyChanged(nameof(Size)); } }
        }

        [BsonElement("baseprice")]
        public decimal BasePrice
        {
            get => basePrice;
            set
            {
                if (basePrice != value)
                {
                    basePrice = value;
                    RecalculateAll();
                    OnPropertyChanged(nameof(BasePrice));
                }
            }
        }

        [BsonElement("discountpercentage")]
        public decimal DiscountPercentage
        {
            get => discountPercentage;
            set
            {
                if (discountPercentage != value)
                {
                    discountPercentage = value;
                    RecalculateAll();
                    OnPropertyChanged(nameof(DiscountPercentage));
                }
            }
        }

        public string ProductImage { get; set; }

        [BsonElement("vatpercentage")]
        public decimal VatPercentage
        {
            get => vatPercentage;
            set
            {
                if (vatPercentage != value)
                {
                    vatPercentage = value;
                    RecalculateAll();
                    OnPropertyChanged(nameof(VatPercentage));
                }
            }
        }

        // 🧮 Calculated Outputs (don't set manually!)
        [BsonElement("discountamount")]
        public decimal DiscountAmount
        {
            get => discountAmount;
            private set { if (discountAmount != value) { discountAmount = value; OnPropertyChanged(nameof(DiscountAmount)); } }
        }

        [BsonElement("lineamount")]
        public decimal LineAmount
        {
            get => lineAmount;
            private set { if (lineAmount != value) { lineAmount = value; OnPropertyChanged(nameof(LineAmount)); } }
        }

        [BsonElement("netamount")]
        public decimal NetAmount
        {
            get => netAmount;
            private set { if (netAmount != value) { netAmount = value; OnPropertyChanged(nameof(NetAmount)); } }
        }

        [BsonElement("vatamount")]
        public decimal VatAmount
        {
            get => vatAmount;
            private set { if (vatAmount != value) { vatAmount = value; OnPropertyChanged(nameof(VatAmount)); } }
        }

        // 🔁 Central calculation
        private void RecalculateAll()
        {
            LineAmount = BasePrice * Quantity;
            DiscountAmount = (BasePrice * DiscountPercentage / 100m) * Quantity;
            NetAmount = LineAmount - DiscountAmount;
            VatAmount = NetAmount * VatPercentage / 100m;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}