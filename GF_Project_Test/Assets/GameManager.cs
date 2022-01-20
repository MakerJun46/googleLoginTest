using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UserData User;
    public List<string> hasHand;

    public string DR;
    public string DS;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        User = new UserData();
        hasHand = new List<string>();

    }


    public void print()
    {
        foreach (string item in hasHand)
        {
            Debug.Log("gamemanager : " + item);
        }
    }
}
