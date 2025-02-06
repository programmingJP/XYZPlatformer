using UnityEngine;

namespace PixelCrew.Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta; //дельта здоровья

        public void Apply(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>(); //получаем компонент здоровья

            if (healthComponent != null) //если компонент существует
            {
                healthComponent.ModifyHealth(_hpDelta); //применяем модификацию дельты здоровья
            }
        }
    }
}
