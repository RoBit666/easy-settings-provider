using System;

namespace Robit.ESP
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEditorSettingsAttribute : Attribute
    {
        private Type _type;
        public Type type => _type;
        
        /// <summary>Attribute indicating that this editor will be used to edit the specified type</summary>
        /// <param name="type">Editable type</param>
        public CustomEditorSettingsAttribute(Type type)
        {
            _type = type;
        }
    }
}