using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Common.Constants;

namespace ComponentFeature
{
    public class IOTEquipmentEntity : Entity
    {
        [SerializeField]
        private MCUDevice mcuDevice;
        [SerializeField] private List<IOTDeviceEntity> iotDevices = new();
        [SerializeField] private GameObject iotDeviceFolder;
        /// <summary>
        /// 在自己的层级下创建子物体作为文件夹存放 IOT 设备
        /// </summary>
        public void CreateIOTDeviceFolder()
        {
            // 如果存在IOT Devices名称的子物体并且 iotDeviceFolder 为 null 则赋值
            if (transform.Find("IOT Devices") != null && iotDeviceFolder == null)
                iotDeviceFolder = transform.Find("IOT Devices").gameObject;
            if (iotDeviceFolder != null) return;
            iotDeviceFolder = new("IOT Devices")
            {
                // 更改标签为 Folder
                tag = "Folder"
            };
            iotDeviceFolder.transform.SetParent(transform);
            iotDeviceFolder.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 项目使用 Addressables 包，所以需要通过 Addressables 来获取预制体
        /// </summary>
        /// <param name="mcuAddress"></param>
        public async void AddMCU([ValueDropdown("GetIOTDeviceAddresses")] string mcuAddress)
        {
            if (iotDeviceFolder == null) return;
            GameObject mcuPrefab = await Addressables.LoadAssetAsync<GameObject>(mcuAddress).Task;
            var mcu = Instantiate(mcuPrefab, iotDeviceFolder.transform);

            mcu.transform.localPosition = Vector3.zero;
            mcu.name = AddressKeys.GenerateName(gameObject, mcuPrefab.name);

            mcuDevice = mcu.GetComponent<MCUDevice>();
            iotDevices.Add(mcu.GetComponent<MCUDevice>());
        }
        public async void AddIOTDevice([ValueDropdown("GetIOTDeviceAddresses")] string iotDeviceAddress, GameObject mcuObject = null)
        {
            if (iotDeviceFolder == null || (mcuObject == null && mcuDevice == null)) return;
            if (mcuObject == null) mcuObject = mcuDevice.gameObject;
            GameObject iotDevicePrefab = await Addressables.LoadAssetAsync<GameObject>(iotDeviceAddress).Task;
            var device = Instantiate(iotDevicePrefab, iotDeviceFolder.transform);

            device.transform.localPosition = Vector3.zero;
            device.name = AddressKeys.GenerateName(gameObject, iotDevicePrefab.name);

            iotDevices.Add(device.GetComponent<IOTDeviceEntity>());
            mcuObject.GetComponent<LinkBehavior>().AddLink(device);
        }

        public string[] GetIOTDeviceAddresses()
        {
            return AddressKeys.IOTDeviceAddresses;
        }
    }
}


