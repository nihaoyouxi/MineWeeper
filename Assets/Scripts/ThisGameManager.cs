using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGameManager : MonoBehaviour {

    int width = 22;//定义每行格子数
    int heigth = 16;//定义每列格子数
    int x;//格子所在位置x坐标
    int y;//格子所在位置y坐标
    bool ifDead = false;
    public GameObject Square;//实例化的格子
    int mineCout = 50;//总雷数
    public static int aliveMineCount = 50;
    List<Square> AllSquare = new List<Square>();//所有格子
    public List<Square> AllMine = new List<Square>();//所有雷
    public List<Texture> AllTextures = new List<Texture>();//1-9的图片素材
    public UILabel label;

    // Use this for initialization
    public void Start() {
        for (int i = 0; i < 10; i++) {
            Texture texture = (Texture)Resources.Load("" + i);//加载图片素材
            AllTextures.Add(texture);//给格子赋值图片素材
        }
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < heigth; j++) {
                GameObject go = Instantiate(Square, new Vector3(-1.05f + 0.1f * i, 0.5f - 0.1f * j, 0), Quaternion.identity);
                go.transform.SetParent(transform);
                go.transform.localPosition = new Vector3(-1.05f + 0.1f * i, 0.5f - 0.1f * j, 0);
                go.transform.localScale = new Vector3(0.003344482f, 0.003344482f, 0.003344482f);
                go.GetComponent<Square>().xPosition = i;
                go.GetComponent<Square>().yPosition = j;
                go.GetComponent<Square>().ID = (i + 1) * (j + 1);
                AllSquare.Add(go.GetComponent<Square>());
            }
        }
        IniteMine(0, 0, 22, 16);
    }

    public List<Square> GetMineList(){
        return AllMine;
    }

    public void SetIfDead(bool MyCondition) {
        this.ifDead = MyCondition;
    }

    void IniteMine(int startwidth,int startheigth,int width,int heigth) {
        int k = 0;
        for (int i = 0; i < mineCout; i= AllMine.Count)
        {
            x = Random.Range(startwidth, width);//一下俩行代码随机雷的位置
            y = Random.Range(startheigth, heigth);
            if (AllMine.Count != 0)//若已存在雷数不为0
            {
                for (int j = 0; j < AllMine.Count; j++)
                {
                    Square sq = AllMine[j];
                    if (sq != null)
                    {
                        while (x == sq.xPosition && y == sq.yPosition)//判断雷区是否重叠
                        {
                            x = Random.Range(startwidth, width);
                            y = Random.Range(startheigth, heigth);
                        }
                    }
                }
                AllSquare[x * y].SetSquareType(9);
                AllSquare[x * y].SetShowPicture(AllTextures[9]);
                if (!AllSquare[x * y].hasInList) {
                    AllSquare[x * y].hasInList = true;
                    AllMine.Add(AllSquare[x * y]);
                }
                k++;
            }
            else if(AllMine.Count == 0)//若所存在雷数为0，添加雷
            {
                AllSquare[x * y].SetSquareType(9);
                AllSquare[x * y].SetShowPicture(AllTextures[9]);
                if (!AllSquare[x * y].hasInList)
                {
                    AllSquare[x * y].hasInList = true;
                    AllMine.Add(AllSquare[x * y]);
                }
                k++;
            }
        }
        SetCount(heigth);
        print(k);
    }

    void SetCount(int heigth) {
        for (int i=0;i<AllSquare.Count;i++) {
            int mineCount = 0;//一个格子周围雷数
            if (AllSquare[i].SquareType!=9)
            {
                //一下八个if语句判断一个格子周围格子是否为雷，是的话添加到对应格子脚本里雷列表里
                if (i - 1>= 0&& i - 1 < AllSquare.Count&& i%16!=0) {//防止数组越界
                    if (AllSquare[i - 1] != null)//防止空指针
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i - 1]);
                        if (AllSquare[i - 1].SquareType == 9)//若格子类型为雷，（0-8代表雷数，9代表就是一个雷）
                            mineCount++;//雷数+1
                    }
                }
                if (i+1>=0&&i + 1 < AllSquare.Count && i % 16 != 15) {
                    if (AllSquare[i + 1] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i + 1]);
                        if (AllSquare[i + 1].SquareType == 9)
                            mineCount++;
                    }
                }
                if (i - heigth >= 0&& i - heigth < AllSquare.Count) {
                    if (AllSquare[i - heigth] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i - heigth]);
                        if (AllSquare[i - heigth].SquareType == 9)
                            mineCount++;
                    }
                }
                if (i - heigth - 1>=0&& i - heigth - 1 < AllSquare.Count && i % 16 != 0) {
                    if (AllSquare[i - heigth - 1] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i - 1 - heigth]);
                        if (AllSquare[i - heigth - 1].SquareType == 9)
                            mineCount++;
                    }
                }
                if (i - heigth + 1>=0&& i - heigth + 1 < AllSquare.Count && i % 16 != 15) {
                    if (AllSquare[i - heigth + 1] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i + 1 - heigth]);
                        if (AllSquare[i - heigth + 1].SquareType == 9)
                            mineCount++;
                    }
                }
                if (i + heigth + 1>=0&&i + heigth + 1< AllSquare.Count && i % 16 != 15) {
                    if (AllSquare[i + heigth + 1] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i + 1 + heigth]);
                        if (AllSquare[i + heigth + 1].SquareType == 9)
                            mineCount++;
                    }
                }
                if (i + heigth - 1>=0&&i + heigth - 1 < AllSquare.Count && i % 16 != 0) {
                    if (AllSquare[i + heigth - 1] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i - 1 + heigth]);
                        if (AllSquare[i + heigth - 1].SquareType == 9)
                            mineCount++;
                    }
                }
                if (i + heigth>=0&&i + heigth < AllSquare.Count) {
                    if (AllSquare[i + heigth] != null)
                    {
                        AllSquare[i].GetAroundMine().Add(AllSquare[i + heigth]);
                        if (AllSquare[i + heigth].SquareType == 9)
                            mineCount++;
                    }
                }
                AllSquare[i].SetSquareType(mineCount);//给对应格子，赋值雷数
                AllSquare[i].SetShowPicture(AllTextures[mineCount]);//给对应格子赋值点击后显示的图片
            }
        }
    }

    public void IniteAllList() {
        aliveMineCount = 50;
        Application.LoadLevel("RemoveMine");
    }

	// Update is called once per frame
	void Update () {
        label.text = "" + aliveMineCount;
        if (ifDead) {
            foreach (Square sq in AllSquare) {
                sq.enabled = false;
                sq.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
