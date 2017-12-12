using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using SimpleJSON;

namespace Achievements
{
    [System.Serializable]
    public struct baseAchievement
    {
        public int iId;
        public string sName;
        public string sDescription;
        public string sMatchingData;
        public int iTreshold;
        public string sMatchingKey;
        public int iReward;
        public string sImage;
        public string sPath;
    };


    public class AchievementManager : MonoBehaviour
    {
        [SerializeField]
        static bool debugEnabled = true;

        public static AchievementManager instance = null;

        static List<int> UnlockedAchievements = new List<int>();
        static List<int> LockedAchievements = new List<int>();
        static baseAchievement[] achievements;

        static bool datasSuccessfullyLoaded = false;

        string PlayerID = "";

        public static bool DatasSuccessfullyLoaded
        {
            get
            {
                return datasSuccessfullyLoaded;
            }
        }

        //Awake is always called before any Start functions
        void Awake()
        {
            LockedAchievements = new List<int>();
            UnlockedAchievements = new List<int>();

            if (instance == null)
                instance = this;

            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        //Update is called every frame.
        void Update()
        {
            if(datasSuccessfullyLoaded)
            {

            }
        }

        //will make an api call to get player datas
        public void Init(string _playerID)
        {
            var dataRequest = new GetUserDataRequest();
            dataRequest.PlayFabId = _playerID;
            dataRequest.Keys = new List<string>();
            dataRequest.Keys.Add("UnlockedAchievements");
            dataRequest.Keys.Add("LockedAchievements");
            PlayFabClientAPI.GetUserData(dataRequest, LoadDatas, LoadDatasError);
        }
        void LoadDatas(GetUserDataResult result)
        {
            var Unlockedcontent = JSON.Parse(result.Data["UnlockedAchievements"].Value);
            for (int i = 0; i < Unlockedcontent["id"].Count; i++)
            {
                UnlockedAchievements.Add(Unlockedcontent["id"][i].AsInt);
            }

            var Lockedcontent = JSON.Parse(result.Data["LockedAchievements"].Value);
            for (int i = 0; i < Lockedcontent["id"].Count; i++)
            {
                UnlockedAchievements.Add(Lockedcontent["id"][i].AsInt);
            }
            //Debug.Log(LockedAchievements);

            datasSuccessfullyLoaded = true;
        }

        void LoadDatasError(PlayFabError error)
        {
            if(debugEnabled)
                Debug.Log("Error while Getting User Datas");
        }

        public static AchievementManager GetSingleton()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("AchievementManager");
                instance = go.AddComponent<AchievementManager>();
            }
            return instance;
        }

        public List<int> GetLockedAchievements()
        {
            return LockedAchievements;
        }

        public List<int> GetUnLockedAchievements()
        {
            return UnlockedAchievements;
        }

        public static void LogUnlockedAchievements()
        {
            if(debugEnabled)
            {
                Debug.Log("Unlocked Achievements Ids : ");
                for (int i = 0; i < UnlockedAchievements.Count; i++)
                {
                    Debug.Log(UnlockedAchievements[i]);
                }
            }
        }

        public static void LogLockedAchievements()
        {
            if(debugEnabled)
            {
                Debug.Log("Locked Achievements Ids : ");
                for (int i = 0; i < LockedAchievements.Count; i++)
                {
                    Debug.Log(UnlockedAchievements[i]);
                }
            }
        }

        public static bool UnlockAchievement(int id)
        {
            if(LockedAchievements.Contains(id))
            {
                LockedAchievements.Remove(id);
                UnlockedAchievements.Add(id);

                return true;
            }
            else
                return false;
        }

        public static bool UnlockAchievement(string name)
        {
            int id = -1; ;

            for(int i = 0; i < achievements.Length; i++)
            {
                if(string.Compare(achievements[i].sName,name) == 0)
                {
                    id = achievements[i].iId;
                }
            }

            if (id == -1)
                return false;

            if (LockedAchievements.Contains(id))
            {
                LockedAchievements.Remove(id);
                UnlockedAchievements.Add(id);

                return true;
            }
            else
                return false;
        }
    }
}
