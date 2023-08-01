using UnityEngine;
using UnityEngine.EventSystems;

namespace root
{
    public class PlayerBlockEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static bool ButtonPressed;

        public void OnPointerDown(PointerEventData eventData){
                ButtonPressed = true;
            }
 
            public void OnPointerUp(PointerEventData eventData){
                ButtonPressed = false;
            }
        
    
    }
}