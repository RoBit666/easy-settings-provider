using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Robit.ESP
{
    public abstract class Settings<T> : ScriptableObject
        where T : Settings<T>
    {
        private static T _instance = null;
        
        /// <summary>Path relative to the ProjectSettings directory</summary>
        public abstract string localFilePath { get; }

        /// <summary>Singleton instance</summary>
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<T>();
                    if (_instance.Load() == false)
                    {
                        _instance.Save();
                    }
                }

                return _instance;
            }
        }
        
        /// <summary>Global file path</summary>
        public virtual string filePath => $"{Directory.GetCurrentDirectory()}/ProjectSettings/{localFilePath}.json";
        
        /// <summary>Method for saving settings</summary>
        /// <returns>Returns true if the save was successful</returns>
        public virtual bool Save()
        {
            var directoryName = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }

            var sw = new StreamWriter(filePath, false, Encoding.Default);
            try
            {
                var json = JsonUtility.ToJson(this, true);
                sw.Write(json);

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }
            finally
            {
                sw.Dispose();
            }
        }
        
        /// <summary>Method for loading settings</summary>
        /// <returns>Returns true if the load was successful</returns>
        public virtual bool Load()
        {
            if (File.Exists(filePath) == false)
            {
                return false;
            }
            
            var sr = new StreamReader(filePath, Encoding.Default);
            try
            {
                var json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json) != false)
                {
                    return false;
                }
                
                JsonUtility.FromJsonOverwrite(json, this);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sr.Dispose();
            }
        }
    }
}