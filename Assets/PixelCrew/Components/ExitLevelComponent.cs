using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;//имя сцены которое мы будем передавать, мы так же можем сделать это через индекс

        public void Exit()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
