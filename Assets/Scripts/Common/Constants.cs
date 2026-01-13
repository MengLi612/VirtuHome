using UnityEngine;

namespace Common
{
    public static class Constants
    {
        public static class EventKeys
        {
            public static class Lifecycle
            {
                public const string Awake = "Lifecycle/Awake";
                public const string Start = "Lifecycle/Start";
                public const string Update = "Lifecycle/Update";
                public const string LateUpdate = "Lifecycle/LateUpdate";
                public const string FixedUpdate = "Lifecycle/FixedUpdate";
                public const string OnEnable = "Lifecycle/OnEnable";
                public const string OnDisable = "Lifecycle/OnDisable";
                public const string OnCollisionEnter = "Lifecycle/OnCollisionEnter";
                public const string OnCollisionExit = "Lifecycle/OnCollisionExit";
                public const string OnCollisionStay = "Lifecycle/OnCollisionStay";
                public const string OnTriggerEnter = "Lifecycle/OnTriggerEnter";
                public const string OnTriggerExit = "Lifecycle/OnTriggerExit";
                public const string OnTriggerStay = "Lifecycle/OnTriggerStay";
                public const string OnDestroy = "Lifecycle/OnDestroy";
                public const string OnApplicationQuit = "Lifecycle/OnApplicationQuit";
            }
            public static class MouseInput
            {
                public const string RightHold = "MouseInput/RightHold";
                public const string XDrag = "MouseInput/XDrag";
                public const string Select = "MouseInput/Select";
                public const string SelectClick = "MouseInput/SelectClick";
            }
            public static class OperatingStateKeys
            {
                public const string Disabled = "OperatingState/Disabled";
                public const string Active = "OperatingState/Active";
                public const string Standby = "OperatingState/Standby";
                public const string Sleeping = "OperatingState/Sleeping";
            }
            public static string GenerateTimeKey(string id, int time)
            {
                return $"EventKeys/{id}/Time/{time}";
            }
            public static string GenerateCallMsgSendKey(string id)
            {
                return $"EventKeys/{id}/CallMsgSend";
            }
        }
        public static class AddressKeys
        {
            public static string[] IOTDeviceAddresses =
            {
                "Assets/GameResources/Prefab/Devices/MCU Devices.prefab",
                "Assets/GameResources/Prefab/Devices/Infrared ranging sensor.prefab",
                "Assets/GameResources/Prefab/Devices/Timer.prefab",
            };
            public static string GenerateName(GameObject param, string id = null)
            {
                if (id != null)
                    return $"{param.name}_{id}";
                else
                    return param.name;
            }
        }
    }
}
