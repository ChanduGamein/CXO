using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusManger : MonoBehaviour, IPunObservable
{
    public static StatusManger instance = null;
    public int CategoryNumber,QuestionIndex,QuestionTotal,MaxQuestions;
    public float Timer;
    public TextMeshProUGUI QuestionText, TimerText,QuestionTotalText;
    public Image TimerImage;
    public bool StartTimer;
    public GameObject WinnerBG;
    bool isFinished;
    public int[] scoreList,Timelist;
    public List<int> Indexlist,MainGrpList;
    public string[] NameList;
    int checkindex,tempValue;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CategoryNumber);
            stream.SendNext(QuestionIndex);
            stream.SendNext(QuestionTotal);
            stream.SendNext(MaxQuestions);
            stream.SendNext(Timer);

        }
        else
        {
            CategoryNumber = (int)stream.ReceiveNext();
            QuestionIndex = (int)stream.ReceiveNext();
            QuestionTotal = (int)stream.ReceiveNext();
            MaxQuestions = (int)stream.ReceiveNext();
            Timer = (float)stream.ReceiveNext();
            if(QuestionTotal > MaxQuestions) IntroSceneManager.instance.ResetManagersGrp(); 
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
        MaxQuestions = 3;
        //AssignList();
        //for (int i = 0; i < PunManager.instance.PlayerCount-1; i++)
        //{
        //    GetWinner();
        //}
      
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestionTotal <= MaxQuestions)
        {
            if (StartTimer && Timer > 0)
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
                    if (QuestionTotal <= MaxQuestions)
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
        Timer = 45;
        StartTimer = true;
    }
    void DisableManagers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IntroSceneManager.instance.ResetManagersGrp();
          //  WinnerBG.SetActive(true);
            AssignList();

            for (int i = 0; i < PunManager.instance.PlayerCount-1; i++)
            {
                GetWinner();
            }

        }
        else
        {
            PhotonNetwork.Disconnect();
            IntroSceneManager.instance.PunMangerObject.SetActive(false);
        }

    }
   
    public void GetWinner()
    {
        Indexlist.Clear();
        int maxElement = scoreList[2];
        for (int index = 2; index < scoreList.Length; index++)
        {
            if (scoreList[index] >= maxElement)
                maxElement = scoreList[index];
        }
        Debug.Log(maxElement);
        if (maxElement==0)
        {
            WinnerBG.SetActive(true);
            WinnerBG.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text ="No Winner";
            return;
        }
        else
        {
            for (int index = 0; index < scoreList.Length; index++)
            {
                if (scoreList[index] == maxElement)
                {
                    Debug.Log(index);
                    Indexlist.Add(index);
                }
            }
            if (Indexlist.Count > 1)
            {
                int minElement = Timelist[Indexlist[0]];
                for (int index = 1; index < Indexlist.Count; index++)
                {
                    if (Timelist[Indexlist[index]] < minElement)
                    {
                        minElement = Timelist[Indexlist[index]];
                    }

                }
                Debug.Log(minElement);
                for (int index = 0; index < Indexlist.Count; index++)
                {
                    if (Timelist[Indexlist[index]] == minElement)
                    {
                       
                        if (checkindex > 0)
                        {
                            if (tempValue != Indexlist[index]) MainGrpList.Add(Indexlist[index]);
                        }
                        else
                        {
                            MainGrpList.Add(Indexlist[index]);
                        }

                        checkindex++;
                        tempValue = Indexlist[index];

                        scoreList[Indexlist[index]] = 1;
                        Timelist[Indexlist[index]] = 10000;
                        WinnerBG.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = NameList[MainGrpList[0]];// NameList[index];
                        return;
                    }
                }

            }
            else
            {
                if (checkindex > 0) {
                  if (tempValue != Indexlist[0])  MainGrpList.Add(Indexlist[0]); 
                }
                else { 
                    MainGrpList.Add(Indexlist[0]);
                }

                checkindex++;
                tempValue = Indexlist[0];

                scoreList[Indexlist[0]]= 1;
                Timelist[Indexlist[0]] = 10000;
                WinnerBG.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = NameList[MainGrpList[0]]; // NameList[Indexlist[0]];

            }
        }

        IntroSceneManager.instance.AssignSprites();
       
    }
    void AssignList()
    {
        System.Array.Copy(IntroSceneManager.instance.playerScores, scoreList, 12);
        System.Array.Copy(IntroSceneManager.instance.TotalTimevalues, Timelist, 12);
        System.Array.Copy(IntroSceneManager.instance.playerNames, NameList, 12);

        // scoreList = new List<int>(IntroSceneManager.instance.playerScores);

    }
   
}
