using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private bool _isTouchingLayer;
        private Collider2D _collider;

        public bool IsTouchingLayer => _isTouchingLayer;
    
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
