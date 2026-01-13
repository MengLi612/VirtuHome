using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Core
{
#if ODIN_INSPECTOR
    public class ComponentBehavior : SerializedMonoBehaviour
#else
    public class ComponentBehavior : MonoBehaviour
#endif
    {
        private string selfId;
        protected string SelfId
        {
            get
            {
                if (string.IsNullOrEmpty(selfId))
                {
                    selfId = Self.Id;
                }
                return selfId;
            }
        }
        public virtual Entity Self => GetComponentInParent<Entity>();
        private readonly Dictionary<string, string> stringData = new();

        public string GetString(string key)
        {
            if (stringData.ContainsKey(key))
            {
                return stringData[key];
            }
            return string.Empty;
        }
        public void SetString(string key, string value)
        {
            stringData[key] = value;
        }
        public void RemoveString(string key)
        {
            stringData.Remove(key);
        }
        public void ClearString()
        {
            stringData.Clear();
        }
        public string SetAndGetString(string key, string value)
        {
            stringData[key] = value;
            return stringData[key];
        }
        public string GetAndRemoveString(string key)
        {
            if (stringData.ContainsKey(key))
            {
                string value = stringData[key];
                stringData.Remove(key);
                return value;
            }
            return string.Empty;
        }
    }
}
