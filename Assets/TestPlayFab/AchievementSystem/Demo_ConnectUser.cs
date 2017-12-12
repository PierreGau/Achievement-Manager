using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Achievements;

public class Demo_ConnectUser : MonoBehaviour
{
    [SerializeField]
    string TitleId = "96F3";

    [SerializeField]
    string username = "Hudnel1664";

    [SerializeField]
    string PassWord = "12test12";

    LoginWithPlayFabRequest testCreation;
    string PlayerID = "";

    AchievementManager achievementManager;

    // Use this for initialization
    void Start ()
    {
        testCreation = new LoginWithPlayFabRequest { Username = username, TitleId = TitleId, Password = PassWord };
        PlayFabClientAPI.LoginWithPlayFab(testCreation, OnLoginSuccess, OnLoginFailure);
        achievementManager = AchievementManager.GetSingleton();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(AchievementManager.DatasSuccessfullyLoaded)
        {

            //AchievementManager.LogLockedAchievements();
            //AchievementManager.LogUnlockedAchievements();
        }
	}

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");

        achievementManager.Init(result.PlayFabId);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }  
}
