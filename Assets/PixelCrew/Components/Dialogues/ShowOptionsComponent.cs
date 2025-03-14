using PixelCrew.UI.Hud.Dialogues;
using UnityEngine;

namespace PixelCrew.Components.Dialogues
{
    public class ShowOptionsComponent : MonoBehaviour
    {
        [SerializeField] private OptionDialogueData _data;

        private OptionDialogueController _dialogueBox;


        public void Show()
        {
            if (_dialogueBox == null)
                _dialogueBox = FindObjectOfType<OptionDialogueController>();
            
            _dialogueBox.Show(_data);
        }
    }
}