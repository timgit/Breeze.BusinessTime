using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Breeze.BusinessTime.Rules;
using Breeze.BusinessTime.WebExample.Models;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;

namespace Breeze.BusinessTime.WebExample.Services
{
    public class PreferredDealerProtector:IProcessBreezeRequests
    {
        private readonly IPrincipal _user;

        public PreferredDealerProtector(IPrincipal user)
        {
            _user = user;
        }

        public void Process(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if (!_user.IsInRole("Dealer")) return;

            saveMap.ToList().ForEach(item =>
            {
                var errors = item.Value
                    .Where(entityInfo =>
                        entityInfo.Entity is Dealer
                        &&
                        entityInfo.OriginalValuesMap.ContainsKey("Preferred")
                        &&
                        ((Dealer)entityInfo.Entity).Preferred != (bool)entityInfo.OriginalValuesMap["Preferred"]
                    )
                    .Select(entityInfo =>
                        new EFEntityError(entityInfo, "Unauthorized", "You are not authorized to make this change.",
                            "Preferred")
                    )
                    .ToList();

                if (errors.Any())
                    throw new EntityErrorsException(errors);
            });
        }
    }
}