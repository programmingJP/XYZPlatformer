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

            if (itemDef.IsStackable)
            {
                AddToStack(id, value);
            }

            else
            {
                AddNonStack(id,value);
            }
            
            OnChanged?.Invoke(id, Count(id));
        }

        private void AddToStack(string id, int value)
        {
            var isFull = _inventory.Count >= DefsFacade.I.Player.InventorySize;
            
            var item = GetItem(id);
            
            if (item == null) //если предмет равен налл
            {
                if (isFull) return;

                item = new InventoryItemData(id); //мы создаем новый предмет
                _inventory.Add(item); //добавляем в наш список предметов
            }

            item.Value += value; // добавляем количество если предмет уже существует
        }
        private void AddNonStack(string id, int value)
        {
            var itemLasts = DefsFacade.I.Player.InventorySize - _inventory.Count;
            value = Mathf.Min(itemLasts, value);
            
            for (int i = 0; i < value; i++)
            {
                var item = new InventoryItemData(id) {Value = 1};
                _inventory.Add(item);
            }
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.isVoid) return;

            if (itemDef.IsStackable)
            {
                RemoveFromStack(id, value);
            }
            else
            {
                RemoveNonStack(id, value);
            }
            
            OnChanged?.Invoke(id, Count(id));
        }

        private void RemoveFromStack(string id, int value)
        {
            var item = GetItem(id); // получаем предмет
            if (item == null) return;// если айтем налл, то выходим

            item.Value -= value; // если предмет есть отнимаем количество

            //если количество меньше либо равно нулю, то удаляем предмет из инвенторя
            if (item.Value <= 0)
            {
                _inventory.Remove(item);
            }
        }

        private void RemoveNonStack(string id, int value)
        {
            for (int i = 0; i < value; i++)
            {
                var item = GetItem(id); // получаем предмет
                if (item == null) return;// если айтем налл, то выходим
                _inventory.Remove(item);
            }
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
