using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace Onur.Template
{
    abstract public class BaseDatabase : SingletonDontDestroy<BaseDatabase>
    {

        [SerializeField] BaseScriptableObject[] _saveData;
        [SerializeField] bool _resetGameAtLaunch;
        [SerializeField] bool _disableDBLoad;

        #region BASE
        protected override void Awake()
        {

#if UNITY_EDITOR
            Debug.Log("[Database::Awake] Database is working on Editor Mode.");
            if (_resetGameAtLaunch)
            {
                reset();
            }
            else if (_disableDBLoad == false)
            {
                loadGame();
            }
#else
            Debug.Log("[Database::Awake] Database is working on Device Mode.");
            loadGame();
#endif
        }
        #endregion

        /// <summary>
        /// Game Reset System
        /// </summary>
        public void reset()
        {
            foreach (var item in _saveData)
            {
                item.reset();
            }
            saveGame();
        }

        /// <summary>
        /// Game Save System
        /// </summary>
        public void saveGame()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            for (int i = 0; i < _saveData.Length; i++)
            {
                BaseScriptableObject objectToPersist = _saveData[i];
                string persistentDataSubdirectoryPath = Application.persistentDataPath + "/savedata/";
                string persistentDataPath = persistentDataSubdirectoryPath + string.Format("{0}_{1}.sdata", objectToPersist.GetType(), i);
                try
                {
                    if (!Directory.Exists(persistentDataSubdirectoryPath))
                    {
                        DirectoryInfo info = Directory.CreateDirectory(persistentDataSubdirectoryPath);
                    }
                    FileStream file = File.Create(persistentDataPath);
                    string json = JsonUtility.ToJson(objectToPersist);
                    serializer.Serialize(file, json);
                    file.Close();
                }
                catch (System.Exception)
                {
                }
            }
        }

        private void loadGame()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            int loadedFileNum = 0;
            for (int i = 0; i < _saveData.Length; i++)
            {
                BaseScriptableObject objectToPersist = _saveData[i];
                string persistentDataPath = Application.persistentDataPath + string.Format("/savedata/{0}_{1}.sdata", objectToPersist.GetType(), i);
                if (File.Exists(persistentDataPath))
                {
                    FileStream file = File.Open(persistentDataPath, FileMode.Open);
                    string json = (string)serializer.Deserialize(file);
                    file.Close();
                    JsonUtility.FromJsonOverwrite(json, objectToPersist);

                    loadedFileNum++;
                }
            }
            if (loadedFileNum == 0)
            {
                reset();
            }
        }
    }
}