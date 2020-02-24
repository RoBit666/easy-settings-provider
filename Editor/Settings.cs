using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Robit.ESP
{
    public abstract class Settings : ScriptableObject
    {
        public virtual void Save(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == true)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            
            var directoryName = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }
            
            using (var sw = new StreamWriter(filePath, false, Encoding.Default))
            {
                var json = JsonUtility.ToJson(this, true);
                sw.Write(json);
            }
        }

        public virtual void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == true)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (File.Exists(filePath) == false)
            {
                throw new FileLoadException("File not found!", filePath);
            }
            
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                var json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json) == false)
                {
                    JsonUtility.FromJsonOverwrite(json, this);
                }
            }
        }
    }
}