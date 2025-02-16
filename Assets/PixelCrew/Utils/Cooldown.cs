using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;

        private float _timesUp;
        public void Reset()
        {
            _timesUp = Time.time + _value;
        }
        
        //проверяем что таймс ап меньше чем тайм.тайм (если меньше то время вышло)
        public bool IsReady => _timesUp <= Time.time; 
    }
}
