using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    //Реализация реалоада уровня на компонентной основе
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>(); //получаем текущую сессию
            //Destroy(session.gameObject); // уничтожаем сессию //получается что после перезагрузки мы возьмем дефолтное состояние сесии

            session.LoadLastSave();
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
