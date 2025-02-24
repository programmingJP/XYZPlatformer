using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
            
            var force = new Vector2(Direction * _speed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse); //реализация если делать чтобы меч падал
        }

        //в фиксед апдейте реализуем передвижение
        /*private void FixedUpdate()
        {
            var position = _rigidbody.position; //получаем текущую позицию риджидбади

            //В одном случае мы будем прибавлять положительное значение, а вдругом случае отрицательное
            position.x += _direction * _speed; //прибавляем к позиции скорость

            _rigidbody.MovePosition(position);//передаем новую позицию в риджидбади
            
            //Unity рекомендует двигает обьекты с риджидбади через риджидбади, а не трансформ, чтобы не было проблем с физикой, синхронизацией трансформа и физического обьекта
        }*/
    }
}
