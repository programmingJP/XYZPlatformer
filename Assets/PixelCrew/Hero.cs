using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        
        //[SerializeField] private LayerCheck _groundCheck; для отдельного скрипта чекера
        
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;
        
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        
        //Параметр для всех методов детекта кроме третьего, так как мы его указали в самом компоненте
        [SerializeField] private LayerMask _groundLayer;
        
        //Параметры для третьего метода детекта

        //Параметры для рейкаст сферы
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x  * _speed, _rigidbody.velocity.y);

            bool isJumping = _direction.y > 0;
            bool isGrounded = IsGrounded();

            if (isJumping)
            {
                if (isGrounded && _rigidbody.velocity.y <= 0)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }
            
            _animator.SetBool(IsGroundKey, isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0); //если направление х не равно нулю, то будет true
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        //Метод для обновления направления спрайта
        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
            }
        }

        //Первый способ детекта
        /*private bool IsGrounded() // метод рейкаста одного луча
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _groundLayer);
            
            return hit.collider != null; // если у хита есть коллайдер, значит нашли обьект который лежит на слое граунд леер
        }*/
        
        //Второй способ детекта
        private bool IsGrounded() // метод рейкаста одного луча
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0,_groundLayer);
            
            return hit.collider != null;
        }

        //Третий способ детекта
        
        /*private bool IsGrounded() // метод рейкаста одного луча
        {
            return _groundCheck.IsTouchingLayer;
        }*/

        private void OnDrawGizmos()
        {
            //в дебажных методах мы используем мировые координаты!
            
            //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red); //отрисовка рейкаст луча в сцене
            
            
            //Отрисовка рейкаст сферы
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);
            
            //Отрисовка рейкаст сферы для третьего способа, просто чтобы понять сработало или нет
            /*Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position ,0.3f);*/
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void SaySomething()
        {
            Debug.Log("Hello!");
        }
    }
}
