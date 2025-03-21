﻿using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private FloatPersistentProperty _music;
        [SerializeField] private FloatPersistentProperty _sfx;

        public FloatPersistentProperty Music => _music;
        public FloatPersistentProperty Sfx => _sfx;

        private static GameSettings _instance;
        public static GameSettings I => _instance == null ? LoadGameSettings() : _instance;

        private static GameSettings LoadGameSettings()
        {
            return _instance = Resources.Load<GameSettings>("GameSettings");
        }

        private void OnEnable()
        {
            /*//нам нужно сохранить настройки в постоянную память
            //для этого мы воспользуемся PlayerPrefs
            //1 параметр ключ, второй значение, таким образом мы сохраняем их в постоянную память
            PlayerPrefs.SetFloat("music", 20f);
            PlayerPrefs.Save();*/
            
            _music = new FloatPersistentProperty(1,SoundSetting.Music.ToString());
            _sfx = new FloatPersistentProperty(1,SoundSetting.Sfx.ToString());
        }

        private void OnValidate()
        {
            _music.Validate();
            _sfx.Validate();
        }
    }

    public enum SoundSetting
    {
        Music,
        Sfx
    }
}