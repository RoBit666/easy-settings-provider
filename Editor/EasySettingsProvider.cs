using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;

namespace Robit.ESP
{
    public sealed class EasySettingsProvider<T> : SettingsProvider
        where T : Settings<T>
    {
        private EditorSettings _editorObject = null;

        public EasySettingsProvider(string settingsPath, SettingsScope scopes, IEnumerable<string> keywords = null) : base(settingsPath, scopes, keywords) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            var instance = Settings<T>.instance;
            instance.Load();
            
            if (_editorObject == null)
            {
                _editorObject = EditorSettings.Create(instance);
            }
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            
            var instance = Settings<T>.instance;
            instance.Save();

            if (_editorObject != null)
            {
                UObject.DestroyImmediate(instance);
            }
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);
            
            _editorObject.OnSettingsGUI();
        }
    }
}