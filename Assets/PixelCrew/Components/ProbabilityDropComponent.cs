using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class ProbabilityDropComponent : MonoBehaviour
    {
        [SerializeField] private int _count; //количество выпадающих элементов
        [SerializeField] private DropData[] _drop; //ссылки на префабы
        [SerializeField] private DropEvent _onDropCalculated;
        [SerializeField] private bool _spawnOnEnable;

        private void OnEnable()
        {
            if (_spawnOnEnable)
            {
                CalculateDrop();
            }
        }

        [ContextMenu("CalculateDrop")]
        public void CalculateDrop()
        {
            var itemsToDrop = new GameObject[_count];
            
            //Нужно пройтись в цикле и сделать вероятность выпадения
            var itemCount = 0;

            var total = _drop.Sum(dropData => dropData.Probability); //сумма вероятностей (общая вероятность)
            var sortedDrop = _drop.OrderBy(dropData => dropData.Probability); //сортируем по вероятности

            while (itemCount < _count)
            {
                var random = UnityEngine.Random.value * total;
                var current = 0f;
                
                //Если наше число совпадает с рандомом, то мы добавляем элемент в массив
                foreach (var dropData in sortedDrop)
                {
                    current += dropData.Probability;
                    if (current >= random)
                    {
                        itemsToDrop[itemCount] = dropData.Drop;
                        itemCount++;
                        break;
                    }
                }
            }
            
            _onDropCalculated.Invoke(itemsToDrop); //вызываем событие и передаем туда массив элементов, которые нам нужно выкинуть
        }


        [Serializable]
        public class DropData
        {
            public GameObject Drop; //ссылка на префаб
            [Range(0, 100f)]
            public float Probability; //вероятность выпадения
        }

        public void SetCount(int count)
        {
            _count = count;
        }
    }
    
    [Serializable]
    public class DropEvent : UnityEvent<GameObject[]> //будем передавать все, что нам нужно заспавнить
    {
    }
}
