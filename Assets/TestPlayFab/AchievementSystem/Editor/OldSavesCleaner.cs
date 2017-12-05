using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using PlayFab;


namespace Achievements
{
    public class OldSavesCleaner : EditorWindow
    {
        Rect textRect = new Rect(new Vector2(50, 10), new Vector2(200, 300));
        [MenuItem("Achievements/Delete all old saves")]
        static void OnOpenWindow()
        {
            OldSavesCleaner tm = EditorWindow.GetWindow<OldSavesCleaner>(true, "");
            tm.minSize = new Vector2(350, 100);
            tm.maxSize = new Vector2(350, 100);
            tm.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Are you sure you want to delete your old saves ? ");
            EditorGUILayout.LabelField("this action is definitive.");

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("yes", GUILayout.Width(128), GUILayout.Height(20)))
            {
                string folderPath = EditorUtility.OpenFolderPanel("Select destination folder", "", "");

                //if chosen path is correct
                if (folderPath != string.Empty)
                    DeleteOldSaves(folderPath);
                Close();
            }
            else if (GUILayout.Button("no", GUILayout.Width(128), GUILayout.Height(20)))
            {
                Close();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DeleteOldSaves(string _path)
        {
            string serverDatas = "";
            string[] files = Directory.GetFiles(_path);
            foreach (string file in files)
            {
                //select .json files
                if (file.EndsWith(".json"))
                {
                    File.Delete(file); File.Delete(file + ".meta");
                }
            }

            //request playfab server
            var getRequest = new PlayFab.ServerModels.GetTitleDataRequest();
            PlayFabServerAPI.GetTitleData(getRequest, (result) =>
            {
                foreach (var entry in result.Data)
                {
                    if (string.Compare(entry.Key, "Achievements") == 0)
                    {
                        //create a new save file with playfab datas
                        serverDatas = entry.Value;
                        AchievementEditor.PushToLocalJSON(serverDatas, _path);
                    }            
                }

            },
            (error) => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.ErrorMessage);

            });          
        }
    }
}

