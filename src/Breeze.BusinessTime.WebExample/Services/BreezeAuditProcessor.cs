using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Breeze.ContextProvider;

namespace Breeze.BusinessTime.WebExample.Services
{
    public class BreezeAuditProcessor: IProcessBreezeRequests
    {
        private readonly IPrincipal _user;

        public BreezeAuditProcessor(IPrincipal user)
        {
            _user = user;
        }

        public void Process(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            saveMap.ToList().ForEach(item => 
                item.Value.ForEach(entityInfo =>
                {
                    var userId = _user.Identity.Name;
                    var action = entityInfo.EntityState.ToString();
                    var entity = item.Key.Name;
                    var key = (int) entityInfo.Entity.GetType().GetProperty("Id").GetValue(entityInfo.Entity);
                    //BackgroundJob.Enqueue(() => logger.Log(userId, entity, key, action));
                }));
        }
    }
}