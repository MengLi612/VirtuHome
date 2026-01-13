using System;
using System.Collections.Generic;
using Common.Abstract;
using Common.Enum;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ComponentFeature
{
    [Serializable]
    public record LinkUnit
    {
        [field: SerializeField] public string UnitId { get; }
        [field: SerializeField][EnumToggleButtons] public LinkType LinkType { get; set; }
        [field: SerializeField] public GameObject FromObject { get; set; }
        [ShowInInspector]
        public Entity FromEntity
        {
            get
            {
                if (FromObject == null) return null;
                return FromObject.GetComponent<Entity>();
            }
        }
        [ShowInInspector]
        public LinkBehavior FromLinkBehavior
        {
            get
            {
                if (FromObject == null) return null;
                return FromObject.GetComponent<LinkBehavior>();
            }
        }

        [field: SerializeField] public GameObject ToObject { get; set; }
        [ShowInInspector]
        public Entity ToEntity
        {
            get
            {
                if (ToObject == null) return null;
                return ToObject.GetComponent<Entity>();
            }
        }
        [ShowInInspector]
        public LinkBehavior ToLinkBehavior
        {
            get
            {
                if (ToObject == null) return null;
                return ToObject.GetComponent<LinkBehavior>();
            }
        }

        public LinkUnit(LinkType linkType, GameObject from, GameObject to)
        {
            UnitId = Guid.NewGuid().ToString();
            LinkType = linkType;
            FromObject = from;
            ToObject = to;
            Link();
        }
        private void Link()
        {
            FromLinkBehavior.AddLink(this);
            ToLinkBehavior.AddLink(this);
        }
    }

    public partial class LinkBehavior
    {
        /// <summary>
        /// 共享所有的链接，防止重复创建
        /// </summary>
        public static List<string> LinkIds = new();
    }

    public partial class LinkBehavior : AbstractIOTDeviceFeature
    {
        [SerializeField] private LinkStateType _curState;
        public LinkStateType CurState
        {
            get => _curState;
            private set
            {
                _curState = value;
                OnStateChanged?.Invoke(value);
            }
        }

        [field: SerializeField] public string LinkId { get; set; }
        [field: SerializeField] public List<LinkUnit> LinkedUnits { get; } = new();

        public Action<LinkStateType> OnStateChanged;

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(LinkId) && LinkIds.Contains(LinkId))
            {
                for (int i = 1; ; i++)
                {
                    LinkId = LinkId + "_" + i;
                    if (!LinkIds.Contains(LinkId))
                    {
                        break;
                    }
                }
            }
        }
        private void OnDisable()
        {
            DisableLink();
        }

        public void AddLink(GameObject to)
        {
            if (to.TryGetComponent(out Entity _))
            {
                new LinkUnit(LinkType.USB, gameObject, to);
                if (CurState == LinkStateType.Disconnected)
                {
                    CurState = LinkStateType.Connected;
                }
            }
        }
        public void AddLink(LinkUnit unit)
        {
            if (unit.ToEntity != null)
            {
                LinkedUnits.Add(unit);
                if (CurState == LinkStateType.Disconnected)
                {
                    CurState = LinkStateType.Connected;
                }
            }
        }
        public void RemoveLink(GameObject obj)
        {
            LinkedUnits.Remove(LinkedUnits.Find(x => x.ToObject == obj));
            if (LinkedUnits.Count == 0 && CurState == LinkStateType.Connected)
            {
                CurState = LinkStateType.Disconnected;
            }
        }
        public LinkUnit GetLinkUnit(string toLinkId)
        {
            return LinkedUnits.Find(x => x.ToLinkBehavior.LinkId == toLinkId);
        }
        public LinkUnit GetLinkUnit(LinkBehavior linkBehavior)
        {
            return LinkedUnits.Find(x => x.ToLinkBehavior == linkBehavior);
        }
        public void DisableLink()
        {
            foreach (var item in LinkedUnits)
            {
                item.ToLinkBehavior.RemoveLink(gameObject);
            }
            LinkedUnits.Clear();
        }
    }
}