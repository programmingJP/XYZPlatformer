using UnityEngine;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destenationTransform;

        public void Teleport(GameObject target) //target - это тот обьект который мы будем перемещать
        {
            target.transform.position = _destenationTransform.position;
        }
    }
}
