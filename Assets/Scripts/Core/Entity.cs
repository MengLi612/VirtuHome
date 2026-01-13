using System.Collections.Generic;
using Common.Abstract;
using Common.Interface;
using UnityEngine;

namespace Core
{
    public class Entity : ComponentBehavior, IGetSelfPartable
    {
        [field: SerializeField] public string Id { get; protected set; }

        [field: SerializeField] public List<AbstractAsyncCommand> RunningCommands { get; } = new();

        private void Start()
        {
            Id ??= gameObject.GetInstanceID().ToString();
        }

        public T GetPart<T>() where T : Component
        {
            return GetComponentInChildren<T>();
        }
    }
}
