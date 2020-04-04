using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Kameosa
{
    namespace Services
    {
        public static class FileService
        {
            private const string BINARY_FILE_EXTENSION = ".dat";
            private const string JSON_FILE_EXTENSION = ".json";

            public static void Save<T>(T t, string filename = "default")
            {
                BinarySave(t, Application.persistentDataPath, filename);
            }

            public static T Load<T>(string filename = "default")
            {
                return BinaryLoad<T>(Application.persistentDataPath, filename);
            }

            public static void BinarySave<T>(T t, string folderPath, string filename)
            {
                string path = Path.Combine(folderPath, filename + BINARY_FILE_EXTENSION);
                //Debug.Log("Saving " + path);
                FileStream fileStream = null;

                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    fileStream = File.Create(path);
                    binaryFormatter.Serialize(fileStream, t);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }

            public static T BinaryLoad<T>(string folderPath, string filename)
            {
                string path = Path.Combine(folderPath, filename + BINARY_FILE_EXTENSION);
                //Debug.Log("Loading " + path);
                FileStream fileStream = null;
                T result = default(T);

                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    fileStream = File.Open(path, FileMode.Open);
                    result = (T) binaryFormatter.Deserialize(fileStream);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }

                return result;
            }

            public static void JsonSave<T>(T t, string folderPath, string filename)
            {
                string data = JsonUtility.ToJson(t);

                string path = Path.Combine(folderPath, filename + JSON_FILE_EXTENSION);

                File.WriteAllText(path, data);
            }

            public static T JsonLoad<T>(string folderPath, string filename)
            {
                string path = Path.Combine(folderPath, filename + JSON_FILE_EXTENSION);

                if (File.Exists(path))
                {
                    string data = File.ReadAllText(path);
                    T result = JsonUtility.FromJson<T>(data);

                    return result;
                }
                else
                {
                    Debug.LogError("Cannot load json!");

                    return default(T);
                }
            }
        }
   }
}
