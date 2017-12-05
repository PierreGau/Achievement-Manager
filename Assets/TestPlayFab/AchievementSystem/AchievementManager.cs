using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

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
        public static AchievementManager instance = null;

        List<int> UnlockedAchievements;
        List<int> LockedAchievements;
        List<baseAchievement> achievements;

        string PlayerID = "";

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

        }

        //will make an api call to get player datas
        public void Init(string _playerID)
        {
            var dataRequest = new GetUserDataRequest();
            dataRequest.PlayFabId = _playerID;
            dataRequest.Keys = new List<string>();
            dataRequest.Keys.Add("UnlockedAchievements");
            PlayFabClientAPI.GetUserData(dataRequest, (result) => 
            {

            },
            (error) => 
            {

            });
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
    }
}
