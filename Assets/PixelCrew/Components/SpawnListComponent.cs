using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void SpawnAll()
        {
            foreach (var spawnData in _spawners)
            {
                spawnData?.Component.Spawn();
            }
        }

        public void Spawn(string id)
        {
            /*
             * Важно помнить, что если у нас код будет горячий и будет вызываться в апдейт лупе или где то еще, то не стоит использовать Linq
             * так как он может генерить дополнительный мусор в виде ошметков памяти
             */
            //В скобках мы передаем редикат (функцию в сокращенном виде) и проверяем на соответствие Id
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            
            
            spawner?.Component.Spawn();
            
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}
