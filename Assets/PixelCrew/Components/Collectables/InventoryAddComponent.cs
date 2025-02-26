using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<PixelCrew.Creatures.Hero.Hero>(); //TODO не работает неймспейс нужно будет починить
            if (hero != null)
                hero.AddInInventory(_id, _count);
        }
    }
}