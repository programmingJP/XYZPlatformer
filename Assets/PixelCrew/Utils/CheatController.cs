using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PixelCrew.Utils
{
    public class CheatController : MonoBehaviour
    {

        //Важно данное не решение не идеальное, при вводе одного чита если имеется похожий по последовательности чит, но более короткий, то коротикий перекроет тот, который мы хотим ввести
        [SerializeField] private float _inputTimeToLive; // время жизни ввода
        [SerializeField] private CheatItem[] _cheats;
    
        private string _currentInput; // текущий ввод
        private float _inputTime; 
        private void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput; // подписываемся на события ввода Keyboard.current.onTextInput - получаем ввод с клавиатуры
        }

        private void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput; // отписываемся от события
        }

        //Добавляем в _currentInput новый символ
        private void OnTextInput(char inputChar)
        {
            _currentInput += inputChar; // добавляем ввод в строку
            _inputTime = _inputTimeToLive; // обнуляем время жизни ввода

            FindAnyCheats(); //пытаемся найти какой то из наших читов
        }

        private void FindAnyCheats()
        {
            foreach (var cheatItem in _cheats)
            {
                if (_currentInput.Contains(cheatItem.Name)) //если текущий инпут совпадает с названием чита который содержиться в cheatItem
                {
                    cheatItem.Action.Invoke(); //вызываем метод
                    _currentInput = string.Empty; // обнуляем текущий ввод, чтобы читы не пересекались
                }
            }
        }

        private void Update()
        {
            if (_inputTime < 0) // если время ввода кончилось
            {
                _currentInput = string.Empty; // обнуляем текущий ввод
            }
            else // если время ввода не кончилось
            {
                _inputTime -= Time.deltaTime; // уменьшаем время жизни ввода // каждый кадр мы уменьшаем время инпут тайм на время прошедшее с момента последнего кадра
                //но если мы что -то вводим - событие OnTextInput, тогда мы обнуляем время жизни ввода
                
                //т.е если мы в _inputTimeToLive поставим 2 секунды, то строка будет жить 2 секунды с момента полседнего нажатия
            }
        }
    }

    [Serializable] // Атрибут для сериализации, чтобы использовать в качестве serializefield монобихевиара
    public class CheatItem
    {
        public string Name; //тут мы будем содержать последовательность клавиш, которые мы должны нажать
        public UnityEvent Action; //тут мы будем смотреть, что мы должны сделать в ответ////тут мы будем содержать действие, которое должно выполняться при нажатии на последовательность клавиш
    }
}