using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System;

public class PunManager : MonoBehaviourPunCallbacks {
    string version = "1";
   // [SerializeField]    TextMeshProUGUI RoomNameText;
    //  [SerializeField] Button JoinRoom;
    [SerializeField] public int PlayerCount;
    public object TyppedLobby { get; private set; }

    private string[] teamMembersUserIds; private string[] expectedUsers;
    private TypedLobby typedLobby;
    private RoomOptions roomOptions;

    public static PunManager instance = null;
    [SerializeField] GameObject PlayerObject, PlayerPrefab;
    public String Room_Name;
    [SerializeField] bool IsAdmin;
    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (instance == null) instance = this;
    }
    void Start() {
        PhotonNetwork.GameVersion = this.version;
        PhotonNetwork.ConnectUsingSettings();
        PhotonView photonView = PhotonView.Get(this);




    }

    private void Update() {

        // Debug.Log(DummyObject.transform.localPosition.x);
        // cam.GetComponent<D2FogsPE>().Density = DummyObject.transform.localPosition.x * 2f;
        // sliderval.text = (DummyObject.transform.localPosition.x * 2f).ToString();

        if (PhotonNetwork.IsMasterClient) {
            //if (PlayerObject != null) {
            //   // Debug.Log(PlayerObject.GetComponent<PlayerScript>().PlayerName);
            //   // PlayerObject.GetComponent<PlayerScript>().PlayerName = DataStorage.GetComponent<DataStorage>()._UserNameVal;

            //}
        }
    }
    private bool IsNullOrEmpty(string text) {
        throw new NotImplementedException();
    }

    public void ControlMaster() {
        //PhotonNetwork.SetMasterClient(PhotonNetwork.playerList[1]);
        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        Debug.Log("done");

    }


    public override void OnConnected() {
        Debug.Log("OnConnected");

    }

    public override void OnConnectedToMaster() {
        Debug.Log("OnConnectedToMaster");
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinLobby();           // CreateRoom.interactable = true;

        }
    }
    public override void OnCreatedRoom() {
        Debug.Log("OnCreatedRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("OnCreateRoomFailed");
    }
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("OnDisconnected");
    }
    public override void OnFriendListUpdate(List<FriendInfo> friendList) {
        Debug.Log("OnFriendListUpdate");
    }
    public override void OnJoinedLobby() {
        Debug.Log("OnJoinedLobby");
    }


    public override void OnJoinedRoom() {
        Debug.Log("OnJoinedRoom");
        PlayerCount++;

        PlayerObject=PhotonNetwork.Instantiate(PlayerPrefab.name, PlayerPrefab.transform.position, PlayerPrefab.transform.rotation, 0);

        countofPlayers();
        //JoinRoom.gameObject.SetActive(false);

        //  photonView.RPC("RPC_Join", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
    }
    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("OnJoinRandomFailed");

    }
    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("OnJoinRoomFailed");
    }
    public override void OnLeftLobby() {
        Debug.Log("OnLeftLobby");
    }
    public override void OnLeftRoom() {
        Debug.Log("OnLeftRoom"); countofPlayers(); PlayerCount--;
    }
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {
        Debug.Log("OnLobbyStatisticsUpdate"); Debug.Log(PhotonNetwork.CountOfRooms.ToString());
    }
    public override void OnMasterClientSwitched(Player newMasterClient) {
        Debug.Log("OnMasterClientSwitched");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("OnPlayerEnteredRoom"); //nextPlayer.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        Debug.Log(newPlayer.ActorNumber);
        PlayerCount++;
        if (newPlayer.ActorNumber>1)
        {
            IntroSceneManager.instance.VerticalGrp.transform.parent.GetComponent<Image>().enabled = false;
            if (newPlayer.ActorNumber%2==0)
            {
                IntroSceneManager.instance.HorizontalPrefabObject = Instantiate(IntroSceneManager.instance.HorizontalUIPrefab, Vector3.zero, Quaternion.identity);
                IntroSceneManager.instance.HorizontalPrefabObject.transform.SetParent(IntroSceneManager.instance.VerticalGrp.transform);
                IntroSceneManager.instance.HorizontalPrefabObject.transform.localPosition = Vector3.zero;
                IntroSceneManager.instance.HorizontalPrefabObject.transform.localScale = Vector3.one;
                GameObject B = Instantiate(IntroSceneManager.instance.ParticipantUIPrefab, Vector3.zero, Quaternion.identity);
                B.transform.SetParent(IntroSceneManager.instance.HorizontalPrefabObject.transform);
                B.transform.localPosition = Vector3.zero;
                B.transform.localScale = Vector3.one;
                B.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =(newPlayer.ActorNumber - 1).ToString();
                IntroSceneManager.instance.ParticipantList.Add(B);
            }
            else
            {
                GameObject B = Instantiate(IntroSceneManager.instance.ParticipantUIPrefab, Vector3.zero, Quaternion.identity);
                B.transform.SetParent(IntroSceneManager.instance.HorizontalPrefabObject.transform);
                B.transform.localPosition = Vector3.zero;
                B.transform.localScale = Vector3.one;
                B.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (newPlayer.ActorNumber - 1).ToString();
                IntroSceneManager.instance.ParticipantList.Add(B);

            }
        }
       
        if (newPlayer.ActorNumber > 4)
        {
            IntroSceneManager.instance.ChooseCategoryBtn.interactable = true;

        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log("OnPlayerLeftRoom"); countofPlayers();
        Debug.Log(otherPlayer.ActorNumber);
        if (otherPlayer.ActorNumber > 1) {
            IntroSceneManager.instance.ParticipantList[otherPlayer.ActorNumber - 2].transform.GetChild(2).GetComponent<Image>().sprite = IntroSceneManager.instance.CrossSprite;
            IntroSceneManager.instance.PlayerLeftCOunter++;
        }
        if (IntroSceneManager.instance.ParticipantList.Count- IntroSceneManager.instance.PlayerLeftCOunter < 4)
        {
            IntroSceneManager.instance.ChooseCategoryBtn.interactable = false;
        }
    }
    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps) {
        Debug.Log("OnPlayerPropertiesUpdate");
    }
    public override void OnRegionListReceived(RegionHandler regionHandler) {
        Debug.Log("OnRegionListReceived");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        Debug.Log("OnRoomListUpdate");      //  Debug.Log(roomList[0]);                      Debug.Log(roomList.Count);        Debug.Log("rooms" + roomList.Count);
        foreach (RoomInfo r in roomList) {
            Debug.Log("Name : " + r.Name + "    Countr of player " + r.PlayerCount + "     " + r.MaxPlayers);
            // GameObject go = Instantiate(buttonprefab) as GameObject;
            //go.transform.parent = Panel.transform;     
          
            // RoomNameText.text = r.Name;

        }
        if (IsAdmin)
        {
            CustomcreateRoom();
        }
        if (roomList.Count == 1) {
            // CustomRoomname.gameObject.SetActive(false);
            //Name.transform.parent.gameObject.SetActive(false);

            // JoinRoom.gameObject.SetActive(true);
            //Debug.Log(UserNameText.text);
            CustomJoinRoom();
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
        Debug.Log("OnRoomPropertiesUpdate");
    }
    void countofPlayers() {
        //  PlayersCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();


    }
    public void CustomcreateRoom() {
       // if (DataStorage.GetComponent<DataStorage>()._TherapistNameVal.Length != 0) 
            {
            // GamePanel.SetActive(true);
            //  RoomNameText.text = "Room" + " : " + System.DateTime.Now;
            PhotonNetwork.CreateRoom(Room_Name, roomOptions, typedLobby, teamMembersUserIds);

            //PhotonNetwork.CurrentRoom.IsOpen = false;
            //  PhotonNetwork.CurrentRoom.IsVisible = false;
            //  PhotonNetwork.AutomaticallySyncScene = true;
            // PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void CustomJoinRoom() {
        //if (CustomRoomname.text.Length != 0)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 11;
            PhotonNetwork.JoinRoom(Room_Name, expectedUsers);
        }

        // PlayBtn.gameObject.SetActive(true);
        // TherapyBtn.enabled = true;



    }

    public void CloseRoomForplayers()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

    }
    public void OpenRoomForplayers()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;

    }


}
