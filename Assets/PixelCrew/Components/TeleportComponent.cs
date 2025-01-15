using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destenationTransform;
        [SerializeField] private float _alfaTime = 1f;
        [SerializeField] private float _moveTime = 1f;

        public void Teleport(GameObject target) //target - это тот обьект который мы будем перемещать
        {
            //target.transform.position = _destenationTransform.position;

            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var input = target.GetComponent<PlayerInput>();//получаем спрайт
            SetLockInput(input, true);
            
            yield return AlphaAnimation(sprite, 0f);
            target.SetActive(false);
            
            yield return MoveAnimation(target);
            
            target.SetActive(true);
            yield return AlphaAnimation(sprite, 1f);
            SetLockInput(input, false);
        }

        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position =
                    Vector3.Lerp(target.transform.position, _destenationTransform.position, progress);

                yield return null;
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            var time = 0f;
            var spriteAlpha = sprite.color.a; //получаем прозрачность спрайта

            while (time < _alfaTime) //пока время меньше настройки нашего времени, мы будем выполнять код
            {
                time += Time.deltaTime;
                var progress = time / _alfaTime;
                var temprorarypAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);

                var color = sprite.color;
                color.a = temprorarypAlpha;

                sprite.color = color;

                yield return null;
            }
        }
    }
}
