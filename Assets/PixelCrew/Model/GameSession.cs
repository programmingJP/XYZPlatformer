using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        
        public PlayerData Data => _data;
        private PlayerData _save;

        private void Awake()
        {
            if (IsSessionExit()) //если сессия существует в сцене значит эта появилась второй и ее нужно уничтожить
            {
                Destroy(gameObject);
                // DestroyImmediate уничтожает обьект в этом же кадре // Плохая практика использовать внутри геймплейного кода, использовать только для эдитор кода
            }
            else //если сессия не существует в сцене значит это первая сессия и ее нужно сохранить между сценами
            {
                Save();
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionExit()
        {
            //Поиск сессии в сцене

            var sessions = FindObjectsOfType<GameSession>();

            foreach (var gameSession in sessions)
            {
                if (gameSession != this)// если игровая сессия не равна текущей сессии то вернем тру
                    return true;

            }

            return false; //а иначе вернем фолс
        }
        
        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
        }
    }
}