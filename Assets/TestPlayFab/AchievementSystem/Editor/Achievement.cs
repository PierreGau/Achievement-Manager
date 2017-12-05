using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using UnityEditor;
using SimpleJSON;
using System.IO;



namespace Achievements
{
    public class AchievementEditor
    {
        public baseAchievement values;
        public static List<baseAchievement> achievements = new List<baseAchievement>();

        static bool isDescriptionActive = true;
        static bool isRewardActive = true;
        static bool isImageActive = true;

        public string GetImagePath()
        {
            return values.sPath;
        }

        public string GetName()
        {
            return values.sName;
        }

        public AchievementEditor(string _name, string _description, string _MatchingData, string _MatchingKey, string _Image, int _Treshold, int _Reward, string imagePath)
        {
            values.sName = _name;
            values.sDescription = _description;
            values.sMatchingData = _MatchingData;
            values.sMatchingKey = _MatchingKey;
            values.sImage = _Image;
            values.iTreshold = _Treshold;
            values.iReward = _Reward;
            values.sPath = imagePath;
        }

        public AchievementEditor()
        {
            values.sName = "";
            values.sDescription = "";
            values.sMatchingData = "";
            values.sMatchingKey = "";
            values.sImage = "";
            values.iTreshold = 0;
            values.iReward = 0;
            values.sPath = "";
        }

        // faire. ça. bien.
        static public void EditorDisplay(baseAchievement achievement)
        {
            string display = "Achievement name : " + achievement.sName;
            display += ", Achievement Description : " + achievement.sDescription;

            //Debug.Log(display);
        }

        public AchievementEditor CreatFromJSON(string JSONObject)
        {
            AchievementEditor toReturn = new AchievementEditor();

            return toReturn;
        }

        public string ToJSON()
        {
            string toReturn = "";
            return toReturn;
        }

        public static void PushToPlayFab(Rect position)
        {
            string datas = ConvertToJson(achievements);

            var updateRequest = new SetTitleDataRequest();
            updateRequest.Key = "Achievements";
            updateRequest.Value = datas;
            PlayFabServerAPI.SetTitleData(updateRequest, (result) =>
            {
                //Debug.Log("Set titleData successful");
                Rect popupRect = new Rect(position.x - 200, position.y, 200, 80);
                PopUp newPopUp = new PopUp();
                newPopUp.Init("Datas Succesfully saved to PlayFab", popupRect);
            },
            (error) =>
            {
                Debug.Log("Got error setting titleData:");
                Debug.Log(error.ErrorMessage);
            });
        }

        public static void PushToLocalJSON(string _path)
        {
            string datas = ConvertToJson(achievements);
            int fileCount = 0;
            string[] files = Directory.GetFiles(_path);
            foreach (string file in files)
            {
                if (file.EndsWith(".json"))
                    fileCount++;
            }
            StreamWriter writer = new StreamWriter(_path + "/save" + fileCount + ".json");
            try
            {
                writer.WriteLine(datas);
            }
            catch
            {
                Debug.Log("error when trying to write in " + _path);
            }

            writer.Dispose();
            writer.Close();
        }

        public static void PushToLocalJSON(string _datas, string _path)
        {
            int fileCount = 0;
            string[] files = Directory.GetFiles(_path);
            foreach (string file in files)
            {
                if (file.EndsWith(".json"))
                    fileCount++;
            }
            StreamWriter writer = new StreamWriter(_path + "/save" + fileCount + ".json");
            try
            {
                writer.WriteLine(_datas);
            }
            catch
            {
                Debug.Log("error when trying to write in " + _path);
            }

            writer.Dispose();
            writer.Close();
        }

        public static void LoadFromLocalJSON(string _path)
        {
            string datas;

            StreamReader reader = new StreamReader(_path);
            try
            {
                datas = reader.ReadToEnd();
                //Debug.Log(datas);
                LoadFromJson(datas);
            }
            catch
            {
                Debug.Log("error when trying to write in " + _path);
            }

            reader.Dispose();
            reader.Close();
        }

        public static void LoadFromPlayfab(Rect position)
        {
            string datas;
            var getRequest = new PlayFab.ServerModels.GetTitleDataRequest();
            PlayFabServerAPI.GetTitleData(getRequest, (result) => {
                bool isLoadingOk = false;
                foreach (var entry in result.Data)
                {
                    if (string.Compare(entry.Key, "Achievements") == 0)
                    {
                        isLoadingOk = true;
                        datas = entry.Value;
                        LoadFromJson(datas);
                    }
                    else
                    {
                        Debug.Log("No achievements in database");
                    }
                }
                if(isLoadingOk)
                    newPopUp(position, "Datas Succesfully loaded from PlayFab");
            },
            (error) => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.ErrorMessage);

            });
        }

        static void newPopUp(Rect position,string _text)
        {
            Rect popupRect = new Rect(position.x - 200, position.y, 200, 80);
            PopUp newPopUp = new PopUp();
            newPopUp.Init(_text, popupRect);
        }

        static void OnOpenWindowMessage()
        {
            AchievementDisplay tm = EditorWindow.GetWindow<AchievementDisplay>(true, "congrat");
            tm.minSize = new Vector2(600, 100);
            tm.Show();
        }

        static public string ConvertToJson(baseAchievement toConvert)
        {
            string json = "\"" + toConvert.sName + "\":" + EditorJsonUtility.ToJson(toConvert);

            return json;
        }

        static public string ConvertToJson(List<baseAchievement> toConvert)
        {
            string json = "{";

            for(int i = 0; i < toConvert.Count; i++)
            {
                json += "\"" + toConvert[i].sName + "\":" + EditorJsonUtility.ToJson(toConvert[i]);
                if(i != toConvert.Count-1)
                    json += ",";
            }
            

            json += "}";
            return json;
        }

        static public void LoadFromJson(string _json)
        {
            var content = JSON.Parse(_json);

            achievements.Clear();
            for (int i = 0; i < content.Count; i++)
            {
                baseAchievement newOne;

                newOne.iId = content[i]["iId"];
                newOne.sName = content[i]["sName"];
                newOne.sDescription = content[i]["sDescription"];
                newOne.sMatchingData = content[i]["sMatchingData"];
                newOne.iTreshold = content[i]["iTreshold"];
                newOne.sMatchingKey = content[i]["sMatchingKey"];
                newOne.iReward = content[i]["iReward"];
                newOne.sImage = content[i]["sImage"];
                newOne.sPath = content[i]["sPath"];

                achievements.Add(newOne);
            }      
        }

        static public List<baseAchievement> ConvertFromJson(string _json)
        {
            var content = JSON.Parse(_json);
            List<baseAchievement> listToReturn = new List<baseAchievement>();

            for (int i = 0; i < content.Count; i++)
            {
                baseAchievement newOne;

                newOne.iId = content[i]["iId"];
                newOne.sName = content[i]["sName"];
                newOne.sDescription = content[i]["sDescription"];
                newOne.sMatchingData = content[i]["sMatchingData"];
                newOne.iTreshold = content[i]["iTreshold"];
                newOne.sMatchingKey = content[i]["sMatchingKey"];
                newOne.iReward = content[i]["iReward"];
                newOne.sImage = content[i]["sImage"];
                newOne.sPath = content[i]["sPath"];

                listToReturn.Add(newOne);
            }

            return listToReturn;
        }

        static public void ClearList()
        {
            achievements.Clear();
        }

        //Fonction de comparaison d'achievements
    }
}