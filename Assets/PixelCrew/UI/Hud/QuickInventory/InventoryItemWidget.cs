﻿using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _index;

        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            session.QuckInventory.SelectedIndex.SubscribeAndInvoke(OnIndexChanged);
        }

        private void OnIndexChanged(int newValue, int _)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id); //получаем айдишку дефенишена
            _icon.sprite = def.Icon; //получаем иконку
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }
    }
}