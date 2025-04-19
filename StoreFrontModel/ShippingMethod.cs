using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontModel
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System.ComponentModel;

    public class ShippingMethod : INotifyPropertyChanged
    {
        private string name;
        private decimal basePrice;
        private decimal minimumOrderForFreeShipping;
        private decimal vatPercentage;
        private decimal vatAmount;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [BsonElement("basePrice")]
        public decimal BasePrice
        {
            get => basePrice;
            set
            {
                if (basePrice != value)
                {
                    basePrice = value;
                    OnPropertyChanged(nameof(BasePrice));
                }
            }
        }

        [BsonElement("minOrderForFreeShipping")]
        public decimal MinimumOrderForFreeShipping
        {
            get => minimumOrderForFreeShipping;
            set
            {
                if (minimumOrderForFreeShipping != value)
                {
                    minimumOrderForFreeShipping = value;
                    OnPropertyChanged(nameof(MinimumOrderForFreeShipping));
                }
            }
        }

        [BsonElement("vatPercentage")]
        public decimal VatPercentage
        {
            get => vatPercentage;
            set
            {
                if (vatPercentage != value)
                {
                    vatPercentage = value;
                    OnPropertyChanged(nameof(VatPercentage));
                }
            }
        }

        [BsonIgnore]
        public decimal VatAmount
        {
            get => vatAmount;
            set
            {
                if (vatAmount != value)
                {
                    vatAmount = value;
                    RecalculateVat();
                    OnPropertyChanged(nameof(VatAmount));
                }
            }
        }

        private void RecalculateVat()
        {
            VatAmount = (BasePrice * VatPercentage) / 100;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
