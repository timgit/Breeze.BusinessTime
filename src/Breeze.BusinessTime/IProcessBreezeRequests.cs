using System;
using System.Collections.Generic;
using Breeze.ContextProvider;

namespace Breeze.BusinessTime
{
    public interface IProcessBreezeRequests
    {
        void Process(Dictionary<Type, List<EntityInfo>> saveMap);
    }
}