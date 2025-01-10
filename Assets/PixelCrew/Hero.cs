using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _damageJumpForce;
        
        //[SerializeField] private LayerCheck _groundCheck; для отдельного скрипта чекера
        
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        
        //Переводим строки в хэш и записываем в "константы" переменные
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        
        //Параметр для всех методов детекта кроме третьего, так как мы его указали в самом компоненте
        [SerializeField] private LayerMask _groundLayer;
        
        //Параметры для третьего метода детекта

        //Параметры для рейкаст сферы
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        private int _coins;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            float xVelocity = _direction.x * _speed; //считаем x  координату в зависимости от направления
            float yVelocity = CalculateYVelocity(); //считаем y  координату
            
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            
            
            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0); //если направление х не равно нулю, то будет true
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            float yVelocity = _rigidbody.velocity.y; //получаем текущую скорость по y
            
            bool isJumpPressing = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true; //если мы стоим на земле, то разрешаем двойной прыжок

            if (isJumpPressing) //если мы нажимаем прыжок, то начинаем расчет прыжка
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0) // иначе мы уменьшаем скорость, чтобы у нас регулировалась высота прыжка
            {
                yVelocity *= 0.5f;
            }
            
            return yVelocity;
        }
        
        //1 проход проверяем стоим ли мы на земел и разрешаем простой прыжок, 2 проход проверяем нажали ли мы прыжок и падаем ли мы, исли да, то разрешаем двойной прыжок, 3 проход, мы выдим что мы прыгнули и нажали двойной прыжок, поэтому запрещаем  эти действия
        
        private float CalculateJumpVelocity(float yVelocity)
        {
            bool isFalling = _rigidbody.velocity.y <= 0; // проверка на то, что мы падаем 0.001f
            if(!isFalling) return yVelocity;  //Если мы не падаем, то возращаем дефолтное значение

            if (_isGrounded) //если мы на земле тогда мы просто прыгаем
            {
                yVelocity += _jumpForce;
            }
            else if (_allowDoubleJump) //если нам доступен двойной пыжок, то совершаем двойной прыжок и запрещаем его повторное использование
            {
                yVelocity = _jumpForce;
                _allowDoubleJump = false; //сбрасываем флаг
            }
            
            return yVelocity;
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


        private void Update()
        {
            _isGrounded = IsGrounded();
        }

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

        public void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log($"Coins: {_coins}");
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce); //сила при получении урона, чтобы герой чуть подлетел
        }
    }
}
