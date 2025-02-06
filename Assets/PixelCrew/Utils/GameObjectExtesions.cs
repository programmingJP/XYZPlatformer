using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtesions
    {
        //Экстеншен (расширение) для геймобджекта, позволяет нам вызывать данный метод у геймобджекта
        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            //побитовый сдвиг слоя
            //0010 - слой
            //0110 - маска
            //0110 она нам сложит два числа
            //мы сравниваем находится ли геймобджект в маске, которую мы указываем
            return layer == (layer | 1 << go.layer);
        }
    }
}