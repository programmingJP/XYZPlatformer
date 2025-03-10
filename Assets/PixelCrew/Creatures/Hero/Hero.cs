using System.Collections;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature, ICanAddInInventory
    {
        [SerializeField] private CheckCircleOverLap _interactionCheck;
        [SerializeField] private LayerCheck _wallCheck;
        
        [SerializeField] private float _slamDownVelocity;
        //[SerializeField] private float _interactionRadius;


        [SerializeField] private int _damageOnGroundSlam;

        private HealthComponent _slamDamageModify;
        
        private bool _allowDoubleJump;

        
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [Header("Super throw")]
        [SerializeField] private Cooldown _superThrowCooldown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;

        [Space] [Header("Particles")] 
        [SerializeField] private ProbabilityDropComponent _hitDrop;
        //[SerializeField] private ParticleSystem _hitParticles;
        
        
        
        //TODO DASH
        [SerializeField] private float _dashDuration;
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashCooldown;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = UnityEngine.Animator.StringToHash("is-on-wall");
        
        //Cервисные переменные
        private bool _isDashing;
        private bool _canDash = true;

        private bool _isOnWall;
        private bool _superThrow;
        private float _defaultGravityScale;

        private GameSession _session;
        private HealthComponent _health;
        
        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coin");

        protected override void Awake()
        {
            /*
             * с помощью ключевого слова base вызываем метод Awake у родительского класса
             * а потом дописываем и вызываем код непосредственно в классе наследнике
             */
            base.Awake();
            _slamDamageModify = GetComponent<HealthComponent>();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        //Метод для обновления данных сессии по ХП игрока т.е сохраняем текущее здоровье игрока в сессию

        private void Start() //вызываем в старте так как гейм сессион у нас забирается на эвейке
        {
            _session = FindObjectOfType<GameSession>(); // записываем игровую сессию в переменную
            _health = GetComponent<HealthComponent>();//получаем компонент здоровья

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            
            _health.SetHealth(_session.Data.Hp.Value); //записываем текущее здоровье в компонент
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        

        protected override float CalculateYVelocity()
        {
            float yVelocity = Rigidbody.velocity.y; //получаем текущую скорость по y
            
            bool isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true; //если мы стоим на земле, то разрешаем двойной прыжок
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }
        
        
        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded &&_allowDoubleJump && !_isOnWall) //!_isOnWall запрещает прыгать когда мы на стене
            {
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpForce;
            }
            
            return base.CalculateJumpVelocity(yVelocity);
        }


        protected override void Update()
        {
            base.Update();
            
            //Будем висеть на стене только в том случае, если будем в сторону нее двигаться
            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection) //проверяем столкнулись ли мы с стеной
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0; //если мы столкнулись с стеной, то мы не можем падать //выключили гравитацию
            }

            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale; //включили гравитацию
            }
            
            Animator.SetBool(IsOnWall, _isOnWall);
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5); //передаем количество коинов которое есть и максимальное количество которое можем выкинуть
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //если мы столкнулись с землей (проверяем слой)
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity) //.relativeVelocity - скорость относительно двух коллайдеров (т.е когда они столкнулись, мы можем понять с какой скоростью они столкнулись и понять какие у них были относительно друг другу скорости)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        //тут мы просто запускаем анимацию
        public override void Attack()
        {
            if (SwordCount <= 0) return;

            base.Attack();
        }
        
        
        private void UpdateHeroWeapon()
        {
            var numSwords = _session.Data.Inventory.Count("Sword");
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }

        public void Dash()
        {
            if (!_canDash || _isDashing) return;
            
            if (!IsGrounded) StartCoroutine(PerformDash());

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

        public void OnDoThrow()
        {
            if (_superThrow)
            {
                var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            _particles.Spawn("Throw");
            _session.Data.Inventory.Remove("Sword", 1);
        }

        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || SwordCount <= 1) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;
            
            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        public void UsePotion()
        {
            var potionCount = _session.Data.Inventory.Count("HealthPotion"); //забираем текущие данные

            if (potionCount > 0)
            {
                _health.ModifyHealth(3);
                _session.Data.Inventory.Remove("HealthPotion", 1);
            }
        }
    }
}
