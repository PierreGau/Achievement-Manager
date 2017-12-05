using PlayFab;
using UnityEngine;
using PlayFab.ServerModels;
using PlayFab.ClientModels;

public class LeTest : MonoBehaviour
{
    bool datasLoaded = false;
    string datas = "none";
    public void Start()
    {
        PlayFabSettings.TitleId = "96F3"; // Please change this value to your own titleId from PlayFab Game Manager
        //SetTitleData();
        var testCreation = new LoginWithPlayFabRequest { Username = "Hudnel1664", TitleId = "96F3", Password = "12test12"};
        //var request = new LoginWithPlayFabRequest { Password = "yosh", TitleId = "96F3" , Username =  "Enel1664" };
        //PlayFabClientAPI.LoginWithPlayFab(testCreation, OnLoginSuccess, OnLoginFailure);  

        GetTitleInternalData();
    }

    public void Update()
    {
        if(datasLoaded)
        {
            datas += "Done";
            Debug.Log("Loaded Datas : " + datas);
            datasLoaded = false;
        }

        if (Input.GetKey(KeyCode.D) ^ Input.GetKey(KeyCode.Q))
        {
            Debug.Log("TaRace");
        }
    }

    public void SetTitleData()
    {
        var updateRequest = new SetTitleDataRequest();
        updateRequest.Key = "Achievements";
        updateRequest.Value = "well, not so cool";
        PlayFabServerAPI.SetTitleData(updateRequest, (result) =>
        {
            Debug.Log("Set titleData successful");
        },
        (error) =>
        {
            Debug.Log("Got error setting titleData:");
            Debug.Log(error.ErrorMessage);
        });
    }

    public void GetTitleInternalData()
    {
        datasLoaded = false;
        var getRequest = new PlayFab.ServerModels.GetTitleDataRequest();
        PlayFabServerAPI.GetTitleData   (getRequest, (result) => {
            foreach (var entry in result.Data)
            {
                if(string.Compare(entry.Key,"Achievements") == 0)
                {
                    //Debug.Log("Got the following titleData:");
                    //Debug.Log(entry.Key + ": " + entry.Value);
                    datas = entry.Key + entry.Value;
                    datasLoaded = true;
                }
                else
                {
                    Debug.Log("No achievements in database");
                }
            }       
        },
        (error) => {
            Debug.Log("Got error getting titleData:");
            Debug.Log(error.ErrorMessage);
            
        });

        
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        GetTitleInternalData();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    //private void OnRegisterSuccess(LoginResult result)
    //{
    //    Debug.Log("Congratulations, you made your first successful API call!");
    //}

    //private void OnRegisterFailure(PlayFabError error)
    //{
    //    Debug.LogWarning("Something went wrong with your first API call.  :(");
    //    Debug.LogError("Here's some debug information:");
    //    Debug.LogError(error.GenerateErrorReport());
    //}
}