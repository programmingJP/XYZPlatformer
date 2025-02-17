using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] private float _damageVelocity;
        
        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverLap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        
        //протектед позволяет видеть переменную внутри класса и в его наследниках
        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected bool IsGrounded;
        private bool _isJumping;
        
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }
        
        private void FixedUpdate()
        {
            float xVelocity = Direction.x * _speed; //считаем x  координату в зависимости от направления
            float yVelocity = CalculateYVelocity(); //считаем y  координату
            
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetBool(IsRunning, Direction.x != 0); //если направление х не равно нулю, то будет true
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);

            UpdateSpriteDirection(Direction);
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
        
        protected virtual float CalculateYVelocity()
        {
            float yVelocity = Rigidbody.velocity.y; //получаем текущую скорость по y
            
            bool isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing) //если мы нажимаем прыжок, то начинаем расчет прыжка
            {
                _isJumping = true;
                
                bool isFalling = Rigidbody.velocity.y <= 0; 
                yVelocity = isFalling? CalculateJumpVelocity(yVelocity) : yVelocity;
            }

            else if (Rigidbody.velocity.y > 0 && _isJumping) // иначе мы уменьшаем скорость, чтобы у нас регулировалась высота прыжка
            {
                yVelocity *= 0.5f;
            }
            
            return yVelocity;
        }
        
        //1 проход проверяем стоим ли мы на земел и разрешаем простой прыжок, 2 проход проверяем нажали ли мы прыжок и падаем ли мы, исли да, то разрешаем двойной прыжок, 3 проход, мы выдим что мы прыгнули и нажали двойной прыжок, поэтому запрещаем  эти действия
        
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded) //если мы на земле тогда мы просто прыгаем
            {
                yVelocity += _jumpForce;
                _particles.Spawn("Jump");
            }
            
            return yVelocity;
        }
        
        public void UpdateSpriteDirection(Vector2 direction)
        {
            //для инвертирования мы будем домнажать на 1 либо - 1
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier,1,1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Debug.Log("SharkHitNow");
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity); //сила при получении урона, чтобы герой чуть подлетел
        }
        
        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }
        
        private void PerformAttack() //бывший OnAttack, поменял потому что происходит конфликт (OnDoAttack у Алексея)
        {
            _attackRange.Check();
            _particles.Spawn("Slash");
        }
    }
}
