using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PointPatrol : Patrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _treshold; //при какой разници между векторами мы будем считать что приблизились достаточно
        
        private Creature _creature;
        private int _destinationPointIndex;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (IsOnPoint())
                {
                    //Когда дойдем до конца массива мы снова начнем с нуля
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                var direction = _points[_destinationPointIndex].position - transform.position; //Получаем вектор направления из одной точки в другую(до нужной нам точки)
                direction.y = 0;
                _creature.SetDirection(direction.normalized);

                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            //magnitute - это длина вектора //Если _treshold больше длины вектора до нашей точки, значит мы дошли до этой точки
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;
        }
    }
}