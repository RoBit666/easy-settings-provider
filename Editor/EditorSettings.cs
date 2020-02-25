using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Robit.ESP
{
    public class EditorSettings : ScriptableObject
    {
        private UObject _target;
        private SerializedObject _serializedObject;
        
        /// <summary>Instance of Editable Settings</summary>
        public UObject target => _target;
        
        /// <summary>Instance of a serialized settings object</summary>
        public SerializedObject serializedObject => _serializedObject;

        /// <summary>Method for creating an editor instance</summary>
        /// <param name="obj">The object for which you want to create an editor</param>
        /// <returns>Editor Instance</returns>
        /// <exception cref="ArgumentNullException">If parameter is null</exception>
        public static EditorSettings Create(UObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            
            // Search for all descendants of the EditorSettings class
            var editorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass == true && t.IsAbstract == false && t.IsSubclassOf(typeof(EditorSettings)) == true);
            
            // Search for a suitable editor for a given type through the attribute
            var editorTypeForSettings = editorTypes.FirstOrDefault(type =>
            {
                var attribute = type.GetCustomAttributes(typeof(CustomEditorSettingsAttribute), true)
                    .FirstOrDefault(att => (att as CustomEditorSettingsAttribute)?.type == obj.GetType());

                return attribute != null;
            });
            
            // Creating an editor instance and filling in its fields
            var instance = editorTypeForSettings == null ? CreateInstance<EditorSettings>() : CreateInstance(editorTypeForSettings) as EditorSettings;
            instance._target = obj;
            instance._serializedObject = new SerializedObject(obj);
            
            return instance;
        }
        
        /// <summary>GUI Rendering Method</summary>
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