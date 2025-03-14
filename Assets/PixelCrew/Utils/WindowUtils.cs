using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = Object.FindObjectOfType<Canvas>();
            Object.Instantiate(window, canvas.transform); //так как мы не наследуемся от монобихевиара, инстантиэйт нам нужно вызывать через Обжект
        }
    }
}