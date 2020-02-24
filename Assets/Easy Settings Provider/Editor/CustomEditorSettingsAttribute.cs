using System;

namespace Robit.ESP
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEditorSettingsAttribute : Attribute
    {
        private Type _type;
        public Type type => _type;
        
        public CustomEditorSettingsAttribute(Type type)
        {
            _type = type;
        }
    }
}