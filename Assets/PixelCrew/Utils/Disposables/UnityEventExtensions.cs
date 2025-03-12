using System;
using UnityEngine.Events;

namespace PixelCrew.Utils.Disposables
{
    public static class UnityEventExtensions
    {
        public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction call) //TODO ПЕРЕД ЮНИТИ ЭВЕНТ ДОБАВИТЬ this
        {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
        
        public static IDisposable Subscribe<TType>(this UnityEvent<TType> unityEvent, UnityAction<TType> call) //TODO ПЕРЕД ЮНИТИ ЭВЕНТ ДОБАВИТЬ this
        {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
    }
}