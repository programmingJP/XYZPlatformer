using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        
        public PlayerData Data => _data;
        private PlayerData _save;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuckInventory { get; private set; } //аксессор

        private void Awake()
        {
            LoadHud();
            
            if (IsSessionExit()) //если сессия существует в сцене значит эта появилась второй и ее нужно уничтожить
            {
                Destroy(gameObject);
                // DestroyImmediate уничтожает обьект в этом же кадре // Плохая практика использовать внутри геймплейного кода, использовать только для эдитор кода
            }
            else //если сессия не существует в сцене значит это первая сессия и ее нужно сохранить между сценами
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuckInventory = new QuickInventoryModel(Data);
            _trash.Retain(QuckInventory);
        }

        private void LoadHud()
        {
            //мы не перезаписываем старый худ, а просто добавляем его к новой сессии
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
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
            
            _trash.Dispose();
            InitModels();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}