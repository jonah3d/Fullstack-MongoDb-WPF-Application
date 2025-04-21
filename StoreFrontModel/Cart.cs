using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Cart : INotifyPropertyChanged
    {
        private decimal subTotal;
        private decimal shippingCost;
        private decimal totalVat;
        private decimal total;

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

        [BsonElement("subtotal")]
        public decimal SubTotal
        {
            get => subTotal;
            set
            {
                if (subTotal != value)
                {
                    subTotal = value;
                    OnPropertyChanged(nameof(SubTotal));
                }
            }
        }

        [BsonElement("shippingcost")]
        public decimal ShippingCost
        {
            get => shippingCost;
            set
            {
                if (shippingCost != value)
                {
                    shippingCost = value;
                    OnPropertyChanged(nameof(ShippingCost));
                    RecalculateTotals(); // optional: update total when shipping changes
                }
            }
        }

        [BsonElement("totalVatAmount")]
        public decimal TotalVat
        {
            get => totalVat;
            set
            {
                if (totalVat != value)
                {
                    totalVat = value;
                    OnPropertyChanged(nameof(TotalVat));
                }
            }
        }

        [BsonElement("Total")]
        public decimal Total
        {
            get => total;
            set
            {
                if (total != value)
                {
                    total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        [BsonIgnore]
        public User User { get; set; }

        [BsonElement("shippingMethod")]
        public string ShippingMethod { get; set; }

        public Cart()
        {
            ShippingCost = 0m; // Set default shipping cost here if needed
            Items.CollectionChanged += Items_CollectionChanged;
        }

        public void AddItem(CartItem newItem)
        {
            var existingItem = Items.FirstOrDefault(item =>
                item.ProductId == newItem.ProductId &&
                item.VariantId == newItem.VariantId &&
                item.Size == newItem.Size &&
                item.Quantity == newItem.Quantity &&
                item.Color == newItem.Color &&
                item.BasePrice == newItem.BasePrice &&
                item.DiscountAmount == newItem.DiscountAmount &&
                item.DiscountPercentage == newItem.DiscountPercentage &&
                item.VatPercentage == newItem.VatPercentage &&
                item.NetAmount == newItem.NetAmount &&
                item.VatAmount == newItem.VatAmount  &&
                item.ProductName == newItem.ProductName &&
                item.ProductImage == newItem.ProductImage


                );

            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                Items.Add(newItem);
            }
        }

        private async void UcCartItem_RemoveItem(object sender, CartItem itemToRemove)
        {
            try
            {
                // Remove from collection
                Items.Remove(itemToRemove);

              
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing item from cart: {ex.Message}");
            }
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CartItem newItem in e.NewItems)
                {
                    newItem.PropertyChanged += CartItem_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (CartItem oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= CartItem_PropertyChanged;
                }
            }

            RecalculateTotals();
        }

        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity) ||
                e.PropertyName == nameof(CartItem.NetAmount) ||
                e.PropertyName == nameof(CartItem.VatAmount) ||
                e.PropertyName == nameof(CartItem.BasePrice) ||
                e.PropertyName == nameof(CartItem.DiscountAmount) ||
                e.PropertyName == nameof(CartItem.DiscountPercentage) ||
                e.PropertyName == nameof(CartItem.VatPercentage))
            {
                RecalculateTotals();
            }
        }

        public void RecalculateTotals()
        {
            SubTotal = Items.Sum(item => item.NetAmount);   // Already includes quantity
            TotalVat = Items.Sum(item => item.VatAmount);   // Already includes quantity
            Total = SubTotal + TotalVat + ShippingCost;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
