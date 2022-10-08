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
    public InputField[] UserFields;
    public TextMeshProUGUI UserNameText;
    [SerializeField] Button NextButton;
    public string[] playerNames;
    public int[] playerScores,TotalTimevalues,QuestionTimevalues;
    public int ScoreValue, TimeValue, TotalTimeValue;
    public Sprite[] BgImages,CategoryLogoSprites,LeaderBoardSprites,LeaderBoardRankHolders;
    public Image MainBG,CategoryLogoImage;
    [SerializeField] GameObject AdminScreen1,MainGrp,CategoryGrp,UserCanvas,CategoryGrp_User,CategoryMangers,TotalScoreGrp_User;
    public RaiseEvent RaiseEventObject;
    public TextMeshProUGUI UserScoreText, UserTimeText;
    public TextMeshProUGUI[] UserScoreTextGrp, UserTimeTextGrp;
    public int Timeleft;
    public Button NextBtn_Admin;
    public Button[] SubmitBtnsGrp;
    public bool AnswerOnlyOnce;
    public TextMeshProUGUI MaxQuestiontext_Admin;
    public GameObject PunMangerObject,LeaderBoardGrp,MainBG_holder,CategoryLogo,HorizontalGrp1, HorizontalGrp2;
    public TextMeshProUGUI[] HoldersTotalQuestionvalText;
    public GameObject MainCanvas;
    PhotonView photonView;
    public List<int> QuestionLength;




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
      
        QuestionLength = new List<int>();
        for (int i=1;i<=7;i++)
        {
            string s= "Questions"+i.ToString();
            QuestionLength.Add(Resources.LoadAll(s).Length);
            HoldersTotalQuestionvalText[i - 1].text ="/"+ QuestionLength[i-1].ToString();

        }
        ChooseCategoryBtn.onClick.AddListener(()=> CategoryBtnOptions());
        CategoryBtns[0].onClick.AddListener(() => SetCategory(0, QuestionLength[0], CategoryBtns[0].gameObject));
        CategoryBtns[1].onClick.AddListener(() => SetCategory(1, QuestionLength[1], CategoryBtns[1].gameObject));
        CategoryBtns[2].onClick.AddListener(() => SetCategory(2, QuestionLength[2], CategoryBtns[2].gameObject));
        CategoryBtns[3].onClick.AddListener(() => SetCategory(3, QuestionLength[3], CategoryBtns[3].gameObject));
        CategoryBtns[4].onClick.AddListener(() => SetCategory(4, QuestionLength[4], CategoryBtns[4].gameObject));
        CategoryBtns[5].onClick.AddListener(() => SetCategory(5, QuestionLength[5], CategoryBtns[5].gameObject));
        CategoryBtns[6].onClick.AddListener(() => SetCategory(6, QuestionLength[6], CategoryBtns[6].gameObject));


        NextBtn_Admin.onClick.AddListener(()=> NextQuestion());

       // UserFields[0].onValueChanged.AddListener(delegate { UsernameUpdate(); });
      //  UserFields[2].onValueChanged.AddListener(delegate { ScoreUpdate(); });
       // UserFields[3].onValueChanged.AddListener(delegate { TimeUpdate(); });
        for (int i=0;i< UserFields.Length;i++)
        {
            UserFields[i].onValueChanged.AddListener(delegate { FieldsCheck(); });

        }

      
        
       
    }

    void UsernameUpdate()
    {
        UserNameText.text = UserFields[0].text+" "+ UserFields[1].text;// UserFields[0].text;
    }
    //void ScoreUpdate()
    //{
    //    ScoreValue =(int)float.Parse(UserFields[2].text);
    //}
    //void TimeUpdate()
    //{
    //    TimeValue =(int)float.Parse(UserFields[3].text);
    //}
     
    void  FieldsCheck()
    {
        if (UserFields[0].text.Length > 0 && UserFields[1].text.Length > 0 && UserFields[2].text.Length > 0 && UserFields[3].text.Length > 0 && UserFields[4].text.Length > 0)
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
        UsernameUpdate();

       ///// if (!NextButton.interactable)
       // {
       //     if (UserFields[0].GetComponent<TextChecker>().TextFilled && UserFields[1].GetComponent<TextChecker>().TextFilled &&
       //         UserFields[2].GetComponent<TextChecker>().TextFilled && UserFields[3].GetComponent<TextChecker>().TextFilled &&
       //         UserFields[4].GetComponent<TextChecker>().TextFilled)
       //     {
       //         NextButton.interactable = true;
       //     }
       //     else
       //     {
       //         NextButton.interactable = false;
       //     }
       // }
      
    }
   public void CategoryBtnOptions()
    {
        PunManager.instance.CloseRoomForplayers();
        AwaitingParticipantsGrp.SetActive(false);
            Categoriesgrp.SetActive(true);
    }
    void SetCategory(int i, int j,GameObject B)
    {
        categoryNumber = i;
        StatusManger.instance.CategoryNumber = categoryNumber;
        StatusManger.instance.MaxQuestions = j;
        MaxQuestiontext_Admin.text = "/ "+j.ToString();
        CategoryName.text = B.name;
        StartGameBtn.interactable = true;
    }
    public void Applycategory()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MainBG.sprite = BgImages[categoryNumber];
            CategoryLogoImage.sprite = CategoryLogoSprites[categoryNumber];
            AdminScreen1.SetActive(false);
            CategoryGrp.SetActive(false);
            AwaitingParticipantsGrp.SetActive(true);
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
    bool savedonce;
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
            UserScoreText.text =ScoreValue.ToString();
            UserTimeText.text = TotalTimeValue.ToString();
            if (!savedonce) { 
                ApiManager.instance.UpdateUserValues_Finalscores(ScoreValue, TotalTimeValue);
                savedonce = true;
            }
            for (int i=0;i< UserScoreTextGrp.Length;i++)
            {
                UserScoreTextGrp[i].text = ScoreValue.ToString();
                UserTimeTextGrp[i].text = TotalTimeValue.ToString();
            }
        }
      

    }
    void NextQuestion()
    {
        NextBtn_Admin.interactable = false;
          
        Questions_Start();

        StartCoroutine(delaystartNextQuestions());
    }
    IEnumerator delaystartNextQuestions()
    {
        yield return new WaitForSeconds(2f);
        NextBtn_Admin.interactable = true;
    }
    public void AssignSprites()
    {
        foreach (Transform T in HorizontalGrp1.transform)
        {
            T.gameObject.SetActive(false);
        }
        foreach (Transform T in HorizontalGrp2.transform)
        {
            T.gameObject.SetActive(false);
        }
        LeaderBoardGrp.SetActive(true);
        MainBG_holder.GetComponent<Image>().sprite =LeaderBoardSprites[categoryNumber];
        CategoryLogo.transform.GetChild(categoryNumber).gameObject.SetActive(true);
        if (StatusManger.instance.MainGrpList.Count>0)
        {
            for (int i = 0; i < StatusManger.instance.MainGrpList.Count; i++)
            {
                if (i<=4)
                {
                    HorizontalGrp1.SetActive(true);
                    HorizontalGrp1.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = LeaderBoardRankHolders[categoryNumber];
                    HorizontalGrp1.transform.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerNames[StatusManger.instance.MainGrpList[i]]; 
                    HorizontalGrp1.transform.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = playerScores[StatusManger.instance.MainGrpList[i]].ToString(); 
                    HorizontalGrp1.transform.GetChild(i).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = TotalTimevalues[StatusManger.instance.MainGrpList[i]].ToString();
                    HorizontalGrp1.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    HorizontalGrp2.SetActive(true);
                    HorizontalGrp2.transform.GetChild(i-5).transform.GetChild(0).GetComponent<Image>().sprite = LeaderBoardRankHolders[categoryNumber];
                    HorizontalGrp2.transform.GetChild(i-5).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerNames[StatusManger.instance.MainGrpList[i]];
                    HorizontalGrp2.transform.GetChild(i-5).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = playerScores[StatusManger.instance.MainGrpList[i]].ToString();
                    HorizontalGrp2.transform.GetChild(i-5).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = TotalTimevalues[StatusManger.instance.MainGrpList[i]].ToString();
                    HorizontalGrp1.transform.GetChild(i-5).gameObject.SetActive(true);

                }
            }
        }
       
       
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
