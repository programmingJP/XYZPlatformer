using System;
using System.Collections.Generic;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        //метод для добавления обьекта в инвентарь
        public void Add(string id, int value)
        {
            if(value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.isVoid) return;


            var item = GetItem(id);
            if (item == null) //если предмет равен налл
            {
                item = new InventoryItemData(id); //мы создаем новый предмет
                _inventory.Add(item); //добавляем в наш список предметов
            }

            item.Value += value; // добавляем количество если предмет уже существует
            
            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.isVoid) return;
            
            var item = GetItem(id); // получаем предмет
            if (item == null) return;// если айтем налл, то выходим

            item.Value -= value; // если предмет есть отнимаем количество

            //если количество меньше либо равно нулю, то удаляем предмет из инвенторя
            if (item.Value <= 0)
            {
                _inventory.Remove(item);
            }
            
            OnChanged?.Invoke(id, Count(id));
        }

        //получаем предмет, если такой предмет уже имеется, то мы возращаем его
        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                {
                    return itemData; 
                }
            }

            return null;
        }

        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                    return item.Value;
            }

            return count;
        }
    }

    //Сам предмет
    [Serializable]
    public class InventoryItemData
    {
       [InventoryId] public string Id; //индефикатор
        public int Value; //количество

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
