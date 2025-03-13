using PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.Components.Dialogues.Editor
{
    [CustomEditor(typeof(ShowDialogueComponent))]
    public class ShowDialogueComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            if (_modeProperty.GetEnum(out ShowDialogueComponent.Mode mode))
            {
                switch (mode)
                {
                    case ShowDialogueComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogueComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}