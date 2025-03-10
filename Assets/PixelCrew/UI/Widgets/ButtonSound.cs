using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.Widgets
{
    public class ButtonSound : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _audioClip;

        //кэшируем, такак как кнопки часто используются, чтобы постоянно не искать обьект(оптимизация)
        private AudioSource _source;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(_source == null) 
                _source = GameObject.FindWithTag("SfxAudioSource").GetComponent<AudioSource>(); //кэшируем, такак как кнопки часто используются, чтобы постоянно не искать обьект(оптимизация)
            
            _source.PlayOneShot(_audioClip);
        }
    }
}