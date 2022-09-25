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
        IntroSceneManager.instance.ResetManagersGrp();
        WinnerBG.SetActive(true);

    }
}
