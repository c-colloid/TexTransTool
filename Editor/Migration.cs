#if UNITY_EDITOR

using System;
using System.IO;
using net.rs64.TexTransTool.Utils;
using UnityEditor;
using UnityEngine;


namespace net.rs64.TexTransTool
{
    [InitializeOnLoad]
    internal static class Migration
    {
        public static string SaveDataVersionPath = "ProjectSettings/net.rs64.TexTransTool-Version.json";

        static Migration()
        {
            if (!File.Exists(SaveDataVersionPath))
            {
                var NawSaveDataVersion = new SaveDataVersionJson();
                NawSaveDataVersion.SaveDataVersion = ToolUtils.ThiSaveDataVersion;
                var jsonStr = JsonUtility.ToJson(NawSaveDataVersion);

                File.WriteAllText(SaveDataVersionPath, jsonStr);
            }
            else
            {
                var jsonStr = File.ReadAllText(SaveDataVersionPath);
                var SaveDataVersionJson = JsonUtility.FromJson<SaveDataVersionJson>(jsonStr);

                if (SaveDataVersionJson.SaveDataVersion == (ToolUtils.ThiSaveDataVersion - 1))
                {
                    MigrationExecute();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public static void MigrationExecute()
        {
                    throw new NotImplementedException();
        }


        [Serializable]
        private class SaveDataVersionJson
        {
            public int SaveDataVersion;
        }
    }
}
#endif