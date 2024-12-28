using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
    
        private Vector2 _direction;
    
    
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void SaySomething()
        {
            Debug.Log("Hello!");
        }
    
        private void Update()
        {
            if (_direction != Vector2.zero)
            {
                Vector3 delta = new Vector3(_direction.x, _direction.y, 0) * _speed * Time.deltaTime;
                transform.position += delta;
            }
        }
    }
}
