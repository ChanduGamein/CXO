using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameStatusManager : MonoBehaviour, IPunObservable
{
   // public static GameStatusManager instance = null;
  
    public int QuestionIndex;
    public int ScoreValue, TimeValue;

    public string playerName;
    public int PlayerNumber;
    PhotonView photonView;
    
    private void Awake()
    {
      
    }
    private void Start()
    {
        photonView = PhotonView.Get(this);
        if (!photonView.IsMine)//is not local player
        {
            return;
        }
        playerName = IntroSceneManager.instance.UserNameText.text;
      
       // photonView.RPC("PlayerStatusSetup", RpcTarget.MasterClient, playerName);
    }
   //[PunRPC]
   // void PlayerStatusSetup(string playerName)
   // {
   //     photonView.RPC("SyncPlayerStatus", RpcTarget.All, playerName);
      
   // }
   // [PunRPC]
   // void SyncPlayerStatus(string playerName)
   // {
   //     IntroSceneManager.instance.playerNames[PunManager.instance.PlayerCount-1] = playerName;
   //     IntroSceneManager.instance.ParticipantList[PunManager.instance.PlayerCount-1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerName;
   // }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);
            stream.SendNext(PlayerNumber);

        }
        else
        {
            playerName = (string)stream.ReceiveNext();
            PlayerNumber = (int)stream.ReceiveNext();
        }
    }

   
}
