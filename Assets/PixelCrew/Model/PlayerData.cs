using System;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;

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
