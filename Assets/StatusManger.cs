using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusManger : MonoBehaviour, IPunObservable
{
    public static StatusManger instance = null;
    public int CategoryNumber,QuestionIndex,QuestionTotal;
    public float Timer;
    public TextMeshProUGUI QuestionText, TimerText,QuestionTotalText;
    public Image TimerImage;
    public bool StartTimer;
    [SerializeField] GameObject WinnerBG;
    bool isFinished;
    public int[] scoreList,Timelist;
    public List<int> Indexlist;
    public string[] NameList;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CategoryNumber);
            stream.SendNext(QuestionIndex);
            stream.SendNext(QuestionTotal);
            stream.SendNext(Timer);

        }
        else
        {
            CategoryNumber = (int)stream.ReceiveNext();
            QuestionIndex = (int)stream.ReceiveNext();
            QuestionTotal = (int)stream.ReceiveNext();
            Timer = (float)stream.ReceiveNext();
            if(QuestionTotal > 3) IntroSceneManager.instance.ResetManagersGrp(); 
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Timer = 45;
       
       
      //  GetWinner();
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestionTotal <= 3)
        {
            if (StartTimer && Timer > 0&& QuestionTotal <= 3)
            {

                Timer -= Time.deltaTime;
                TimerImage.fillAmount = (Timer / 45f);
                TimerText.text = ((int)Timer).ToString();
                if (Timer < 30 && Timer > 15)
                {
                    TimerImage.color = Color.yellow;
                }
                else if (Timer < 15)
                {
                    TimerImage.color = Color.red;
                }
            }
            else
            {
                if (Timer < 0)
                {
                    if (QuestionTotal <= 3)
                    {
                        IntroSceneManager.instance.Questions_Start();

                    }
                   

                }
            }
        }
        else
        {
            if (!isFinished)
            {
                DisableManagers();
                isFinished = true;
            }
          
        }
         
    }

    public void TimerStart()
    {
        Timer = 10;
        StartTimer = true;
    }
    void DisableManagers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IntroSceneManager.instance.ResetManagersGrp();
            WinnerBG.SetActive(true);
            GetWinner();

        }

    }
    public void GetWinner()
    {
        AssignList();
        int maxElement = scoreList[0];
        for (int index = 2; index < scoreList.Length; index++)
        {
            if (scoreList[index] >= maxElement)
                maxElement = scoreList[index];
        }
        Debug.Log(maxElement);
        for (int index = 0; index < scoreList.Length; index++)
        {
            if (scoreList[index] == maxElement)
            {
                Indexlist.Add(index);
            }
        }
        if (Indexlist.Count > 1)
        {
            int minElement = Timelist[Indexlist[0]];
            for (int index=1;index < Indexlist.Count; index++)
            {
                if (Timelist[Indexlist[index]] < minElement)
                {
                    minElement = Timelist[Indexlist[index]];
                }
               
            }
            Debug.Log(minElement);
            for (int index = 0; index < Timelist.Length; index++)
            {
                if (Timelist[index] == minElement)
                {
                    WinnerBG.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = NameList[index];
                   
                    return;
                }
            }

        }
        else
        {
            WinnerBG.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = NameList[Indexlist[0]] ;
           
        }
     
    }
    void AssignList()
    {
        System.Array.Copy(IntroSceneManager.instance.playerScores, scoreList, 12);
        System.Array.Copy( IntroSceneManager.instance.TotalTimevalues, Timelist, 12);
        System.Array.Copy(IntroSceneManager.instance.playerNames, NameList, 12);
       // scoreList = new List<int>(IntroSceneManager.instance.playerScores);
    }
   
}
