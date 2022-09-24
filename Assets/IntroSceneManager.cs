using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        DontDestroyOnLoad(this.gameObject);

        ChooseCategoryBtn.onClick.AddListener(()=> CategoryBtnOptions());
        CategoryBtns[0].onClick.AddListener(() => SetCategory(0, CategoryBtns[0].gameObject));
        CategoryBtns[1].onClick.AddListener(() => SetCategory(1, CategoryBtns[1].gameObject));
        CategoryBtns[2].onClick.AddListener(() => SetCategory(2, CategoryBtns[2].gameObject));
        CategoryBtns[3].onClick.AddListener(() => SetCategory(3, CategoryBtns[3].gameObject));
        CategoryBtns[4].onClick.AddListener(() => SetCategory(4, CategoryBtns[4].gameObject));
        CategoryBtns[5].onClick.AddListener(() => SetCategory(5, CategoryBtns[5].gameObject));
        CategoryBtns[6].onClick.AddListener(() => SetCategory(6, CategoryBtns[6].gameObject));

        UserFields[0].onValueChanged.AddListener(delegate { UsernameUpdate(); });
        for (int i=0;i< UserFields.Length;i++)
        {
            UserFields[i].onValueChanged.AddListener(delegate { FieldsCheck(); });

        }
    }

    void UsernameUpdate()
    {
        UserNameText.text = UserFields[0].text;
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
        CategoryName.text = B.name;
        StartGameBtn.interactable = true;
    }
}
