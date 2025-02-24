using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class ArmHeroComponent : MonoBehaviour
    {
        private Hero _hero;

        private void Start()
        {
            _hero = GetComponent<Hero>();
        }

        public void ArmHero(GameObject go)
        {
           var hero = go.GetComponent<Creatures.Hero.Hero>();

           if (hero != null)
           {
               hero.ArmHero();
           }
        }
    }
}