using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System;

public class PunManagertemp : MonoBehaviourPunCallbacks {
    string version = "1";
    [SerializeField]
    TextMeshProUGUI RoomNameText;
    //  [SerializeField] Button JoinRoom;
    [SerializeField] private int i;
    public object TyppedLobby { get; private set; }

    private string[] teamMembersUserIds; private string[] expectedUsers;
    private TypedLobby typedLobby;
    private RoomOptions roomOptions;

    public static PunManagertemp instance = null;
    [SerializeField] public string _roomName;
    [SerializeField] GameObject LoadingTextObject, InitialUIObject, PlayerObject, PlayerPrefab;
    [SerializeField] TMP_InputField textfield;
    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (instance == null) instance = this;
    }
    void Start() {
        PhotonNetwork.GameVersion = this.version;
        PhotonNetwork.ConnectUsingSettings();
        PhotonView photonView = PhotonView.Get(this);





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
        LoadingTextObject.SetActive(false); InitialUIObject.SetActive(true);
    }


    public override void OnJoinedRoom() {
        Debug.Log("OnJoinedRoom");
        i++;
        var x = UnityEngine.Random.Range(-2,2);
        Debug.Log(x);
        PlayerObject=PhotonNetwork.Instantiate(PlayerPrefab.name,new Vector3(x,PlayerPrefab.transform.position.y,PlayerPrefab.transform.position.z), PlayerPrefab.transform.rotation, 0);
        //MainCam.transform.SetParent(PlayerObject.transform);
        //MainCam.transform.localPosition = new Vector3(0, 1f, -1f);
      //  ExmpleObjct.SetActive(true); ; HoldBtnObj.SetActive(true);// CamTemp.SetActive(false);
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
        Debug.Log("OnLeftRoom"); countofPlayers(); i--;
    }
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {
        Debug.Log("OnLobbyStatisticsUpdate"); Debug.Log(PhotonNetwork.CountOfRooms.ToString());
    }
    public override void OnMasterClientSwitched(Player newMasterClient) {
        Debug.Log("OnMasterClientSwitched");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("OnPlayerEnteredRoom");
        i++;
       // ExmpleObjct.SetActive(true); ; HoldBtnObj.SetActive(true);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log("OnPlayerLeftRoom"); countofPlayers();

    }
    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps) {
        Debug.Log("OnPlayerPropertiesUpdate");
    }
    public override void OnRegionListReceived(RegionHandler regionHandler) {
        Debug.Log("OnRegionListReceived");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        Debug.Log("OnRoomListUpdate");      //  Debug.Log(roomList[0]);                      Debug.Log(roomList.Count);        Debug.Log("rooms" + roomList.Count);
        foreach (RoomInfo r in roomList) {
            Debug.Log("Name : " + r.Name + "    Countr of player " + r.PlayerCount + "     " + r.MaxPlayers);
            InitialUIObject.SetActive(false);
            // GameObject go = Instantiate(buttonprefab) as GameObject;
            //go.transform.parent = Panel.transform;    
            RoomNameText.text = r.Name;

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
        {
            // GamePanel.SetActive(true);
            RoomNameText.text = _roomName;
            PhotonNetwork.CreateRoom(RoomNameText.text, roomOptions, typedLobby, teamMembersUserIds);

            //PhotonNetwork.CurrentRoom.IsOpen = false;
            //  PhotonNetwork.CurrentRoom.IsVisible = false;
            //  PhotonNetwork.AutomaticallySyncScene = true;
            // PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void CustomJoinRoom() {
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinRoom(RoomNameText.text, expectedUsers);
        }



    }

    public void TexfieldInput()
    {
        _roomName = textfield.text;
    }






}
