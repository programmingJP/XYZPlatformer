using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    //Реализация реалоада уровня на компонентной основе
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
