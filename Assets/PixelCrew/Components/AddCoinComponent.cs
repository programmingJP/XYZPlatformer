using UnityEngine;

namespace PixelCrew.Components
{
    public class AddCoinComponent : MonoBehaviour
    {
        [SerializeField] private int _coinsToAdd;

        private Creatures.Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Creatures.Hero>();
        }

        public void Add()
        {
            _hero.AddCoins(_coinsToAdd);
        }
    }
}
