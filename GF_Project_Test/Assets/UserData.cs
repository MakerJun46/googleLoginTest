using System.Collections;
using System.Collections.Generic;
using System;

public class UserData
{
    public List<string> hasHand = new List<string>();

    public UserData()
    {
    }

    public UserData(bool[] hashand)
    {
    }

    public void setData(List<string> hashand)
    {
        hasHand = hashand;
    }
}
