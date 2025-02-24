using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    
    //Вызываем Interact пока что из героя, так как на данный момент взаимодействовать с чем либо может только герой
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}
