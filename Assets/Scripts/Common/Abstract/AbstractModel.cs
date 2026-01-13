using Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Common.Abstract
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class AbstractUIModel : ComponentBehavior
    {
        [SerializeField]
        private UIDocument UIDocument;

        protected virtual void Start()
        {
            UIDocument.rootVisualElement.dataSource = this;
        }
        protected virtual void OnValidate()
        {
            UIDocument = GetComponent<UIDocument>();
        }
    }
}
