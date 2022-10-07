using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Text.RegularExpressions;
using System;

public class APIResponseAllocator : MonoBehaviour
{
    #region Variables

    [Header("API Response Holders")]
    public static APIResponseAllocator instance;
    public C_RegisterData registerResponse;
    public C_RegisterData2 registerResponse2;

    #endregion



    #region Unity CallBacks
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject.GetComponent(instance.GetType()));
            DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Serializable]
    public class C_RegisterData
    {
        public bool success;
        public string message;
        public Data data;


    }

    public class Data
    {
        public string firstName;
        public string lastName;
        public string designation;
        public string companyName;
        public string email;
        public int totalScore;
        public int totalTime;
        public int rank;
        public int status;
        public int emailVerified;
        public long createdAt;
        public long updatedAt;
        public int otp;
        public long expireTime;
        public int blockedAt;
        public string _id;
        public int __v;
    }

    [Serializable]
    public class C_RegisterData2
    {
        public bool success;
        public string message;
       // public Data data;


    }

}

