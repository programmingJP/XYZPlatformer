using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private bool _loop;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex; //Номер текущего спрайта
        private float _nextFrameTime; //Время когда мы должны обновить наш спрайт

        private bool _isPlaying = true;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _frameRate; //В одну секунду из одной секунды у нас получится наша переменная
            _nextFrameTime =  Time.time + _secondsPerFrame; //Текущее время + количество времени на фрейм. Определяем на старте
        }

        private void Update()
        {
            if (!_isPlaying || _nextFrameTime > Time.time) return;
            
            if (_currentSpriteIndex >= _sprites.Length) //Если мы вышли за граници массива
            {
                if (_loop) //Если у нас стоит луп, значит проигрываем анимацию с нуля (сбрасываем текущий спрайт индекс)
                {
                    _currentSpriteIndex = 0;
                }
                else //Если не луп и конец нашего списка спрайтов, то мы выходим, чтобы не зайти в следующий блок
                {
                    _isPlaying = false;
                    _onComplete?.Invoke();
                    return;
                }
            }
            
            _renderer.sprite = _sprites[_currentSpriteIndex]; //Если наступило время сменить кадр, то мы берем и назначаем нужный нам кадр
            _nextFrameTime += _secondsPerFrame; //Обновляем время следующего апдейта кадра
            _currentSpriteIndex++; //В следующий раз мы поставим следующий кадр
            {
                
            }
        }
    }
}
