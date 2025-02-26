using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        
        public int Hp;

        public InventoryData Inventory => _inventory;
        public PlayerData Clone()
        {
            //не зависимо от того сколько у нас полей, они будут загнаны в строковые данные json
            // а потом десериализованы из него
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
            /*return new PlayerData
            {
                Coins = Coins,
                Hp = Hp,
                IsArmed = IsArmed
            };*/
        }
    }
}
