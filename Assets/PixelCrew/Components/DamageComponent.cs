using UnityEngine;

namespace PixelCrew.Components
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damage; //урон

        public void ApplyDamage(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>(); //получаем компонент здоровья

            if (healthComponent != null) //если компонент существует
            {
                healthComponent.ApplyDamage(_damage); //применяем урон
            }
        }
    }
}
