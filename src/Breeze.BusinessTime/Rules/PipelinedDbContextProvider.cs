﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;

namespace Breeze.BusinessTime.Rules
{
    public class PipelinedDbContextProvider<T>: EFContextProvider<T> where T : DbContext, new()
    {
        private List<IProcessBreezeRequests> _beforePipeline;
        private List<IProcessBreezeRequests> _afterPipeline;

        public List<IProcessBreezeRequests> BeforePipeline
        {
            get { return _beforePipeline ?? (_beforePipeline = new List<IProcessBreezeRequests>()); }
            protected set { _beforePipeline = value; }
        }

        public List<IProcessBreezeRequests> AfterPipeline
        {
            get { return _afterPipeline ?? (_afterPipeline = new List<IProcessBreezeRequests>()); }
            protected set { _afterPipeline = value; }
        }

        public PipelinedDbContextProvider()
        {
            BeforeSaveEntitiesDelegate = ExecuteBeforePipeline;
            AfterSaveEntitiesDelegate = ExecuteAfterPipeline;
        }

        private void ExecuteAfterPipeline(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        {
            UpdateKeysForNewEntities(saveMap, keyMappings);
            ExecutePipeline(AfterPipeline, saveMap);
        }

        private void UpdateKeysForNewEntities(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        {
            saveMap.ToList().ForEach(entityType => entityType.Value
                .Where(info => info.EntityState == ContextProvider.EntityState.Added)
                .ToList()
                .ForEach(info =>
                {
                    // key mappings most likely are a concern if the key is auto-generated on the server
                    if (info.AutoGeneratedKey.AutoGeneratedKeyType == AutoGeneratedKeyType.None) return;

                    var keyProp = entityType.Key.GetProperty(info.AutoGeneratedKey.PropertyName);

                    var key = keyMappings.SingleOrDefault(map => 
                        map.TempValue.ToString() == keyProp.GetValue(info.Entity).ToString());
                        
                    if(key != null)
                        keyProp.SetValue(info.Entity, key.RealValue);
                }
            ));            
        }

        public PipelinedDbContextProvider(IProcessBreezeRequests processBefore, IProcessBreezeRequests processAfter)
            : this(processBefore)
        {            
            if (processAfter == null)
                throw new ArgumentException("Argument processAfter (IProcessBreezeRequests) was null.");

            BeforePipeline.Add(processBefore);
        }

        public PipelinedDbContextProvider(IProcessBreezeRequests processBefore)
            : this()
        {
            if(processBefore == null)
                throw new ArgumentException("Argument processBefore (IProcessBreezeRequests) was null.");

            BeforePipeline.Add(processBefore);
        }

        protected Dictionary<Type, List<EntityInfo>> ExecuteBeforePipeline(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            ExecutePipeline(BeforePipeline, saveMap);
            return saveMap;
        }

        private void ExecutePipeline(List<IProcessBreezeRequests> pipeline, Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if (pipeline != null && pipeline.Any())
                pipeline.ForEach(pipe => pipe.Process(saveMap));
        }
    }
}