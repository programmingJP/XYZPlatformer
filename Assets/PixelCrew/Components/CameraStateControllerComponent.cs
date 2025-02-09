using Cinemachine;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CameraStateControllerComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineVirtualCamera _camera;
        
        private static readonly int ShowTargetKey = Animator.StringToHash("ShowTarget"); //микро оптимизация чтобы постоянно не считался данный хэщ

        public void SetPosition(Vector3 TargetPosition) //устанавливаем поизицию камеры туда, куда хотим поставить
        {
            TargetPosition.z = _camera.transform.position.z; //так как у нас Z координата не участвует, мы записываем сюда значение нашей камеры по данной оси
            _camera.transform.position = TargetPosition;
        }

        public void SetState(bool state)
        {
            _animator.SetBool(ShowTargetKey, state);
        }
    }
}
