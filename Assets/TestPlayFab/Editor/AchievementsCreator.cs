using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


namespace Achievements
{
    public class AchievementsCreator : EditorWindow
    {
        Texture2D achievementThumbnail = null;
        Texture2D previousThumbnail = null;
        string achievementName = "exemple";
        string description = "this is an example";
        string matchingData = "none";
        string imageName;
        string matchingKey = "none";
        int Treshold;
        int reward;
        string path;
        Vector2 scrollpos = new Vector2(0, 0);

        Texture add;

        public static void OnOpenWindow()
        {
            AchievementsCreator tm = GetWindow<AchievementsCreator>(true, "Achievement Creator");
            tm.Show();
        }
        public void Display()
        {
            OnOpenWindow();
            OnGUI();
        }

        void OnGUI()
        {
            //permet de scroll pour afficher les éléments n'apparaissant pas dans la fenêtre
            scrollpos = EditorGUILayout.BeginScrollView(scrollpos);
            achievementName = EditorGUILayout.TextField("Achievement Name : ", achievementName);
            EditorGUILayout.Space();

            if (Achievement.IsDescriptionActive)
            {
                description = EditorGUILayout.TextField("Achievement Description : ", description);
                EditorGUILayout.Space();
            }

            matchingKey = EditorGUILayout.TextField(new GUIContent("Related Key : ", "Key in database where is stocked the Matching Data, none if you want to manualy unlock"), matchingKey);
            EditorGUILayout.Space();

            matchingData = EditorGUILayout.TextField(new GUIContent("Related Data : ", "Data To check to validate the Achievement, none if you want to manualy unlock"), matchingData);
            EditorGUILayout.Space();

            Treshold = EditorGUILayout.IntField("Treshold value : ", Treshold);
            EditorGUILayout.Space();

            reward = EditorGUILayout.IntField("Reward value : ", reward);
            EditorGUILayout.Space();

            achievementThumbnail = (Texture2D)EditorGUILayout.ObjectField("Thumbnail", achievementThumbnail, typeof(Texture2D), false);
            if (achievementThumbnail != previousThumbnail)
            {
                imageName = achievementThumbnail.name;
                previousThumbnail = achievementThumbnail;
                path = AssetDatabase.GetAssetPath(achievementThumbnail);
                //Debug.Log(path);
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Add"))
            {
                Achievement newAchievement = new Achievement(achievementName, description, matchingData, matchingKey, imageName, Treshold, reward, path);

                // mettre en place 3 possibilités pour le isAchievementOk :
                // 1 : Il est bon 
                //     => popUp voullez vous valider  [oui / non] close.
                // 2 : Des données importantes sont identiques à un achievement existant (image/description/matchingData) 
                //     => popUp message Warning [la donnée **nomDeLaDonée* correspond a celle de l'achievement *achievementName* voullez vous quand même valider ? [oui / non] ]
                // 3 : Des données critiques sont identiques à un achievement existant(Nom)
                //     => popUp message Alert [Un achievement porte déjà le même nom veuillez changer le nom [OK] ]
             
                if (isAchievementOk(newAchievement))
                {
                    Achievement.achievements.Add(newAchievement.values);
                    //Debug.Log("Done");        
                    //string jason = "\"" + newAchievement.values.sName + "\":" + EditorJsonUtility.ToJson(newAchievement.values);
                    //Debug.Log(jason);
                }
                else
                {
                    Debug.Log("OOOOOOOOOOOOOOOOH");
                    EditorGUILayout.HelpBox("This achievement already exist", MessageType.Warning);
                }
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Clear List"))
            {
                ClearAchievementList();
            }

            EditorGUILayout.EndScrollView();
        }

        //renvoyer short 0 = ok, 1 = warning, 2 = critique
        //out Achievement qui comporte des données identiques;
        bool isAchievementOk(Achievement newAchievement)
        {
            bool isOk = true;

            foreach (baseAchievement a in Achievement.achievements)
            {
                if (string.Compare(newAchievement.GetName(), a.sName) == 0)
                {
                    isOk = false;
                }
            }

            return isOk;
        }

        void ClearAchievementList()
        {
            Achievement.achievements.Clear();
        }
    }
}

