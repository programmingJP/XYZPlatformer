using System;
using UnityEditor;

namespace PixelCrew.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool GetEnum<TEnumType>(this SerializedProperty property, out TEnumType retValue) 
            where TEnumType : Enum
        //where TEnumType : Enum ограничиваем тип (на дженерик тип) что наш тип должен быть наследником енама иначе мы получим ошибку компиляции
        //с помощью ключевого слова out мы можем указать возвращаемое значение переменной, когда пишем out например TryGetComponent мы передаем прямо кусочек памяти переменной
        {
            retValue = default;
            var names = property.enumNames;
            if (names == null || names.Length == 0)
                return false;

            var emumName = names[property.enumValueIndex];
            retValue = (TEnumType) Enum.Parse(typeof(TEnumType), emumName);
            return true;
        }
    }
}
