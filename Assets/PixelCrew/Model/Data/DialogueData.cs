using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class DialogueData
    {
        [SerializeField] private string[] _sentences;
        
        public string[] Sentences => _sentences;
    }
}