using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;

namespace Breeze.BusinessTime
{
    public interface IAmABreezeController
    {
        string Metadata();
        SaveResult SaveChanges(JObject saveBundle);
    }
}