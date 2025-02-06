using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    //Реализация реалоада уровня на компонентной основе
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject); //сокращенная проверка на null _action?.Invoke(); работает для любых методов и классов которые могут принимать значение null
            }
        }
    }
}
