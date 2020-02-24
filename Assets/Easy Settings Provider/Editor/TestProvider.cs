using UnityEditor;
using UnityEngine;

namespace Robit.ESP
{
    public class TestProvider
    {
        private static Test _test = null;
        public static Test test => _test;
        
        [SettingsProvider]
        private static SettingsProvider CreateProvider()
        {
            var provider = new EasySettingsProvider<Test>("Test/Test Settings", SettingsScope.User, @"Test Settings\Settings");
            _test = provider.settingsObject;
            
            return provider;
        }
    }

    public class Test : Settings
    {
        [SerializeField] private int _testInt = 1337;
        [SerializeField] private string _testString = "Test";
    }

    [CustomEditorSettings(typeof(Test))]
    public sealed class TestEditor : EditorSettings
    {
        public override void OnSettingsGUI()
        {
            base.OnSettingsGUI();
            
            EditorGUILayout.LabelField("Hello world!");
        }
    }
}
