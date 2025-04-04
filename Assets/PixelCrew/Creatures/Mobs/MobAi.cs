﻿using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Patrolling;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class MobAi : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missCooldown = 0.5f;

        [SerializeField] private float _horizontalTrashold = 0.2f;

        private IEnumerator _current;
        private GameObject _target;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        
        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private bool _isDead;
        private Patrol _patrol;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StarState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return; //поле _isDead завели для того, чтобы если моб умер, он не мог агриться к герою

            _target = go;
            StarState(AgroToHero());
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            
            StarState(GoToHero());
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        private IEnumerator GoToHero()
        {
            //Если у нас в вижен леере герой, мы постепенно двигаемся к нему
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StarState(Attack());
                }
                else
                {
                    var horizontalDelta = Mathf.Abs(_target.transform.position.x - transform.position.x);//используем абсолютное значение, так как оно может быть как слева так и справа
                    if(horizontalDelta <= _horizontalTrashold)
                        _creature.SetDirection(Vector2.zero);
                    else
                        SetDirectionToTarget();
                }
                
                yield return null;//пропускаем один кадр
            }
            
            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("MissHero");
                
            yield return new WaitForSeconds(_missCooldown);
                
            StarState(_patrol.DoPatrol());
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            
            StarState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            //Получаем вектор направления из одной точки в другую
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }
        
        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0; //нулим y так как двигаемся только по горизонтали
            return direction.normalized;////устанавливаем направление / должно быть нормализовано чтобы не было перепадов по скорости(еденичный вектор)
        }

        private void StarState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);//останавливаем движение каждый стейт
            //если текущая корутина не равна нулю, мы ее останавливаем
            //чтобы не возникло коллизии между двумя разными корутинами
            //так как моб должен делать только одну вещ за раз
            if (_current != null) 
                StopCoroutine(_current);

            _current = coroutine;
            StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            
            _creature.SetDirection(Vector2.zero);
            if (_current != null) 
                StopCoroutine(_current);
        }
    }
}
