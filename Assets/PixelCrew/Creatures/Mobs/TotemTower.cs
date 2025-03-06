using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.Health;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAi> _traps;
        [SerializeField] private Cooldown _cooldown;

        private int _currentTrap;

        private void Start()
        {
            foreach (var shootingTrapAi in _traps)
            {
                shootingTrapAi.enabled = false;

                var hp = shootingTrapAi.GetComponent<HealthComponent>();
                
                
                hp._onDie.AddListener(() => OnTrapDead(shootingTrapAi)); //Замыкание, анонимная функция
            }
        }

        private void OnTrapDead(ShootingTrapAi shootingTrapAi)
        {
            var index = _traps.IndexOf(shootingTrapAi);
            _traps.Remove(shootingTrapAi);

            if (index < _currentTrap)
            {
                _currentTrap--;
            }
        }

        private void Update()
        {
            if (_traps.Count == 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
            //Если любой обьект тачинг леер, то нам вернется тру
            var hasAnyTarget = _traps.Any(x => x._vision.IsTouchingLayer);

            if (hasAnyTarget)
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot();
                    _cooldown.Reset();
                    _currentTrap = (int)Mathf.Repeat(_currentTrap + 1, _traps.Count);
                }
            }
        }
    }
}