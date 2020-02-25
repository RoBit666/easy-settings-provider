using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Robit.ESP
{
    public class EditorSettings : ScriptableObject
    {
        private Settings _target;
        public Settings target => _target;
        
        private SerializedObject _serializedObject;
        public SerializedObject serializedObject => _serializedObject;

        public static EditorSettings Create(Settings obj)
        {
            var editorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass == true && t.IsAbstract == false && t.IsSubclassOf(typeof(EditorSettings)) == true);
            
            var editorTypeForSettings = editorTypes.FirstOrDefault(type =>
            {
                var attribute = type.GetCustomAttributes(typeof(CustomEditorSettingsAttribute), true)
                    .FirstOrDefault(att => (att as CustomEditorSettingsAttribute)?.type == obj.GetType());

                return attribute != null;
            });
            
            var instance = editorTypeForSettings == null ? CreateInstance<EditorSettings>() : CreateInstance(editorTypeForSettings) as EditorSettings;
            if (instance != null)
            {
                instance._target = obj;
                instance._serializedObject = new SerializedObject(obj);
            }

            return instance;
        }

        public virtual void OnSettingsGUI()
        {
            _serializedObject.UpdateIfRequiredOrScript();

            var enterChildren = true;
            var iterator = _serializedObject.GetIterator();
            while (iterator.NextVisible(enterChildren) == true)
            {
                enterChildren = false;

                if (iterator.propertyPath == "m_Script")
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);
            }

            _serializedObject.ApplyModifiedProperties();
        }
    }
}