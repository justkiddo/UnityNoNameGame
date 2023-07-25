using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace root
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        private TabGroup _tabGroup;
        public Image background;


        private void Start()
        {
            background = GetComponent<Image>();
            _tabGroup.Subscribe(this);
        
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tabGroup.OnTabEnter(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _tabGroup.OnTabSelected(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tabGroup.OnTabExit(this);
        }
    }
}