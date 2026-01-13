using System;
using System.Collections.Generic;
using Common.Interface;

namespace Core
{
    public abstract class AbstractEventUnit
    {
        public abstract void AddListener(Delegate action);
        public abstract void RemoveListener(Delegate action);
        public abstract void RemoveAllListener();
        public abstract void Trigger(Entity entity, object args);
    }
    public class EventUnit<TArgs> : AbstractEventUnit
    {
        private event Action<Entity, TArgs> Action;

        public override void AddListener(Delegate action)
        {
            Action += (Action<Entity, TArgs>)action;
        }
        public override void RemoveListener(Delegate action)
        {
            Action -= (Action<Entity, TArgs>)action;
        }
        public override void RemoveAllListener()
        {
            Action = null;
        }
        public override void Trigger(Entity entity, object args)
        {
            Action?.Invoke(entity, (TArgs)args);
        }
    }

    public partial class EventBus : ISingletonable<EventBus>
    {
        private static EventBus instance;
        public static EventBus Instance
        {
            get
            {
                instance ??= new EventBus();
                return instance;
            }
        }
    }
    public partial class EventBus
    {
        private readonly Dictionary<string, AbstractEventUnit> eventUnitDic = new();

        public void Register<TArgs>(string key, Action<Entity, TArgs> action)
        {
            if (!eventUnitDic.ContainsKey(key))
            {
                eventUnitDic.Add(key, new EventUnit<TArgs>());
            }
            eventUnitDic[key].AddListener(action);
        }
        public void UnRegister<TArgs>(string key, Action<Entity, TArgs> action)
        {
            if (!eventUnitDic.ContainsKey(key))
            {
                return;
            }
            eventUnitDic[key].RemoveListener(action);
        }
        public void Trigger(string key, Entity sender, object args)
        {
            if (!eventUnitDic.ContainsKey(key))
            {
                return;
            }
            eventUnitDic[key].Trigger(sender, args);
        }
    }
}

