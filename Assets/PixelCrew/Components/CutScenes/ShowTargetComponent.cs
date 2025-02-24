using UnityEngine;

namespace PixelCrew.Components.CutScenes
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target; //Куда будет смотреть наша комаера которая показывает что - то
        [SerializeField] private CameraStateControllerComponent _controller;
        [SerializeField] private float _delay;

#if UNITY_EDITOR
        //делаем еще одну микро оптимизацию, чтобы пытаться взять компонент со цены, а не в рантайме
        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<CameraStateControllerComponent>();
        }
#endif

        public void ShowTarget()
        {
            _controller.SetPosition(_target.position); //меняем на позицию таргета
            _controller.SetState(true);//меняем стейт
            Invoke(nameof(MoveBack), _delay); //короткая запись (какой метод вызвать, через сколько секунд)
            //nameof это встроенное в с# ключевое слово которое возращает стринг имени переданного в него члена
        }

        private void MoveBack()
        {
            _controller.SetState(false);
        }
    }
}
