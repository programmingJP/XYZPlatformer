using System.Collections;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private int _damage;
        
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damageOnGroundSlam;

        private HealthComponent _slamDamageModify;
        
        //[SerializeField] private LayerCheck _groundCheck; для отдельного скрипта чекера
        
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;
        
        //Переводим строки в хэш и записываем в "константы" переменные
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        
        //Параметр для всех методов детекта кроме третьего, так как мы его указали в самом компоненте
        [SerializeField] private LayerMask _groundLayer;

        //Параметры для рейкаст сферы
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [SerializeField] private CheckCircleOverLap _attackRange;

        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepsParticles;
        [SerializeField] private SpawnComponent _jumpParticles;
        [SerializeField] private SpawnComponent _slamDownParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        
        //Параметры для интеракта
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        private Collider2D[] _interactionResult = new Collider2D[1]; //массив коллайдеров c инециализацией одного значения в массиве

        //TODO DASH
        [SerializeField] private float _dashDuration;
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashCooldown;
        
        private bool _isDashing;
        private bool _canDash = true;

        private GameSession _session;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _slamDamageModify = GetComponent<HealthComponent>();
        }

        //Метод для обновления данных сессии по ХП игрока т.е сохраняем текущее здоровье игрока в сессию

        private void Start() //вызываем в старте так как гейм сессион у нас забирается на эвейке
        {
            _session = FindObjectOfType<GameSession>(); // записываем игровую сессию в переменную
            var health = GetComponent<HealthComponent>();//получаем компонент здоровья
            
            health.SetHealth(_session.Data.Hp); //записываем текущее здоровье в компонент
            UpdateHeroWeapon();
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
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

            if (_isGrounded)
            {
                _allowDoubleJump = true; //если мы стоим на земле, то разрешаем двойной прыжок
                _isJumping = false;
            }

            if (isJumpPressing) //если мы нажимаем прыжок, то начинаем расчет прыжка
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping) // иначе мы уменьшаем скорость, чтобы у нас регулировалась высота прыжка
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
                _jumpParticles.Spawn();
            }
            else if (_allowDoubleJump) //если нам доступен двойной пыжок, то совершаем двойной прыжок и запрещаем его повторное использование
            {
                yVelocity = _jumpForce;
                _jumpParticles.Spawn();
                _allowDoubleJump = false; //сбрасываем флаг
            }
            
            return yVelocity;
        }

        //Метод для обновления направления спрайта
        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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

        //if dev комишен компилейшен - позволяет вырезать кусочки кода при компиляции, если в юнити эдиторе, то будет отображаться,а если на какую то платформу, то код вырежеться
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //в дебажных методах мы используем мировые координаты!
            
            //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red); //отрисовка рейкаст луча в сцене
            
            
            //Отрисовка рейкаст сферы
            
            //Старый способ отрисовки
            //Gizmos.color = IsGrounded() ? Color.green : Color.red;
            //Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);

            //Новый способ
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
            
            //Отрисовка рейкаст сферы для третьего способа, просто чтобы понять сработало или нет
            /*Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position ,0.3f);*/
        }
#endif

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
            _session.Data.Coins += coins;
            Debug.Log($"Coins: {_session.Data.Coins}");
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce); //сила при получении урона, чтобы герой чуть подлетел

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5); //передаем количество коинов которое есть и максимальное количество которое можем выкинуть
            _session.Data.Coins -= numCoinsToDispose; //обновляем количество коинов, которое остается после хита

            var burst = _hitParticles.emission.GetBurst(0); //получаем бурст из эмиссии
            burst.count = numCoinsToDispose; //передаем количество коинов которое надо выкинуть
            _hitParticles.emission.SetBurst(0, burst); //ставим обратно наш бюрст в нужный индекс
            
            _hitParticles.gameObject.SetActive(true); //включаем эффект
            _hitParticles.Play(); //проигрываем эффект
        }

        public void Interact()
        {
            // OverlapCircleNonAlloc позволяет получить пересекающиеся обьекты, но он будет выделять лишней памяти
            var size = Physics2D.OverlapCircleNonAlloc(  //создает сферу
                transform.position,  // позиция сферы
                _interactionRadius, // радиус сферы
                _interactionResult, //записывает результаты в этот массив и возращает нам размер этого массива
                _interactionLayer);
            //если не один из элементов не вернется по подходящим условиям то массив будет 0
            for (int i = 0; i < size; i++)
            {
               var interactable =_interactionResult[i].GetComponent<InteractableComponent>();
               if (interactable != null)
               {
                   interactable.Interact(); //Переходим в InteractableComponent и вызываем экшен
               }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //если мы столкнулись с землей (проверяем слой)
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity) //.relativeVelocity - скорость относительно двух коллайдеров (т.е когда они столкнулись, мы можем понять с какой скоростью они столкнулись и понять какие у них были относительно друг другу скорости)
                {
                    _slamDownParticles.Spawn();
                }

                if (contact.relativeVelocity.y >= _damageVelocity)
                {
                    _slamDamageModify.ModifyHealth(- _damageOnGroundSlam); //на эвейке мы закэшировали компонент, чтобы постоянно не дергать его
                }
            }
        }

        public void SpawnFootDust()
        {
            _footStepsParticles.Spawn();
        }
        
        //тут мы просто запускаем анимацию
        public void Attack()
        {
            if (!_session.Data.IsArmed) return;
            
            _animator.SetTrigger(AttackKey);
        }

        //Производим сами расчеты, в анимационном ивенте на определенном кадре, чтобы нельзя было заспамить
        private void PerformAttack() //бывший OnAttack, поменял потому что происходит конфликт (OnDoAttack у Алексея)
        {
            var gos = _attackRange.GetObjectsInRange();

            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();

                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ModifyHealth( - _damage);
                }
            }
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
            _animator.runtimeAnimatorController = _armed;
        }
        
        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }

        public void Dash()
        {
            if (!_canDash || _isDashing) return;
            
            if (!_isGrounded) StartCoroutine(PerformDash());

        }

        private IEnumerator PerformDash()
        {
            _canDash = false;
            _isDashing = true;

            float originalSpeed = _speed;
            _speed = _dashForce;

            yield return new WaitForSeconds(_dashDuration);

            _speed = originalSpeed; 
            _isDashing = false;

            yield return new WaitForSeconds(_dashCooldown); 
            _canDash = true;
        }
    }
}
