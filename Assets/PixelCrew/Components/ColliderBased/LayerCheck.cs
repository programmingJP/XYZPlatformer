using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layer;
        [SerializeField] protected bool _isTouchingLayer;
        
        public bool IsTouchingLayer => _isTouchingLayer;
       
        //TODO Удалить потом
        private Collider2D _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            //Проверяем соприкасается ли коллайдер с указанными слоями
            _isTouchingLayer = _collider.IsTouchingLayers(_layer); // проверяем если мы стоим на каком то слое, то мы можем проверить что наш коллайдер соприкасается с указанными нами слоями.
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //Проверяем соприкасается ли коллайдер с указанными слоями
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }
    }
}
