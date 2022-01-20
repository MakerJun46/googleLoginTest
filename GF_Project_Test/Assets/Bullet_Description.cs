using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Description : MonoBehaviour
{

    public void setDeck_Button()
    {
        GameObject.Find("BackButton_Discription").SetActive(false);

        Destroy(this.gameObject);
    }

}
