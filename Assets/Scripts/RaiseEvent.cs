using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RaiseEvent : MonoBehaviourPun {
   public bool playerJoined;
    public string PlayerName;
    public int ScoreValue,TimeValue, Playernumber,TotalTimeValue;
    private const byte Client_CHANGE_EVENT = 0;
  
    //private void Update() {
    //    if (!PhotonNetwork.IsMasterClient && !playerJoined) {
    //        ChangeClientview();
    //        Debug.Log("test");
    //    } 
    //}

    private void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
      

    }

    private void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj) {
        if (obj.Code == Client_CHANGE_EVENT) {
            object[] datas = (object[])obj.CustomData;
          
           bool b= (bool)datas[0];
           int scoreValue = (int)datas[1];
           int timeValue = (int)datas[2];
           string playername = (string)datas[3];
           int playernumber = (int)datas[4];
           int totalTimeValue = (int)datas[5];
            playerJoined = b;
            ScoreValue = scoreValue;
            TimeValue = timeValue;
            PlayerName = playername;
            Playernumber = playernumber;
            TotalTimeValue = totalTimeValue;
           
            IntroSceneManager.instance.playerNames[Playernumber] = playername;
            IntroSceneManager.instance.playerScores[Playernumber] = scoreValue;
            IntroSceneManager.instance.QuestionTimevalues[Playernumber] = timeValue;
            IntroSceneManager.instance.TotalTimevalues[Playernumber] = totalTimeValue;
            Debug.Log(playernumber);
           
            {
                if (Playernumber % 2 == 0)
                {
                    IntroSceneManager.instance.VerticalGrp.transform.GetChild((playernumber / 2) - 1).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                    IntroSceneManager.instance.VerticalGrp.transform.GetChild((playernumber / 2) - 1).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = timeValue.ToString() + " sec";
                }
                else
                {
                   // if (playernumber != 1)
                    {
                        IntroSceneManager.instance.VerticalGrp.transform.GetChild((int)(playernumber / 2) - 1).transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
                        IntroSceneManager.instance.VerticalGrp.transform.GetChild((int)(playernumber / 2) - 1).transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = timeValue.ToString() + " sec";

                    }

                }
            }
         
        }
    }

    public void UpdateAfterDelay()
    {
      //  if (!PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(_updateValues());

        }
    }
  
    IEnumerator _updateValues()
    {
       
        yield return new WaitForSeconds(4f);
        UpdateValues();
    }
    public void UpdateValues() {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsLocal)
            {
                Playernumber = p.ActorNumber;
            }            
        }
        playerJoined = true;
        PlayerName = IntroSceneManager.instance.UserNameText.text;// +" "+ IntroSceneManager.instance.UserFields[1].text;
       
           ScoreValue = IntroSceneManager.instance.ScoreValue;
         TimeValue = IntroSceneManager.instance.TimeValue;
        TotalTimeValue = IntroSceneManager.instance.TotalTimeValue;
        object[] datas = new object[] { playerJoined, ScoreValue, TimeValue, PlayerName, Playernumber, TotalTimeValue };

        PhotonNetwork.RaiseEvent(Client_CHANGE_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
  
}