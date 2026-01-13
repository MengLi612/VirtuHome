using System;
using System.Collections.Generic;
using System.Linq;
using Common.Abstract;
using Common.Concrete.Unit;
using Common.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ComponentFeature
{
    [Serializable]
    public class EnterRequestUnit
    {
        [field: SerializeField] public string UnitId { get; set; }
        [field: SerializeField] public string DeviceLinkId { get; }
        [field: SerializeField] public object Parameter { get; }

        public RequestType RequestType { get; }
        public Type ParamType => Parameter.GetType();

        public EnterRequestUnit(string deviceLinkId, object parameter, RequestType requestType)
        {
            DeviceLinkId = deviceLinkId;
            Parameter = parameter;
            RequestType = requestType;
        }
    }

    public class RequestEnter : AbstractIOTDeviceFeature
    {
        [DictionaryDrawerSettings(KeyLabel = "Key", ValueLabel = "Unit")]

        // 字典内部的字典，当为key的请求被满足并触发时，会触发value的请求
        public Dictionary<string, EnterRequestUnit> requestDic = new();
        public Dictionary<string, List<string>> requestLink = new();

        public event Action<EnterRequestUnit> EnterRequestEvent;
        public event Action<EnterRequestUnit> BeforeCancelRequestEvent;

        public IEnumerable<Type> GetFilteredTypeList()
        {
            return new List<Type>
            {
                typeof(TimePoint),
                typeof(IOTDeviceState),
            };
        }

        /// <summary>
        /// 输入请求，并触发事件，其中随机生成一个字典key，并返回
        /// </summary>
        /// <param name="linkId"></param>
        /// <param name="enterRequest"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [Button]
        public string EnterRequest([ValueDropdown("GetIOTDeviceLinkIds")] string linkId, [TypeFilter("GetFilteredTypeList")] object enterRequest, [EnumToggleButtons] RequestType requestType)
        {
            if (linkId == null) return null;
            string key = Guid.NewGuid().ToString();
            var requestUnit = new EnterRequestUnit(linkId, enterRequest, requestType) { UnitId = key };
            requestDic.Add(key, requestUnit);
            EnterRequestEvent?.Invoke(requestUnit);
            return key;
        }
        /// <summary>
        /// 取消请求，并触发事件
        /// </summary>
        /// <param name="key"></param>
        [Button]
        public void CancelRequest(string key)
        {
            if (requestDic.ContainsKey(key))
            {
                BeforeCancelRequestEvent?.Invoke(requestDic[key]);
                requestDic.Remove(key);
            }
        }
        [Button]
        public void LinkUnit(string key1, string key2)
        {
            if (requestDic.ContainsKey(key1) && requestDic.ContainsKey(key2) && requestDic[key1].RequestType == RequestType.Positive && requestDic[key2].RequestType == RequestType.Negative)
            {
                if (!requestLink.ContainsKey(key1))
                {
                    requestLink.Add(key1, new List<string>());
                }
                requestLink[key1].Add(key2);
            }
        }
        public EnterRequestUnit[] GetNegativeUnits(object record)
        {
            foreach (string rKey in requestLink.Keys)
            {
                if (requestDic[rKey].Parameter.Equals(record))
                {
                    List<string> nrKey = requestLink[rKey];
                    if (nrKey.Count == 0) continue;
                    return nrKey.Select(x => requestDic[x]).ToArray();
                }
            }
            return null;
        }
        private IEnumerable<string> GetIOTDeviceLinkIds()
        {
            if (!gameObject.TryGetComponent(out LinkBehavior linkBehavior)) return null;
            // 获取所有设备的id
            return linkBehavior.LinkedUnits.Select(x => x.ToLinkBehavior.LinkId);
        }
    }
}