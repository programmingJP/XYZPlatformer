using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class AddCoinComponent : MonoBehaviour
    {
        [SerializeField] private int _coinsToAdd;

        private Creatures.Hero.Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Creatures.Hero.Hero>();
        }

        public void Add()
        {
            _hero.AddCoins(_coinsToAdd);
        }
    }
}
