using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            //Instantiate(_prefab, _target); //задали позицию родителя нашего обьекта (footStepPosition), чтобы обьект спавнился не в сцене(мировые координаты и он не двигался), а именно в родителе
            var instance =Instantiate(_prefab, _target.position, Quaternion.identity); //.position - позиция в мировых координатах

            instance.transform.localScale = _target.lossyScale; //lossyScale - это как обьект виден относительно всего мира, т.е в мировых координатах.
        }
    }
}
