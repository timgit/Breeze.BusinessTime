using Breeze.ContextProvider;

namespace Breeze.BusinessTime.Tests.Helpers
{
    public class FakeEntityInfo:EntityInfo
    {
        public FakeEntityInfo():this(new object()) {}

        public FakeEntityInfo(object entity)
        {
            Entity = entity;
        }

        public new object Entity { get; set; }
    }
}