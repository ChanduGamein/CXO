using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class IntroSceneManager : MonoBehaviour
{
    public static IntroSceneManager instance = null;
    public GameObject ParticipantUIPrefab,HorizontalUIPrefab;
    public GameObject VerticalGrp,HorizontalPrefabObject;
    public List<GameObject> ParticipantList;
    public Button ChooseCategoryBtn;
    public Sprite CrossSprite;
    public int PlayerLeftCOunter;
    public Button[] CategoryBtns;
    public int categoryNumber;
    [SerializeField] TextMeshProUGUI CategoryName;
    [SerializeField] Button StartGameBtn;
    [SerializeField] GameObject AwaitingParticipantsGrp, Categoriesgrp;
    public GameObject UserText;
    public TMP_InputField[] UserFields;
    public TextMeshProUGUI UserNameText;
    [SerializeField] Button NextButton;
    public string[] playerNames;
    public int[] playerScores,Timevalues;
    public int ScoreValue, TimeValue;
    public Sprite[] BgImages;
    public Image MainBG;
    [SerializeField] GameObject AdminScreen1,MainGrp,CategoryGrp,UserCanvas,CategoryGrp_User,CategoryMangers,TotalScoreGrp_User;

    PhotonView photonView;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);

        playerNames = new string[12];
        ChooseCategoryBtn.onClick.AddListener(()=> CategoryBtnOptions());
        CategoryBtns[0].onClick.AddListener(() => SetCategory(0, CategoryBtns[0].gameObject));
        CategoryBtns[1].onClick.AddListener(() => SetCategory(1, CategoryBtns[1].gameObject));
        CategoryBtns[2].onClick.AddListener(() => SetCategory(2, CategoryBtns[2].gameObject));
        CategoryBtns[3].onClick.AddListener(() => SetCategory(3, CategoryBtns[3].gameObject));
        CategoryBtns[4].onClick.AddListener(() => SetCategory(4, CategoryBtns[4].gameObject));
        CategoryBtns[5].onClick.AddListener(() => SetCategory(5, CategoryBtns[5].gameObject));
        CategoryBtns[6].onClick.AddListener(() => SetCategory(6, CategoryBtns[6].gameObject));

        UserFields[0].onValueChanged.AddListener(delegate { UsernameUpdate(); });
        UserFields[2].onValueChanged.AddListener(delegate { ScoreUpdate(); });
        UserFields[3].onValueChanged.AddListener(delegate { TimeUpdate(); });
        for (int i=0;i< UserFields.Length;i++)
        {
            UserFields[i].onValueChanged.AddListener(delegate { FieldsCheck(); });

        }
    }

    void UsernameUpdate()
    {
        UserNameText.text = UserFields[0].text;
    }
    void ScoreUpdate()
    {
        ScoreValue =(int)float.Parse(UserFields[2].text);
    }
    void TimeUpdate()
    {
        TimeValue =(int)float.Parse(UserFields[3].text);
    }
     
    void  FieldsCheck()
    {
        if (UserFields[0].text.Length>0&&UserFields[1].text.Length>0&&UserFields[2].text.Length>0&&UserFields[3].text.Length>0&&UserFields[4].text.Length>0)
        {
            NextButton.interactable = true;
        }
        else
        {
            NextButton.interactable = false;
        }
    }
    private void Update()
    {
        
    }
    void CategoryBtnOptions()
    {
        PunManager.instance.CloseRoomForplayers();
        AwaitingParticipantsGrp.SetActive(false);
            Categoriesgrp.SetActive(true);
    }
    void SetCategory(int i,GameObject B)
    {
        categoryNumber = i;
        StatusManger.instance.CategoryNumber = categoryNumber;
        CategoryName.text = B.name;
        StartGameBtn.interactable = true;
    }
    public void Applycategory()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MainBG.sprite = BgImages[categoryNumber];
            AdminScreen1.SetActive(false);
            CategoryGrp.SetActive(false);
            MainGrp.SetActive(true);
            Questions_Start();
        }
      
    }
    public void Questions_Start()
    {
        StatusManger.instance.QuestionTotal++;
        StatusManger.instance.QuestionTotalText.text = StatusManger.instance.QuestionTotal.ToString();
        StatusManger.instance.TimerStart();
        photonView.RPC("StartGame", RpcTarget.All);
    }
    [PunRPC]
    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            UserCanvas.SetActive(false);
            foreach (Transform T in CategoryGrp_User.transform)
            {
                T.gameObject.SetActive(false);
            }
            CategoryGrp_User.transform.GetChild(StatusManger.instance.CategoryNumber).gameObject.SetActive(true);
        }
        foreach (Transform T in CategoryMangers.transform)
        {
            T.gameObject.SetActive(false);
        }
        CategoryMangers.transform.GetChild(StatusManger.instance.CategoryNumber).gameObject.SetActive(true);
        
    }

    public void ResetManagersGrp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            foreach (Transform T in CategoryGrp_User.transform)
            {
                T.gameObject.SetActive(false);
            }
            foreach (Transform T in CategoryMangers.transform)
            {
                T.gameObject.SetActive(false);
            }

            TotalScoreGrp_User.transform.GetChild(StatusManger.instance.CategoryNumber).gameObject.SetActive(true);
        }
      

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
