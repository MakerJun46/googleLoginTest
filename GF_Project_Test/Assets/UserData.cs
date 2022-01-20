using System.Collections;
using System.Collections.Generic;
using System;

public class UserData
{
    public bool[] hasHand = new bool[57];

    public UserData()
    {
    }

    public UserData(bool[] hashand)
    {
        for(int i = 0; i < 30; i++)
        {
            hasHand[i] = hashand[i];
        }
    }

    public void setData(List<bool> hashand)
    {
        for (int i = 0; i < 30; i++)
        {
            hasHand[i] = hashand[i];
        }
    }
}
