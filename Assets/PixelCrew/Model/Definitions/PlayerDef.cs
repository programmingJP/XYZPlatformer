using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu (menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _invetorySize;
        [SerializeField] private int _maxHealth;

        public int InventorySize => _invetorySize;

        public int MaxHealth => _maxHealth;
    }
}