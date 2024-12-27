using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
    
        private float _direction;
    
    
        public void SetDirection(float direction)
        {
            _direction = direction;
        }

        public void SaySomething()
        {
            Debug.Log("Hello!");
        }
    
        private void Update()
        {
            if (_direction != 0)
            {
                float delta = _direction * _speed * Time.deltaTime;
                float newXPosition = transform.position.x + delta;
                transform.position = new Vector3(newXPosition,transform.position.y, transform.position.z);
            }
        }
    }
}
