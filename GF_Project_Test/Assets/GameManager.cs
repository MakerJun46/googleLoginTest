using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UserData User;
    public List<bool> hasHand;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        User = new UserData();
        hasHand = new List<bool>();
    }
}
