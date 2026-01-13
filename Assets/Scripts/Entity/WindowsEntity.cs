using System;
using System.Collections.Generic;
using UnityEngine;

namespace ComponentFeature
{
    /// <summary>
    /// 场景中窗户物体的实体类
    /// </summary>
    public class WindowsEntity : IOTEquipmentEntity
    {
        [Serializable]
        public class WindowObject
        {
            public GameObject gameObject;
            public int position;
        }

        public enum MoveType
        {
            Left,
            Right,
        }
        /// <summary>
        /// 窗户移动方向
        /// </summary>
        public Vector3 WindowMoveAxis => transform.right;


        /// <summary>
        /// 窗户物体列表
        /// </summary>
        public List<WindowObject> windowObject = new();
        public GameObject windowEdge;

        public Vector3 GetWindowMoveDir(MoveType moveType)
        {
            return moveType == MoveType.Left ? -WindowMoveAxis : WindowMoveAxis;
        }
    }
}
