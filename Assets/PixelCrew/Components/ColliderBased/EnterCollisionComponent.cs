using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action; //Вызываем экшен с геймобжектом, Action который может пересылать геймобджект

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject); //Передаем геймобджект с которым произошло столкновение (с которым заколайделись)
            }
        }
    }
    //По умолчанию Unity не может сериализовать ивент с геймобджектом (дженерик классы в таком виде с аргументом класса в качестве типа), поэтому мы создаем отдельный класс и наследуемся от ивента
    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {
            
    }
}
