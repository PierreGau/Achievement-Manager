using UnityEngine;
//using System.Collections;
using UnityEditor;
using System.IO;
using System.Threading;
/// <summary>
/// Faire une fonction de load d'achievement qui vas chercher ceux présents dans la base de données
/// et les load direct à l'ouverture de la fenêtre
/// plus proposer a l'utilisateur de load si unity perd le lien (a chaque modification de script editor)
/// </summary>
/// 
namespace Achievements
{
    public class AchievementDisplay : EditorWindow
    {
        static string folderPath = "";
        static string filePath = "";
        Vector2 scrollPos = Vector2.zero;
        Texture2D add;
        AchievementsCreator creator;
        Texture2D[] achievementsTexture;

        [MenuItem("Achievements/Display")]
        static void OnOpenWindow()
        {
            AchievementDisplay tm = EditorWindow.GetWindow<AchievementDisplay>(true, "Achievements");
            tm.minSize = new Vector2(680, 100);
            tm.Show();
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginHorizontal();
            if (!add)
            {
                add = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/TestPlayFab/AchievementSystem/Editor/Resources/plus.png", typeof(Texture));
            }
            if (GUILayout.Button(add, GUILayout.Width(64), GUILayout.Height(64)))
            {
                if (!creator)
                {
                    creator = new AchievementsCreator();
                    creator.Display();
                }
            }
            
            if (AchievementEditor.achievements.Count != 0)
            {
                if (achievementsTexture == null || achievementsTexture.Length < AchievementEditor.achievements.Count)
                {
                    achievementsTexture = new Texture2D[AchievementEditor.achievements.Count];
                }
                for (int i = 0; i < AchievementEditor.achievements.Count; i++)
                {
                    if (!achievementsTexture[i])
                    {
                        achievementsTexture[i] = (Texture2D)AssetDatabase.LoadAssetAtPath(AchievementEditor.achievements[i].sPath, typeof(Texture));
                    }
                    if (GUILayout.Button(achievementsTexture[i], GUILayout.Width(64), GUILayout.Height(64)))
                    {
                        AchievementEditor.EditorDisplay(AchievementEditor.achievements[i]);
                    }

                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save to PlayFab", GUILayout.Width(128), GUILayout.Height(20)))
            {
                AchievementEditor.PushToPlayFab(position);     
            }

            else if (GUILayout.Button("Load From Playfab", GUILayout.Width(128), GUILayout.Height(20)))
            {
                AchievementEditor.LoadFromPlayfab(position);
            }

            else if (GUILayout.Button("Clear list", GUILayout.Width(128), GUILayout.Height(20)))
            {
                AchievementEditor.ClearList();
            }

            else if (GUILayout.Button("Save to Local", GUILayout.Width(128), GUILayout.Height(20)))
            {
                folderPath = EditorUtility.OpenFolderPanel("Select destination folder", folderPath, "");
                if (folderPath != string.Empty) 
                    AchievementEditor.PushToLocalJSON(folderPath);
            }
            else if (GUILayout.Button("Load from Local", GUILayout.Width(128), GUILayout.Height(20)))
            {
                filePath = EditorUtility.OpenFilePanel("Select destination folder", filePath, "json");
                if (filePath != string.Empty)
                    AchievementEditor.LoadFromLocalJSON(filePath);
            }
            EditorGUILayout.EndHorizontal();
        }

        void SaveInPlayFab()
        {
            //Vérification si playfab a déjà la dernière version à jour.
        }
    }
}
