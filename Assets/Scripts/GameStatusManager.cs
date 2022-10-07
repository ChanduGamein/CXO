using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class GameStatusManager : MonoBehaviour, IPunObservable
{
   // public static GameStatusManager instance = null;
  
    public int QuestionIndex;
    public int ScoreValue, TimeValue;

    public string playerName;
    public int PlayerNumber;
    PhotonView photonView;
    [SerializeField] float r, g, b;
    public string PlayerName;
    public int Playernumber, TotalTimeValue;
    public bool playerJoined;
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
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(playerName);
        //    stream.SendNext(PlayerNumber);

        //}
        //else
        //{
        //    playerName = (string)stream.ReceiveNext();
        //    PlayerNumber = (int)stream.ReceiveNext();
        //}
    }
    private void Update()
    {


    }
    public void UpdateValues()
    {
        if (photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
          
            

               
                playerJoined = true;
                PlayerName = IntroSceneManager.instance.UserNameText.text;// +" "+ IntroSceneManager.instance.UserFields[1].text;

                ScoreValue = IntroSceneManager.instance.ScoreValue;
                if (IntroSceneManager.instance.TimeValue <= 45) TimeValue = IntroSceneManager.instance.TimeValue;
                TotalTimeValue = IntroSceneManager.instance.TotalTimeValue;
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.IsLocal)
                    {
                        Playernumber = p.ActorNumber;
                    }
                }
                PlayerName = IntroSceneManager.instance.UserNameText.text;// +" "+ IntroSceneManager.instance.UserFields[1].text;

                //ScoreValue = IntroSceneManager.instance.ScoreValue;
                //if (IntroSceneManager.instance.TimeValue <= 45) TimeValue = IntroSceneManager.instance.TimeValue;
                //TotalTimeValue = IntroSceneManager.instance.TotalTimeValue;
                photonView.RPC("changeColour", RpcTarget.MasterClient, Playernumber, PlayerName, ScoreValue, TimeValue, TotalTimeValue);
            
        }
    }
    [PunRPC]
    void changeColour(int Playernumber, string playername,int scoreValue,int timeValue, int totalTimeValue)
    {

        IntroSceneManager.instance.playerNames[Playernumber] = playername;
        IntroSceneManager.instance.playerScores[Playernumber] = scoreValue;
        IntroSceneManager.instance.QuestionTimevalues[Playernumber] = timeValue;
        IntroSceneManager.instance.TotalTimevalues[Playernumber] = totalTimeValue;

        
            if (Playernumber % 2 == 0)
            {
                IntroSceneManager.instance.VerticalGrp.transform.GetChild((Playernumber / 2) - 1).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playername;

                IntroSceneManager.instance.VerticalGrp.transform.GetChild((Playernumber / 2) - 1).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                IntroSceneManager.instance.VerticalGrp.transform.GetChild((Playernumber / 2) - 1).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = timeValue.ToString() + " sec";
            }
            else
            {
                // if (playernumber != 1)
                {
                    IntroSceneManager.instance.VerticalGrp.transform.GetChild((int)(Playernumber / 2) - 1).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playername;

                    IntroSceneManager.instance.VerticalGrp.transform.GetChild((int)(Playernumber / 2) - 1).transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
                    IntroSceneManager.instance.VerticalGrp.transform.GetChild((int)(Playernumber / 2) - 1).transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = timeValue.ToString() + " sec";

                }

            }
       


    }

}
