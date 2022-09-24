using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEvent : MonoBehaviourPun {
   public bool playerJoined;
    public string PlayerName;
    public int ScoreValue,TimeValue, Playernumber;
    private const byte Client_CHANGE_EVENT = 0;
  
    private void Update() {
        if (!PhotonNetwork.IsMasterClient && !playerJoined) {
            ChangeClientview();
            Debug.Log("test");
        } 
    }

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
            playerJoined = b;
            ScoreValue = scoreValue;
            TimeValue = timeValue;
            PlayerName = playername;
            Playernumber = playernumber;

           
        }
    }

  
    private void ChangeClientview() {

        playerJoined = true;
     //   ScoreValue = GameStatusManager.instance.ScoreValue;
      // TimeValue = GameStatusManager.instance.TimeValue;
        object[] datas = new object[] { playerJoined, ScoreValue, TimeValue, PlayerName, Playernumber };

        PhotonNetwork.RaiseEvent(Client_CHANGE_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
  
}