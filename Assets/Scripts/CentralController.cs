using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using sysArray = System.Array;
using System.IO;

public class CentralController : MonoBehaviour
{
    #region Inspector

    //  Inspector上のオブジェクト習得用関数群
    [Tooltip("制御したい Text Controller がある Hieralcy オブジェクトを入れてください。")]
    public GameObject textControllerPlace;

    [Tooltip("Transition ○○ を入れてある Hieralcy オブジェクトを入れてください。")]
    public GameObject transitionPlace;

    [Tooltip("制御したい GuiArrow がある Hieralcy オブジェクトを入れてください。")]
    public GameObject gArrow;

    [Tooltip("表示したいテキストフィールドがあるキャンバスを入れてください。 GuiArrowActive を アタッチする必要があります。")]
    public GameObject[] getCanvas;

    [Tooltip("会話文を表示したいテキストフィールドを入れてください。")]
    public Text[] getText;

    [Tooltip("アニメーションさせたい Character を入れてください。")]
    public GameObject[] Character;

    [Tooltip("FontSize 調査用の Canvas を入れてください。")]
    public GameObject fontSizeCanvas;

    [Tooltip("FontSize 調査用の Text を入れてください。")]
    public Text fontSizeText;

    #endregion


    #region GetTransition

    //  TransitionBook習得用関数群
    private int[] MoveTransition;
    public int[] moveTransition
    {
        get { return MoveTransition; }
        private set { MoveTransition = value; }
    }
    private int[] MoveCanvas;
    public int[] moveCanvas
    {
        get { return MoveCanvas; }
        set { MoveCanvas = value; }
    }
    private int[] MoveChoiceNoSelect;
    public int[] moveChoiceNoSelect
    {
        get { return MoveChoiceNoSelect; }
        set { MoveChoiceNoSelect = value; }
    }
    private int[] ChoiceLength;
    public int[] choiceLength
    {
        get { return ChoiceLength; }
        set { ChoiceLength = value; }
    }
    private int[] fontSise;
    private bool[] noDelay;

    //  TransitionAnimBook習得用関数群
    private int[] aniNo;
    private int[] charactor;
    private string[] aniName;
    private int[] anime;

    //  アニメーションさせたいキャラクターの習得用関数群
    private Animator[] animator;
    //  アニメーション制御
    private int x;

    //  TransitionSpawnPoint習得用関数群
    private int[] spawnEvent;
    private Vector3[] spawnPointVector3;

    private bool[] AActive;
    public bool[] aActive{
        get { return AActive; }
        set { AActive = value; }
    }

    #endregion


    #region ReadSqript
    //  Script読み込み
    TransitionBook tBook;
    TransitionSpawnPoint tSpawnPoint;
    TransitionAnimBook aBook;
    ChoiceController ChoiceCon;
    public TextController TController { get; set; }
    public ScenarioManager sMan { get; set; }

    private GUIArrowActive[] CanvasActive;
    public GUIArrowActive[] canvasActive{
        get { return CanvasActive; }
        set { CanvasActive = value; }
    }


    #endregion


    #region BasicFunction

    //  現在の TransitionBook の行数
    public int j { get; set; }

    //  現在のシナリオ行数
    public int scenarioNum { get; set; }

    //  一つ前に実行されていたキャンバスを入力する
    [System.NonSerialized]
    public GameObject CanvasTorpidity;

    //  現在表示されているキャンバス
    [System.NonSerialized]
    public int nowC;

    //  CanvasPosition の初期値を入力する
    private Vector3[] setCanvasInitialPositionValue;

    #endregion


    #region SpawnEventRelation

    //  spawnEvent の行数を管理
    public int y{ get; set; }

    //  scenarioNum の値と spawnEvent[y] の値が一致した時に True になる
    public bool spawnEventActive{ get; set; }

    //  spawnEventActive を、scenarioNum の値が変化するごとに１回だけ false にする関数に使用する
    //  この関数はmoveObjectが呼び出される前に記述する
    public int count { get; set; }

    //  キャンパスが移動してからキャンパスを発見するまでの時間を保存
    private float timeCount;

    //  timeCount の Debug.Log() を移動、発見後一回だけ表示させるために、spawnEvent[y] の値を格納しておく
    private int timeDisplay;

    #endregion


    #region unityMainFunction

    ///  <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        startMove();
    }


    ///  <summary>
    ///  Update is called once per frame
    ///  </summary>
    void Update()
    {
        mainMove();
    }

    #endregion


    #region startMove

    /**
     *  <summary>
     *  開始時に行われる処理
     *  </summary>
     */
    public void startMove()
    {
        ChoiceCon = GetComponent<ChoiceController>();
        TController = textControllerPlace.GetComponent<TextController>();
        sMan = textControllerPlace.GetComponent<ScenarioManager>();

        //  配列に値を入れる処理
        settBook();
        setAnimBook();
        setSpawnPoint();

        setAllCanvas();
        setGuiArrowActive();
        setCanvasInitialPosition();

        for (int i = 0; i < getCanvas.Length; i++)
        {
            getCanvas[i].SetActive(false);
        }

        //  ChoiceCanvas を使用したい場合はこちらを ON
        ChoiceCon.choiceStartMove();
        //  FontSize 調査用の Canvas を使用したい場合はこちらを ON
        fontSizeCanvas.SetActive(false);

        CanvasTorpidity = getCanvas[0];
        timeDisplay = spawnEvent[0];

        //  画面初期化処理
        setBookprocessing();
        characterAnimation();

        //  現在の日付、時刻を LogData に書き出し
        textSave(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
    }

    #endregion


    #region mainMove

    /**
     *  <summary>
     *  メイン処理部分
     *  </summary>
     */
    public void mainMove()
    {
        //  現在のシナリオの行数を習得
        scenarioNum = TController.maneger;

        //  シナリオの行数が遷移No.と同じ数値になったとき
        if (scenarioNum == moveTransition[j])
        {
            //  ChoiceSelect が 0 以外ならチョイス画面に飛ぶ
            if (moveChoiceNoSelect[j] == 0)
            {
                setBookprocessing();
            }
            else if (choiceLength[j] != 0)
            {
                textControllerPlace.SetActive(false);
                fontSizeCanvas.SetActive(true);

                getCanvas[moveCanvas[j]].SetActive(false);
                CanvasTorpidity.SetActive(false);
            }
            else
            {
                ChoiceCon.choiceNoSelectController();
            }
        }

        moveObject();

        settControllerActive();

        characterAnimation();

        investigateFontSize();
    }

    #endregion


    #region FontSize の調査のための処理

    /**
     * <summary>
     * fontSizeCanvas, fontSizeText のマウス向け処理
     * </summary>
     */
    public void investigateFontSize()
    {
        if (fontSizeCanvas.activeInHierarchy && fontSizeText.fontSize >= 1)
        {
            nowC = getCanvas.Length;
            if (Input.GetMouseButton(0))
            {
                fontSizeText.fontSize += 1;
            }

            if (Input.GetMouseButton(1))
            {
                if (fontSizeText.fontSize != 1)
                {
                    fontSizeText.fontSize -= 1;
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                endFontSizeCanvas();
            }
        }
    }


    public void endFontSizeCanvas()
    {
        Debug.Log(fontSizeText.fontSize);
        textSave("FontSize : " + fontSizeText.fontSize.ToString());
        fontSizeCanvas.SetActive(false);
        sMan.m_textController.maneger -= 1;
        sMan.m_currentLine -= 1;
        sMan.RequestNextLine();
        setBookprocessing();
    }

    #endregion


    #region Spawn

    /**
     *  <summary>
     *  TransitionSpawnPoint の EventNo. の値が
     *  scenarioNum の値と一致したときに
     *  オブジェクトを指定した座標に移動させる処理
     *  </summary>
     */
    public void moveObject()
    {
		if (scenarioNum == spawnEvent[y] && spawnEventActive == false)
        {
            setAllCanvas()[nowC].transform.localPosition = spawnPointVector3[y];
            y++;
			spawnEventActive = true;
        }
        else if(spawnEventActive == false)
        {
            setAllCanvas()[nowC].transform.localPosition = setCanvasInitialPositionValue[nowC];
        }

        if (spawnEventActive == true)
        {
            timeCount += Time.deltaTime;
        }

        if (scenarioNum - 1 == timeDisplay)
        {
            if (spawnEventActive == true)
            {
                Debug.Log("Time : " + timeCount);
                timeDisplay = spawnEvent[y];
                textSave("Time : " + timeCount);
                timeCount = 0;
                spawnEventActive = false;
            }
        }
    }

    #endregion


    #region Main_setBook

    /**
     *  <summary>
     *  transitionBook に入力された処理を実行する
     *  </summary>
     */
    public void setBookprocessing()
    {
        //  一つ前のCanvasを非アクティブ状態にする
        CanvasTorpidity.SetActive(false);

        //  moveCanvasに入力された値と同じ値のCanvasをアクティブ状態にする
        getCanvas[moveCanvas[j]].SetActive(true);
        nowC = moveCanvas[j];

        //  今のキャンバスと前のキャンバスが一致していなかったら
        //  前のキャンバスに今のキャンバスを入れる
        if (CanvasTorpidity != getCanvas[moveCanvas[j]])
        {
            CanvasTorpidity = getCanvas[moveCanvas[j]];
        }

        //  テキストを表示する場所を変更する
        TController._uiText = getText[moveCanvas[j]];

        //  テキストのフォントサイズを変更する
        getText[moveCanvas[j]].fontSize = fontSise[j];

        //  テキストを一文字ずつ表示するか
        if (noDelay[j])
        {
            TController.NoDeley = true;
        }
        else
        {
            TController.NoDeley = false;
        }
        //  何行目まで処理をしたかカウントする
        j += 1;

    }

    #endregion


    #region GuiArrow

    /**
     *  <summary>
     *  文字を表示しているキャンバスが視界外へ行った際、
     *  テキストを進めることが出来なくする処理。
     *  </summary>
     */
    public void settControllerActive()
    {
        for (int i = 0; i < getCanvas.Length; i++)
        {
            // キャンバスが全体のキャンバスではない時
            if (i != 1)
            {
                // 画面外にキャンパスが出ると、テキストが進まなくなる
                if (getCanvas[i].activeInHierarchy)
                {
                    if (canvasActive[i]._isRendered)
                    {
                        textControllerPlace.SetActive(true);
                    }
                    else
                    {
                        textControllerPlace.SetActive(false);
                    }
                }
            }
        }
    }

    #endregion


    #region Animation

    /**
     *  <summary>
     *  キャラクターにアニメーションをさせる関数
     *  </summary>
     */
    public void characterAnimation()
    {
        //scenarioNum = Tcon.maneger;
        if (x < aBook.animData.Length)
        {
            if (scenarioNum == aniNo[x])
            {
                animator[charactor[x]].SetBool(anime[x], true);
                x += 1;
            }
            else
            {
                animator[charactor[x]].SetBool(anime[x], false);
            }
        }
        else
        {
            animator[charactor[x - 1]].SetBool(anime[x - 1], false);

        }
    }

    #endregion


    #region GetTransition

    /**
     *  <summary>
     *      transitionPlace 下の TransitionBook(sqript) の値を習得する
     *  </summary>
     * 
     *  <param name = "moveTransition" >
     *      会話文がmoveTransition行目に来ると、イベントを起こす
     *  </param>
     *  
     *  <param name = "moveCanvas">
     *      moveCanvas番目に代入されたキャンバスを表示し、前のキャンバスを非表示にする
     *  </param>
     *  
     *  <param neme = "moveChoiceNoSelect">
     *      選択肢を表示するか判断する
     *       (0=表示しない,
     *        1=現在の Canvas を無効化した上で表示する,
     *        2=現在の Cancas を無効化せずに表示する)
     *  </param>
     *  
     *  <param neme = "choiceLength">
     *      moveChoiceNoSelect が true の場合選択肢の長さを入力する
     *  </param>
     *  
     *  <param name = "tBook.data[].debugChoiceLength">
     *      choiceLength でキャンバスが出現した後、 transitionChoice
     *       の処理を行うが、処理後の位置( cNO )は引き続き加算されるため、
     *       次はどこから始まるのか、視覚的に分かりやすくするために
     *       最新の行の値を表示する
     *  </param>
     *  
     *  <param neme = "fontSize">
     *      フォントサイズを入力された大きさに変更する。
     *  </param>
     *  
     *  <param neme = "noDeley">
     *      Trueだと会話文が即、表示される。falseだと、一文字づつ表示される。
     *  </param>
     *  
     */
    public void settBook()
    {
        //  配列の初期化処理
        tBook = transitionPlace.GetComponent<TransitionBook>();

        var tdLen = tBook.data.Length + 1;

        sysArray.Resize(ref MoveTransition, tdLen);
        sysArray.Resize(ref MoveCanvas, tdLen);
        sysArray.Resize(ref MoveChoiceNoSelect, tdLen);
        sysArray.Resize(ref ChoiceLength, tdLen);
        sysArray.Resize(ref fontSise, tdLen);
        sysArray.Resize(ref noDelay, tdLen);

        for (int i = 0; i < tBook.data.Length; i++)
        {
            moveTransition[i] = tBook.data[i].transitionNum;
            moveCanvas[i] = tBook.data[i].canvasNum;
            moveChoiceNoSelect[i] = tBook.data[i].choiceNoSelect;
            choiceLength[i] = tBook.data[i].choiceLength;
            if (i == 0)
            {
                tBook.data[i].debugChoiceLength += choiceLength[i];
            }
            else
            {
                tBook.data[i].debugChoiceLength = tBook.data[i - 1].debugChoiceLength + choiceLength[i];
            }
            fontSise[i] = tBook.data[i].fontSize;
            noDelay[i] = tBook.data[i].delay;
        }
    }


    /**
    *  <summary>
    *  transitionPlace 下の TransitionAnimBookの値を習得する。
    *  </summary>
    */
    public void setAnimBook()
    {
        //Tcon = textControllerPlace.GetComponent<TextController>();
        aBook = transitionPlace.GetComponent<TransitionAnimBook>();

        var aaLen = aBook.animData.Length;

        sysArray.Resize(ref aniNo, aaLen);
        sysArray.Resize(ref charactor, aaLen);
        sysArray.Resize(ref aniName, aaLen);
        sysArray.Resize(ref anime, aaLen);
        for (int i = 0; i < aaLen; i++)
        {
            aniNo[i] = aBook.animData[i].animNo;
            charactor[i] = aBook.animData[i].animCharacter;
            aniName[i] = aBook.animData[i].animName;
            anime[i] = Animator.StringToHash(aniName[i]);
        }
        sysArray.Resize(ref animator, Character.Length + 1);
        for (int l = 0; l < Character.Length; l++)
        {
            animator[l] = Character[charactor[l]].GetComponent<Animator>();
        }
    }


    /**
     * <summary>
     *  transitionPlace 下の transitionSpawnPointの値を習得する。
     * </summary>
     */
    public void setSpawnPoint()
    {
        tSpawnPoint = transitionPlace.GetComponent<TransitionSpawnPoint>();

        var spLen = tSpawnPoint.spawnPointData.Length;

        sysArray.Resize(ref spawnEvent, spLen + 1);
        sysArray.Resize(ref spawnPointVector3, spLen);
        sysArray.Resize(ref AActive, spLen);

        for (int i = 0; i < spLen; i++)
        {
            tSpawnPoint.spawnPointData[i].dataLength = i;
            spawnEvent[i] = tSpawnPoint.spawnPointData[i].eventNum;
            spawnPointVector3[i] = tSpawnPoint.spawnPointData[i].spawnPoint;
            aActive[i] = tSpawnPoint.spawnPointData[i].arrowActive;
        }
    }

    #endregion
    
    
    #region Transition 以外の配列処理
    
    /**
     *  <summary>
     *  読み取り専用オブジェクトallcanvasを作成
     *  全Canvas配列を格納
     *  </summary>
     */
    public GameObject[] setAllCanvas()
    {
        GameObject[] allCanvas = null;
        //配列の初期化処理

        var gcLen = getCanvas.Length;
        var gccLen = 0; 
        //var gccLen = ChoiceCon.getChoiceCanvas.Length;

        sysArray.Resize(ref allCanvas, gcLen + 1 + gccLen);

        for (int i = 0; i < gcLen + 1 + gccLen; i++)
        {
            if (i < gcLen)
            {
                allCanvas[i] = getCanvas[i];
            }
            else if(i < gcLen + 1)
            {
                allCanvas[i] = fontSizeCanvas;
            }
            else
            {
                allCanvas[i] = ChoiceCon.getChoiceCanvas[i - gcLen + 1];
            }
        }
        return (allCanvas);
    }


    /**
     *  <summary>
     *  GetCanvas に入力された Canvas 下の GuiArrowActive(sqript) を習得する。
     *  </summary>
     *  
     *  <param name ="camvasActive">
     *      Canvas 内の GuiArrowActive の isRendered が
     *       Trur か False か習得するための関数
     *  </param>
     * 
     */
    public void setGuiArrowActive()
    {
        var sacLen = setAllCanvas().Length;

        //配列の初期化処理
        sysArray.Resize(ref CanvasActive, sacLen);

        for (int i = 0; i < sacLen; i++)
        {
            canvasActive[i] = setAllCanvas()[i].GetComponent<GUIArrowActive>();
        }
    }


    /**
     * <summary>
     * キャンバスの初期位置を習得
     * </summary>
     */
    public void setCanvasInitialPosition()
    {
        sysArray.Resize(ref setCanvasInitialPositionValue, setAllCanvas().Length);
        for (int i = 0; i < setAllCanvas().Length; i++)
        {
            setCanvasInitialPositionValue[i] = setAllCanvas()[i].transform.localPosition;
        }
    }


    #endregion


    #region ログをファイルに書き出す処理

    /**
     *  <summary>
     *  LogFileを書き出す
     *  </summary> 
     *  
     *  <param name="txt">
     *  LogDataとして書き出したい内容を入力する
     *  </param>
     */
    public void textSave(string txt)
    {
        var sysNow = System.DateTime.Now;
        SafeCreateDirectory("LogData");
        StreamWriter sw = new StreamWriter("LogData/" + sysNow.ToString("yyyy_MM_dd") + ".txt", true); //true=追記 false=上書き
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
    }


    /** <summary>
     *  指定したパスにディレクトリが存在しない場合
     *  すべてのディレクトリとサブディレクトリを作成します
     *  </summary>
     *  
     *  <param name="path">
     *  存在するか確認したいフォルダ名を入力する
     *  </param>
     */
    public static DirectoryInfo SafeCreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return null;
        }
        return Directory.CreateDirectory(path);
    }
    #endregion
}
