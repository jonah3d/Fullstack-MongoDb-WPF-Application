using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreFrontModel
{
    public class Company
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("nif")]
        public string Nif { get; set; }


        [BsonElement("registro mercantil")]
        public string RegistroMercantil { get; set; }

        [BsonElement("address")]
        public List<Address> Addresses { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        public Company()
        {
            Addresses = new List<Address>();
        }
    }
}
