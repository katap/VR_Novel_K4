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

    [Tooltip("会話文を表示したいテキストフィールドを入力してください。")]
    public Text[] getText;

    [Tooltip("ON, OFF 制御したい選択肢の Canvas を入れてください。尚、Element に入れるオブジェクトは GetChoiceText と対応させてください。")]
    public GameObject[] getChoiceCanvas;

    [Tooltip("テキストを変えたい選択肢の Text を入れてください。")]
    public Text[] getChoiceText;

    [Tooltip("アニメーションさせたい Character を入れてください。")]
    public GameObject[] Character;

    #endregion


    #region GetTransition

    //  TransitionBook習得用関数群
    private int[] moveTransition;
    private int[] moveCanvas;
    private int[] moveChoiceNoSelect;
    private int[] choiceLength;
    private int[] fontSise;
    private bool[] noDelay;

    //  TransitionChoice習得用関数群
    private int[] cNo;
    private int[] canvasNum1;
    private int[] fontSize1;

    private int[] NextNo1;
    public int[] nextNo1{
        get { return NextNo1; }
        private set { NextNo1 = value; }
    }

    private int[] canvasNum2;
    private int[] fontSize2;

    private int[] NextNo2;
    public int[] nextNo2{
        get { return NextNo2; }
        private set { NextNo2 = value; }
    }

    //  TransitionChoiceText習得用関群
    private string[] choiceText;

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

    [System.NonSerialized]
    public int choiceswitch;
    [System.NonSerialized]
    public int beforeswitch;

	//  TransitionSpawnChoicePoint習得用関数群
    /*
	private int[] spawnChoicePointEvent;
    private Vector3[] spawnChoicePointVector3;
    */


    #endregion


    #region ReadSqript
    //  Script読み込み
    TransitionBook tBook;
    TransitionChoice tChoice;
    TransitionChoiceText tChoiceText;
    TransitionSpawnPoint tSpawnPoint;
    //TransitionSpawnChoicePoint tSpawnChoicePoint;
    TextController TController;
    TransitionAnimBook aBook;

    private GUIArrowActive[] CanvasActive;
    public GUIArrowActive[] canvasActive{
        get { return CanvasActive; }
        set { CanvasActive = value; }
    }

    public ScenarioManager sMan{ get; set; }

    #endregion


    #region BasicFunction

    //  現在の TransitionBook の行数
    private int j = 0;

    //  現在の TransitionChoice の行数
    //  CircleCursole 内でプラス処理を行う
    [System.NonSerialized]
    public int k = 0;

    //  TransitionChoice の Back 処理
    [System.NonSerialized]
    public List<int> cBack = new List<int>();
    //  TransitionChoice が有効になっている際に、Back処理が行われても
    //  最終的に辿ってきた FontSize の正常な値を返すために一時保存するための関数
    [System.NonSerialized]
    public List<int> cBackSize = new List<int>();

    //  transitionChoiceが何行実行されるかを管理する。
    private int BeforeLenth;

    //  現在のシナリオ行数
    private int scenarioNum;

    //  一つ前に実行されていたキャンバスを入力する
    private GameObject CanvasTorpidity;

    //  現在表示されているキャンバス
    public int nowC { get; set; }

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
    private int count;

    //  キャンパスが移動してからキャンパスを発見するまでの時間を保存
    private float timeCount;

    //  timeCount の Debug.Log() を移動、発見後一回だけ表示させるために、spawnEvent[y] の値を格納しておく
    private int timeDisplay;

    #endregion


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
        scenarioController();
    }


    /**
     *  <summary>
     *  開始時に行われる処理
     *  </summary>
     */
    public void startMove()
    {
        //  配列に値を入れる処理
        settBook();
        settChoice();
        setTchoiceText();
        setAnimBook();
        setSpawnPoint();
		//  setSpawnChoicePoint ();

        setAllCanvas();
        setGuiArrowActive();
        setCanvasInitialPosition();


        TController = textControllerPlace.GetComponent<TextController>();
        sMan = textControllerPlace.GetComponent<ScenarioManager>();


        // アクティブなキャンバスを非アクティブにする
        for (int i = 0; i < getChoiceCanvas.Length; i++)
        {
            getChoiceCanvas[i].SetActive(false);
        }
        for (int i = 0; i < getCanvas.Length; i++)
        {
            getCanvas[i].SetActive(false);
        }

        cBack.Add(k);
        CanvasTorpidity = getCanvas[0];
        timeDisplay = spawnEvent[y];
        //  選択画面を何回連続で表示するか、値を習得
        BeforeLenth = choiceLength[j];


        //  画面初期化処理
        setBookprocessing();
        characterAnimation();


        //  現在の日付、時刻を LogData に書き出し
        textSave(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

    }


    /**
     *  <summary>
     *  メイン処理部分
     *  </summary>
     */
    public void scenarioController()
    {
        //  現在のシナリオの行数を習得
        scenarioNum = TController.maneger;

        //  シナリオの行数が遷移No.と同じ数値になったとき
		if (scenarioNum == moveTransition [j]) {
			//  ChoiceSelect が 0 以外ならチョイス画面に飛ぶ
			if (moveChoiceNoSelect [j] == 0) {
				//  tBookの処理
				setBookprocessing ();

			} else {
				//  tChoiceの処理
				choiceNoSelectController ();

            }
        }

		if (scenarioNum == count + 1) {
			spawnEventActive = false;
            count++;
		}

        //  オブジェクトの場所を変更する処理
        moveObject();

        // GuiArrowActiveの処理
        settControllerActive();

        //  アニメーションの処理
        characterAnimation();
    }


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
		if (scenarioNum == spawnEvent[y])
        {
            setAllCanvas()[nowC].transform.localPosition = spawnPointVector3[y];
            y++;
			spawnEventActive = true;
            timeCount = 0;
        }
        else if(spawnEventActive == false)
        {
            setAllCanvas()[nowC].transform.localPosition = setCanvasInitialPositionValue[nowC];
        }

        if (spawnEventActive == true)
        {
            timeCount += Time.deltaTime;
        }

        if(scenarioNum - 1 == timeDisplay && scenarioNum != 1)
        {
            Debug.Log("Time : " + timeCount);
            timeDisplay = spawnEvent[y];
            textSave("Time : " + timeCount);
        }
    }

    #endregion


    #region Main_setBook

    /**
     *  <summary>
     *  setBookに入力された処理を実行する
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


    #region Main_Choice

    /**
     *  <summary>
     *  moveChoiceNoSelect が 0 ではなかったときの操作
     *  </summary>
     */
    public void choiceNoSelectController()
    {
        
        if (choiceswitch != 999)
        {
            choiceCanvasControll();
        }
        else
        {
            //  tChoiceの終了処理
            textControllerPlace.SetActive(true);
            getChoiceCanvas[canvasNum1[beforeswitch]].SetActive(false);
            getChoiceCanvas[canvasNum2[beforeswitch]].SetActive(false);

            for(int i = 0; i < cBackSize.Count; i++)
            {
                textSave("Fontsize : " + cBackSize[i]);
                Debug.Log("Fontsize : " + cBackSize[i]);
            }

            //  tBookの処理
            setBookprocessing();

            BeforeLenth += choiceLength[j];
        }
    }


    /**
     * <summary>
     * choiceCanvas の中央制御部
     * </summary>
     */
    public void choiceCanvasControll() {
        // クリックしてもキャンバスの文字送りが行われないようにする。
        textControllerPlace.SetActive(false);
        //  tChoiceの処理
        if (k != 0)
        {
            getChoiceCanvas[canvasNum1[k - 1]].SetActive(false);
            getChoiceCanvas[canvasNum2[k - 1]].SetActive(false);
        }
        getChoiceText[canvasNum1[k]].fontSize = fontSize1[k];
        getChoiceText[canvasNum2[k]].fontSize = fontSize2[k];
        getChoiceCanvas[canvasNum1[k]].SetActive(true);
        getChoiceCanvas[canvasNum2[k]].SetActive(true);
        nowC = canvasNum1[k] + getCanvas.Length;
        getChoiceText[canvasNum1[k]].text = choiceText[cNo[k]];
        getChoiceText[canvasNum2[k]].text = choiceText[cNo[k]];
        //  moveChoiceNoSelectが2なら、キャンバスと選択肢を同時に表示する
        if (moveChoiceNoSelect[j] != 2)
        {
            getCanvas[moveCanvas[j]].SetActive(false);
            CanvasTorpidity.SetActive(false);
        }

        if (cBack.Count != 1)
        {
            if (Input.GetMouseButtonDown(1))
            {
                k = cBack[cBack.Count - 2];
                cBack.RemoveAt(cBack.Count - 1);
                cBackSize.RemoveAt(cBack.Count - 1);
            }
        }
    }

    #endregion


    #region GuiArrow

    /**
     *  <summary>
     *  文字を表示しているキャンバスが視界外へ行った際、
     *  テキストを進めることが出来なくする。
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

        sysArray.Resize(ref moveTransition, tdLen);
        sysArray.Resize(ref moveCanvas, tdLen);
        sysArray.Resize(ref moveChoiceNoSelect, tdLen);
        sysArray.Resize(ref choiceLength, tdLen);
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
     *  transitionPlace 下の TransitionChoiceの値を習得する。
     *  </summary>
     *  
     *  <param name ="tChoice.choiceData[].debugNo">
     *       現在の配列番号、tBook.data[].debugChoiceLength
     *        と合わせて使う
     *  </param>
     *  
     *  <param name ="cNO">
     *      choiceText 内のどのテキスト文を使うか
     *  </param>
     *  
     *  <param name ="canvasNum">
     *      文字を挿入したいキャンバスその１が入った
     *       GetCanvas の値を入力する。
     *  </param>
     *  
     *  <param name ="fontSize">
     *      キャンバスその１のフォントサイズ切り替え
     *  </param>
     *  
     *  <param name ="canvasNum2">
     *      文字を挿入したいキャンバスその２が入った
     *       GetCanvas の値を入力する。
     *  </param>
     *  
     *  <param name ="fontSize2">
     *      キャンバスその２のフォントサイズ切り替え
     *  </param>
     *  
     */
    public void settChoice()
    {
        //  配列の初期化処理
        tChoice = transitionPlace.GetComponent<TransitionChoice>();

        var tcLen = tChoice.choiceData.Length;

        sysArray.Resize(ref cNo, tcLen);
        sysArray.Resize(ref canvasNum1, tcLen);
        sysArray.Resize(ref fontSize1, tcLen);
        sysArray.Resize(ref NextNo1, tcLen);
        sysArray.Resize(ref canvasNum2, tcLen);
        sysArray.Resize(ref fontSize2, tcLen);
        sysArray.Resize(ref NextNo2, tcLen);

        for (int i = 0; i < tcLen; i++)
        {
            tChoice.choiceData[i].debugNo = i;
            cNo[i] = tChoice.choiceData[i].choiceNo;
            canvasNum1[i] = tChoice.choiceData[i].canvasNum1;
            fontSize1[i] = tChoice.choiceData[i].FontSize1;
            nextNo1[i] = tChoice.choiceData[i].nextChoiceNo1;
            canvasNum2[i] = tChoice.choiceData[i].canvasnum2;
            fontSize2[i] = tChoice.choiceData[i].FontSize2;
            nextNo2[i] = tChoice.choiceData[i].nextChoiceNo2;
        }
    }


    /**
     *  <summary>
     *  transitionPlace 下の TransitionChoiceTextの値を習得する。
     *  </summary>
     *  
     *  <param name = "choiceText">
     *      選択肢に表示する文章を習得する
     *  </param>
     *  
     */
    public void setTchoiceText()
    {
        //配列の初期化処理
        tChoiceText = transitionPlace.GetComponent<TransitionChoiceText>();

        var tctLen = tChoiceText.choiceTextData.Length;

        sysArray.Resize(ref choiceText, tctLen);

        for (int i = 0; i < tctLen; i++)
        {
            choiceText[i] = tChoiceText.choiceTextData[i].choiceText;
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


    /**
     * <summary>
     *  transitionPlace 下の transitionSpawnChoicePointの値を習得する。
     * </summary>
     */
     /*
    public void setSpawnChoicePoint()
    {
        tSpawnChoicePoint = transitionPlace.GetComponent<TransitionSpawnChoicePoint>();

        var spChoiceLen = tSpawnChoicePoint.spawnChoicePointData.Length;

        sysArray.Resize(ref spawnChoicePointEvent, spChoiceLen);
        sysArray.Resize(ref spawnChoicePointVector3, spChoiceLen);

        for (int i = 0; i < spChoiceLen; i++)
        {
            tSpawnChoicePoint.spawnChoicePointData[i].dataLength = i;
            spawnChoicePointEvent[i] = tSpawnChoicePoint.spawnChoicePointData[i].eventNum;
            spawnChoicePointVector3[i] = tSpawnChoicePoint.spawnChoicePointData[i].spawnChoicePoint;
        }
    }
    */

    #endregion
    
    
    #region Transition 以外の配列処理
    
    /**
     *  <summary>
     *  読み取り専用オブジェクトallcanvasを作成
     *  </summary>
     */
    public GameObject[] setAllCanvas()
    {
        GameObject[] allCanvas = null;
        //配列の初期化処理

        var gcLen = getCanvas.Length;
        var gccLen = getChoiceCanvas.Length;

        sysArray.Resize(ref allCanvas, gcLen + gccLen);

        for (int i = 0; i < gcLen + gccLen; i++)
        {
            if (i < gcLen)
            {
                allCanvas[i] = getCanvas[i];
            }
            else
            {
                allCanvas[i] = getChoiceCanvas[i - gcLen];
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
