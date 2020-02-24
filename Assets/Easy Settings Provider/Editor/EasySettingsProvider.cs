using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;

namespace Robit.ESP
{
    public sealed class EasySettingsProvider<T> : SettingsProvider
        where T : Settings
    {
        private readonly string _filePath;
        public string filePath => _filePath;
        
        private T _settingsObject = null;
        public T settingsObject => _settingsObject;
        
        private EditorSettings _editorObject = null;

        public EasySettingsProvider(string settingsPath, SettingsScope scopes, string projectSettingsPath, IEnumerable<string> keywords = null) : base(settingsPath, scopes, keywords)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "ProjectSettings", $"{projectSettingsPath}.json");
            CreateSettings();
        }

        private void CreateSettings()
        {
            if (_settingsObject == null)
            {
                _settingsObject = ScriptableObject.CreateInstance<T>();
            }

            if (string.IsNullOrEmpty(_filePath) == false && File.Exists(_filePath) == true)
            {
                _settingsObject.Load(_filePath);
            }
            
            if (_editorObject == null)
            {
                _editorObject = EditorSettings.Create(_settingsObject);
            }
        }

        private void DestroySettings()
        {
            if (string.IsNullOrEmpty(_filePath) == false)
            {
                settingsObject.Save(_filePath);
            }
            
            if (_settingsObject != null)
            {
                UObject.DestroyImmediate(_settingsObject);
            }

            if (_editorObject != null)
            {
                UObject.DestroyImmediate(_editorObject);
            }
        }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            CreateSettings();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            DestroySettings();
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);
            
            _editorObject.OnSettingsGUI();
        }
    }
}