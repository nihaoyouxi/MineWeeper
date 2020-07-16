using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

    public int SquareType=-1;//格子类型,初始值为-1,9为雷
    public bool hasInList = false;
    public bool hasFlag = false;//雷是否被插旗标注
    public int FlagCount = 0;
    public bool IEnter = false;
    bool hasOpenAroundAll = false;
    public int xPosition;//格子x轴上的坐标
    public int yPosition;//格子y轴上的坐标
    public int ID;//第几个格子
    public int MineCount = -1;
    public bool isOpen = false;//格子是否已经被打开
    Texture showPicture;//格子打开后显示的图片
    Texture startTexture;
    Texture pressTexture;
    UITexture thisTexture;
    List<Square> AroundMine = new List<Square>();//格子周围的雷

    public void SetSquareType(int i) {//设置格子为雷或者数字
        SquareType = i;
    }

    public void SetShowPicture(Texture texture) {//设置格子图片素材
        showPicture = texture;
    }

    public List<Square> GetAroundMine() {//得到格子周围雷的集合
        return AroundMine;
    }

    // Use this for initialization
    void Start () {
        SetMineCount();
        thisTexture = GetComponent<UITexture>();
        startTexture = (Texture)Resources.Load("normal");
        pressTexture = (Texture)Resources.Load("highlight");
    }
	
	// Update is called once per frame
	void Update () {
        thisTexture = GetComponent<UITexture>();
        OnOpen();
        OpenAround();
        foreach (Square sq in AroundMine) {
            if (sq.isOpen==false&&sq.SquareType!=9) {
                return;
            }
        }
        hasOpenAroundAll = true;
    }

    void OnPress() {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            foreach (Square sq in AroundMine)
            {
                sq.IEnter = false;
            }
            IEnter = true;
        }
        if (Input.GetMouseButton(1)) {
            Flag();
        }
        else if (Input.GetMouseButton(0) && !isOpen)
        {
            Open();
        }
    }

    void OnHover() {
        if (!IEnter)
        {
            foreach (Square sq in AroundMine) {
                sq.IEnter = false;
            }
            IEnter = true;
        }
        else
        {
            IEnter = false;
        }
    }

    void OnOpen() {//通过点击一个格子，打开周围无雷区
        if (isOpen&&!hasFlag) {
            bool canOpen = true;
            foreach (Square sq in AroundMine) {
                if (sq!=null)
                {//若格子周围没有雷，则canOpen是true，否则为false，返回
                    if (sq.SquareType == 9)
                    {
                        canOpen = false;
                        return;
                    }
                }
            }
            if (canOpen) {//若canOpen是true
                for (int i=0;i< AroundMine.Count;i++ ) {
                    if(AroundMine [i]!= null)
                        AroundMine[i].Open();//调用格子周围雷列表中对象的Open()函数
                }
            }
        }
    }

    void Flag() {
        if (!isOpen) {
            if (Input.GetMouseButtonDown(1)&&!hasFlag) {
                thisTexture.mainTexture = (Texture)Resources.Load("flag");
                hasFlag = true;
                if(ThisGameManager.aliveMineCount>0&& ThisGameManager.aliveMineCount<=110)
                    ThisGameManager.aliveMineCount--;
            }
            else if (Input.GetMouseButtonDown(1) && hasFlag) {
                thisTexture.mainTexture = (Texture)Resources.Load("normal");
                hasFlag = false;
                if (ThisGameManager.aliveMineCount >= 0 && ThisGameManager.aliveMineCount < 110)
                    ThisGameManager.aliveMineCount++;
            }
        }
    }

    public void Open() {
        if (!this.isOpen && !this.hasFlag)
        {
            if (showPicture != null) {
                thisTexture.mainTexture = showPicture;
                //thisTexture.SetDimensions(20, 20);
                //thisTexture. = new Vector2(20, 20);
            }
            this.isOpen = true;
            if (this.SquareType==9) {
                foreach (Square sq in GameObject.FindWithTag("GameController").GetComponent<ThisGameManager>().GetMineList()) {
                    if (!sq.hasFlag) {
                        sq.Open();
                    }
                }
                GameObject.FindWithTag("GameController").GetComponent<ThisGameManager>().SetIfDead(true);
            }
        }
    }

    void SetFlagCount() {
        this.FlagCount=0;
        foreach (Square sq in AroundMine)
        {
            if (sq != null)
            {//若格子周围没有雷，则canOpen是true，否则为false，返回
                if (sq.hasFlag && FlagCount >= 0 && this.FlagCount < 8)
                {
                    this.FlagCount++;
                }
            }
        }
    }

    void SetMineCount() {
        this.MineCount = 0;
        foreach (Square sq in AroundMine)
        {
            if (sq != null)
            {//若格子周围没有雷，则canOpen是true，否则为false，返回
                if (sq.SquareType==9 && MineCount >= 0 && this.MineCount < 8)
                {
                    this.MineCount++;
                }
            }
        }
    }

    public void OpenAround() {
        if(isOpen&&this.SquareType!=9&& IEnter)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButton(1))
                {
                    SetFlagCount();
                    SetMineCount();
                    if (FlagCount == MineCount&&!hasOpenAroundAll)
                    {
                        for (int i=0;i< AroundMine.Count;i++) {
                                AroundMine[i].Open();
                        }
                        hasOpenAroundAll = false;
                    }
                    else if(FlagCount != MineCount)
                    {
                        //print("OpenAround");
                        for (int j = 0; j < AroundMine.Count; j++)
                        {
                            if (!AroundMine[j].isOpen&& !AroundMine[j].hasFlag) {
                                    AroundMine[j].gameObject.GetComponent<UITexture>().mainTexture = pressTexture;
                            }
                        }
                    }
                }
            }
            if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && isOpen && !hasFlag)
            {
                for (int i = 0; i < AroundMine.Count; i++)
                {
                    if (!AroundMine[i].isOpen && !AroundMine[i].hasFlag)
                    {
                        AroundMine[i].gameObject.GetComponent<UITexture>().mainTexture = AroundMine[i].startTexture;
                    }
                }
            }
        }
    }
}
