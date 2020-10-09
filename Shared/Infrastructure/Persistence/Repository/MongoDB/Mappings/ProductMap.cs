using MongoDB.Bson;
using Shared.Domain.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Shared.Infrastructure.Persistence.Repository.MongoDB.Mappings
{
    public class ProductMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Product>(map =>
            {
                map.AutoMap();
                map.MapIdMember(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                map.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }
    }
}
