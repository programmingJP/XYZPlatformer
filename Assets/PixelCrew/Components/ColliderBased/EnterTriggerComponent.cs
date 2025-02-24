using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    //Реализация реалоада уровня на компонентной основе
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~ 0; //по дефолту устанавливаем слой в Everything
        [SerializeField] private EnterEvent _action;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;

            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;
            
            _action?.Invoke(other.gameObject); //сокращенная проверка на null _action?.Invoke(); работает для любых методов и классов которые могут принимать значение null

        }
    }
}
