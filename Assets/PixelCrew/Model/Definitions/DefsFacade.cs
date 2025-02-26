using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu (menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        //передаем ссылки на наши обьекты
        [SerializeField] private InventoryItemsDef _items;

        public InventoryItemsDef Items => _items;

        private static DefsFacade _instance;
        /// <summary>
        /// мы проверяем равен ли инстанс налл если да, то мы загружаем дефс, иначе создаем инстанс
        /// </summary>
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}