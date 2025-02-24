using System;
using System.Linq;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class CheckCircleOverLap : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlapEvent;
        private readonly Collider2D[] _interactionResult = new Collider2D[10];
        
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            //Получаем и записываем в массив обьекты, которые попали к нам в радиус (максимум до 5 штук)
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position, 
                _radius, 
                _interactionResult,
                _mask);


            for (int i = 0; i < size; i++)
            {
                /*
                 * Если один из элементов с которым мы пересеклись,
                 * будет иметь тэг из списка _tags, то вызываем событие
                 */
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTags)
                { 
                    _onOverlapEvent?.Invoke(overlapResult.gameObject);
                }
            }
        }
        
        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
            
        }
    }
}