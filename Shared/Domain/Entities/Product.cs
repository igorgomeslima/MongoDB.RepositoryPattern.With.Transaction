using System;

namespace Shared.Domain.Entities
{
    public class Product
    {
        //[BsonId]
        public string Id { get; set; }
        //[BsonElement("SKU")]
        public int SKU { get; set; }
        //[BsonElement("Description")]
        public string Description { get; set; }
        //[BsonElement("Price")]
        public Double Price { get; set; }
    }
}
