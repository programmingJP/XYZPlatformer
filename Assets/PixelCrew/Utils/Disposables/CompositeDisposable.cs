using System;
using System.Collections.Generic;

namespace PixelCrew.Utils.Disposables
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();//контейнер для наших подписок

        //метод который принимает подписки
        public void Retain(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
        
        //метод который очищает наши подписки
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            
            //очищаем список
            _disposables.Clear();
        }
    }
}