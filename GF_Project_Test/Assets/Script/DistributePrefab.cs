using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class DistributePrefab : MonoBehaviour
{
    public static DistributePrefab instance;

    public GameObject Parent;
    public GameObject Line;

    public GameObject nullPrefab;
    public GameObject GapOfHand;

    public Text AddHand_Text;

    //종류,유무,레벨
    string[,] MyHandList = { { "DR", "0", "1" },
                             { "DS", "0", "2" },
                             { "DP", "0", "5" },
                             { "CR", "0", "4" },
                             { "CS", "0", "5" },
                             { "CP", "0", "6" },
                             { "FR", "0", "7" },
                             { "FS", "0", "8" },
                             { "FP", "0", "9" },
                             { "ER", "0", "10" },
    };

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AllSort();
    }

    public void SetData()
    {
        for(int i = 0; i < MyHandList.Length; i++)
        {
            MyHandList[i, 1] = GameManager.instance.hasHand[i] == "1" ? "1" : "0";
        }

        AllSort();
    }
    void instantiatePrefab(int HandCount,List<string> Hand)
    {
        int row = 1;
        int index = 0;
        int copyHandCount = HandCount;
        if (HandCount > 5)
        {
            row = (HandCount / 5) + 1;
        }
        Debug.Log("row : " + row);
        for (int i = 0; i < row; i++)
        {
            GameObject List = Instantiate(Line, Parent.transform);
            if (HandCount < 5)
            {
                for (int k = 0; k < HandCount; k++)
                {
                    string a = Hand[index];
                    GameObject obj = Resources.Load<GameObject>("prefab/Main/" + a);
                    Instantiate(obj, List.transform);
                    index++;
                }
                for(int w = 0; w < 5 - HandCount; w++)
                {
                    Instantiate(nullPrefab, List.transform);
                }
            }
            else
            {
                Debug.Log("처음 HandCount: " + copyHandCount);
                for (int j = 0; j < 5; j++)
                {
                    if (copyHandCount == 0)
                    {
                        Debug.Log("nullPrefab생성");
                        Instantiate(nullPrefab, List.transform);
                    }
                    else
                    {
                        string a = Hand[index];

                        GameObject obj = Resources.Load<GameObject>("prefab/Main/" + a);
                        Instantiate(obj, List.transform);
                        index++;
                        copyHandCount--;
                        Debug.Log("HandCount: " + copyHandCount);
                    }

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AllSort()
    {
        DeleteChilds();
        List<string> myHand = new List<string>();
        List<string> myItem = new List<string>();
        List<string> notMyHand = new List<string>();
        List<string> notmyItem = new List<string>();
        int index = 0;
        for (int i=0; i< MyHandList.GetLength(0); i++)
        {
            if (MyHandList[i,1] == "1")
            {
                myHand.Add(MyHandList[i, 0]);
            }
            else
            {
                notMyHand.Add(MyHandList[i, 0]);
            }
        }
        Instantiate(GapOfHand, Parent.transform);
        for (int i=0; i < (myHand.Count/5)+1; i++)//row수 만큼 Line을 만듬
        {
            GameObject List = Instantiate(Line, Parent.transform);
            for(int j = 0; j < 5; j++)
            {
                if (index < myHand.Count)//가지고있는 손의 list인 myHand의 크기를 넘기 전까지 Line안에 obj생성
                {
                    string a = myHand[index];
                    GameObject obj = Resources.Load<GameObject>("prefab/Main/" + a);
                    Instantiate(obj, List.transform);
                    index++;
                }
                else//[o][o][][] 이렇게 4칸에 2개만 남으면 나머지 2칸에 nullPrefab을 넣는다
                {
                    Instantiate(nullPrefab, List.transform);
                }
            }
        }
    }

    public void DeleteChilds()
    {
        // child 에는 부모와 자식이 함께 설정 된다.
        
        for(int i=0; i < Parent.transform.childCount; i++)
        {
            Destroy(Parent.transform.GetChild(i).gameObject);
        }
    }

    public void addHand()
    {
        for (int i = 0; i < MyHandList.Length; i++)
        {
            if (MyHandList[i, 1] == "0")
            {
                MyHandList[i, 1] = "1";
                GameManager.instance.hasHand[i] = "1";
                AddHand_Text.text = "Added : " + MyHandList[i, 0];
                break;
            }
        }
        AllSort();
    }
}
