using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;
public class ApiManager : MonoBehaviour
{
    public static ApiManager instance = null;

    string urlRequest = "https://50.17.146.144/api/user/detail/save";
    string urlRequest2 = "https://rabbithole-blockchain-api-dev.azurewebsites.net/admintRbhtlTransfer/createMintTransfers";

    public UserRegisterDataAPI userRegisterDataAPI;
    public UserRegisterDataAPI2 userRegisterDataAPI2;
    public APIResponseAllocator responseHandler;
    string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjp7ImlkIjoiNjJmZDAyYWE1YzkyMWRiNWExZjZmZWE4Iiwicm9sZSI6MX0sImlhdCI6MTY2NDUzOTczNiwiZXhwIjoxNjY3MTMxNzM2fQ.uRb04LUYhZV6a1nPWrGJ6o0hhGSJgXDKZKjzf71DTmY";

    public GameObject Middle1,Middle2, PunManagerObject;
    public TextMeshProUGUI ResponseText;
    public RaiseEvent raiseEvent;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }
    private void Start()
    {

       
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    // StartCoroutine(SetItem());
        //    User_Register_API("TestingFirstName", "TestinglastName", "Testingdesignation", "TestingcompanyName", "jitendra3.agrawal@w3villa.com", 50, 20, 2);
        //    Debug.Log("API");
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    // StartCoroutine(GetItem());
        //    User_Register_API2("0x8E713ADe8C8a8b40A39023be3a51fB111CF8ceB9", 5);
        //}



    }
    public void User_Register_API(string firstName, string lastName, string designation, string companyName, string email,int totalScore,int totalTime,int rank)
    {
        userRegisterDataAPI.firstName = firstName;
        userRegisterDataAPI.lastName = lastName;
        userRegisterDataAPI.designation = designation;
        userRegisterDataAPI.companyName = companyName;
        userRegisterDataAPI.email = email;
        userRegisterDataAPI.totalScore = totalScore;
        userRegisterDataAPI.totalTime = totalTime;
        userRegisterDataAPI.rank = rank;

     

        APIRequest(urlRequest, null, userRegisterDataAPI, OnUserRegisterSuccess);

      
    }
     public void User_Register_API2(string firstName, string lastName, string designation, string companyName, string email, int totalScore, int totalTime, int rank)
    {
        userRegisterDataAPI.firstName = firstName;
        userRegisterDataAPI.lastName = lastName;
        userRegisterDataAPI.designation = designation;
        userRegisterDataAPI.companyName = companyName;
        userRegisterDataAPI.email = email;
        userRegisterDataAPI.totalScore = totalScore;
        userRegisterDataAPI.totalTime = totalTime;
        userRegisterDataAPI.rank = rank;



        APIRequest(urlRequest, null, userRegisterDataAPI, OnUserRegisterSuccess2);


       // APIRequest(urlRequest2, token, userRegisterDataAPI2, OnUserRegisterSuccess2);

      
    }

     
    public void UpdateUserValues()
    {
       User_Register_API(IntroSceneManager.instance.UserFields[0].text, IntroSceneManager.instance.UserFields[1].text, IntroSceneManager.instance.UserFields[2].text, IntroSceneManager.instance.UserFields[3].text, IntroSceneManager.instance.UserFields[4].text, 50, 20, 2);

    }
    public void UpdateUserValues_Finalscores(int score,int time)
    {
        User_Register_API(IntroSceneManager.instance.UserFields[0].text, IntroSceneManager.instance.UserFields[1].text, IntroSceneManager.instance.UserFields[2].text, IntroSceneManager.instance.UserFields[3].text, IntroSceneManager.instance.UserFields[4].text, score, time, 0);

    }

    public static void APIRequest(string APIurl, string authToken, object formData, Action<string, string> OnSuccess)
    {
        string url;

      
        {
            url = APIurl;

        }
       

        if (instance == null) return;

        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            if (formData != null)
            {
                Debug.Log(formData);
                using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(formData)))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("Authorization", authToken);
                        request.SetRequestHeader("Content-Type", "application/json");
                    }
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(formData));
                    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

                  
                    request.SetRequestHeader("Content-Type", "application/json");
                    yield return request.SendWebRequest();
                    while (!request.isDone) yield return null;


                    if (true)
                    {
                        //if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                        //{
                        //    //HelperUtil.HideLoading();
                        //    //HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                        //    //{
                        //    //    UIManager.instance.ActivatePanel(ScreenData.ScreensName.LoginPanel);
                        //    //    instance.StopAllCoroutines();
                        //    //}));
                        //}
                        //else if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                        //{
                        //    instance.StopAllCoroutines();
                        //    UIManager.instance.ActivatePanel(ScreenData.ScreensName.LoginPanel);
                        //}));

                         OnSuccess(request.downloadHandler.text, request.responseCode.ToString());

                    }

                }
            }

            if (formData == null)
            {
                //if (showLoading)
                //{
                //   // HelperUtil.ShowLoading();
                //}
                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("Authorization", authToken);
                        request.SetRequestHeader("Accept", "application/json");
                        request.SetRequestHeader("Content-Type", "application/json");

                    }
                    yield return request.SendWebRequest();
                  //  if (showLoading) HelperUtil.HideLoading();
                    while (!request.isDone) yield return null;
                   
                    //else if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong);
                    //else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());
                }
            }
        }

    }



    [Serializable]
    public class UserRegisterDataAPI
    {
        public string firstName;
        public string lastName;
        public string designation;
        public string companyName;
        public string email;
        public int totalScore;
        public int totalTime;
        public int rank;
      
    }
    void OnUserRegisterSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.registerResponse);
        Debug.Log(response);
      
        if (responseCode=="200")
        {
            if (responseHandler.registerResponse.success)
            {
                Middle2.SetActive(true); PunManagerObject.SetActive(true);
                Middle1.SetActive(false);
                Debug.Log("OnUserRegisterSuccess");
               // raiseEvent.UpdateAfterDelay();
            }
            else
            {
                ResponseText.transform.parent.gameObject.SetActive(true);
                ResponseText.text = responseHandler.registerResponse.message;
            }
        }

    }

     [Serializable]
    public class UserRegisterDataAPI2
    {
      
        public string recieverAccount;
        public int recieverAmount;
        
      
    }
    void OnUserRegisterSuccess2(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.registerResponse);
        Debug.Log(response);
      

    }

   

    //IEnumerator Post(string url, object bodyJsonString)
    //{
    //    var request = new UnityWebRequest(url, "POST");
    //    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(bodyJsonString));
    //    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
    //   // request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //    request.SetRequestHeader("Content-Type", "application/json");
    //    yield return request.SendWebRequest();
    //    Debug.Log("Status Code: " + request.responseCode);
    //    if (request.responseCode==200)
    //    {
    //        Debug.Log("OnUserRegisterSuccess");
    //    }
    //}






}
