﻿using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _invertXScale;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            //Instantiate(_prefab, _target); //задали позицию родителя нашего обьекта (footStepPosition), чтобы обьект спавнился не в сцене(мировые координаты и он не двигался), а именно в родителе
            var instance = SpawnUtils.Spawn(_prefab, _target.position);
            
            var scale = _target.lossyScale;
            scale.x *= _invertXScale ? -1 : 1;
            instance.transform.localScale = scale;
            instance.SetActive(true);
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}
