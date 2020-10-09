using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Shared.Infrastructure.Persistence.Repository.MongoDB.Conventions
{
    public class LowerCaseElementNameConvention : IMemberMapConvention
    {
        public string Name { get; private set; }

        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetElementName(memberMap.MemberName.ToLower());
        }
    }
}
