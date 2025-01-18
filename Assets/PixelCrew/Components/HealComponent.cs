using UnityEngine;

namespace PixelCrew.Components
{
    public class HealComponent : MonoBehaviour
    {
        [SerializeField] private int _healValue;
        
        public void ApplyHeal(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyHeal(_healValue);
            }
        }
    }
}
