using System.Collections;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Dialogues
{
    public class DialogueBoxController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space] 
        [SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")] 
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private DialogueData _data;
        private int _currentSentecnce;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }

        public void ShowDialogue(DialogueData data)
        {
            _data = data;
            _currentSentecnce = 0;
            _text.text = string.Empty;
            
            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }
        
        private IEnumerator TypeDialogueText()
        {
            _text.text = string.Empty;
            var sentence = _data.Sentences[_currentSentecnce];
            
            foreach (var letter in sentence)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);

                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();
            _text.text = _data.Sentences[_currentSentecnce];
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);

            _typingRoutine = null;
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentecnce++;

            var isDialogueComplete = _currentSentecnce >= _data.Sentences.Length;
            
            if (isDialogueComplete)
            {
                HideDialogueBox();
            }
            else
            {
                OnStartDialogueAnimation();
            }
        }

        private void HideDialogueBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        private void OnStartDialogueAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogueText());
        }
    }
}