using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        public TabGroup tabGroup;

        public void Start()
        {
            tabGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }
    }
}