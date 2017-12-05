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
    // Use this for initialization
    void Start ()
    {
        testCreation = new LoginWithPlayFabRequest { Username = username, TitleId = TitleId, Password = PassWord };
        PlayFabClientAPI.LoginWithPlayFab(testCreation, OnLoginSuccess, OnLoginFailure);
        //AchievementManager.GetSingleton();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        var dataRequest = new GetUserDataRequest();
        dataRequest.PlayFabId = result.PlayFabId;
        dataRequest.Keys = new List<string>();
        dataRequest.Keys.Add("UnlockedAchievements");
        PlayFabClientAPI.GetUserData(dataRequest, OnGetDataSuccess, OnGetDataFailure);
    }

    private void OnGetDataSuccess(GetUserDataResult result)
    {
        Debug.Log(result.Data["UnlockedAchievements"].Value);
    }

    private void OnGetDataFailure(PlayFabError error)
    {
        Debug.Log("PB de GetData");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
