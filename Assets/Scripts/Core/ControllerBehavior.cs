using System;
using UnityEngine;

namespace Core
{
    public class ControllerBehavior : ComponentBehavior
    {
        private string controllerId;
        private GameObject controllers;
        public string ControllerId => controllerId ??= Guid.NewGuid().ToString();
        protected EventBus EventPart => EventBus.Instance;

        public GameObject Controllers
        {
            get
            {
                if (controllers == null)
                {
                    controllers = GameObject.Find("Controllers");
                }
                return controllers;
            }
        }
        public T GetController<T>() where T : ComponentBehavior
        {
            return Controllers.GetComponent<T>();
        }
    }
}
