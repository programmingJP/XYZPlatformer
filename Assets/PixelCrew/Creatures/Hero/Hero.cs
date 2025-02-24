using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature
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

        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        
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
        private float _defaultGravityScale;

        private GameSession _session;

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
            var health = GetComponent<HealthComponent>();//получаем компонент здоровья
            
            health.SetHealth(_session.Data.Hp); //записываем текущее здоровье в компонент
            UpdateHeroWeapon();
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
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
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
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

        //if dev комишен компилейшен - позволяет вырезать кусочки кода при компиляции, если в юнити эдиторе, то будет отображаться,а если на какую то платформу, то код вырежеться



        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"Coins: {_session.Data.Coins}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
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
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

        //Производим сами расчеты, в анимационном ивенте на определенном кадре, чтобы нельзя было заспамить
        

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
            Animator.runtimeAnimatorController = _armed;
        }
        
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
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
            _particles.Spawn("Throw");
        }
        public void Throw()
        {
            if (_throwCooldown.IsReady)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }
    }
}
