using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;

namespace Breeze.BusinessTime.Rules
{
    public class PipelinedDbContextProvider<T>: EFContextProvider<T> where T : DbContext, new()
    {
        private List<IProcessBreezeRequests> _pipeline;

        public List<IProcessBreezeRequests> Pipeline
        {
            get { return _pipeline ?? (_pipeline = new List<IProcessBreezeRequests>()); }
            protected set { _pipeline = value; }
        }

        public PipelinedDbContextProvider()
        {
            BeforeSaveEntitiesDelegate = ProcessPipeline;
        }

        public PipelinedDbContextProvider(IEnumerable<IProcessBreezeRequests> pipeline): this()
        {            
            Pipeline.AddRange(pipeline);            
        }

        public PipelinedDbContextProvider(IProcessBreezeRequests pipe): this()
        {
            Pipeline.Add(pipe);
        }

        protected Dictionary<Type, List<EntityInfo>> ProcessPipeline(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if(Pipeline != null && Pipeline.Any())
                Pipeline.ToList().ForEach(pipe => pipe.Process(saveMap));

            return saveMap;
        }
    }
}