using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using sysArray = System.Array;
using System.IO;

public class Animation_Command : MonoBehaviour
{
    #region Inspector

    //  Inspector上のオブジェクト習得用関数群
    [Tooltip("制御したい Text Controller がある Hieralcy オブジェクトを入れてください。")]
    public GameObject textControllerPlace;

    [Tooltip("Transition ○○ を入れてある Hieralcy オブジェクトを入れてください。")]
    public GameObject transitionPlace;

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

    [Tooltip("制御したい GuiArrow がある Hieralcy オブジェクトを入れてください。")]
    public GameObject gArrow;

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
    [System.NonSerialized]
    public int[] nextNo1;
    private int[] canvasNum2;
    private int[] fontSize2;
    [System.NonSerialized]
    public int[] nextNo2;

    //  TransitionChoiceText習得用関群
    [HideInInspector]
    public string[] choiceText;

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
    [System.NonSerialized]
    public int[] spawnEvent;
    private Vector3[] spawnPointVector3;
    [System.NonSerialized]
    public bool[] aActive;
    /*
	//  TransitionSpawnChoicePoint習得用関数群
	private int[] spawnChoicePointEvent;
    private Vector3[] spawnChoicePointVector3;
    */
    [System.NonSerialized]
    public int choiceswitch;
    [System.NonSerialized]
    public int beforeswitch;
    private CircleCursole cCursole;

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
    [System.NonSerialized]
    public GUIArrowActive[] canvasAcrive;

    #endregion


    #region BasicFunction

    //  現在のTransitionBookの行数
    private int j = 0;

    //  現在のTransitionChoiceの行数
    //  CircleCursole内でプラス処理を行う
    [System.NonSerialized]
    public int k = 0;

    //  transitionChoiceが何行実行されるかを管理する。
    private int BeforeLenth = 0;

    //  現在のシナリオ行数
    [System.NonSerialized]
    public int scenarioNum;

    //  一つ前に実行されていたキャンバスを入力する
    private GameObject CanvasTorpidity;

    //  現在表示されているキャンバス
    [System.NonSerialized]
    public int nowC;

    //  CanvasPosition の初期値を入力する
    private Vector3[] setCanvasPositionInitialValue;

    #endregion


    #region SpawnEventRelation

    //  spawnEvent の行数を管理
    [System.NonSerialized]
    public int y;

    //  scenarioNum の値と spawnEvent[y] の値が一致した時に True になる
    [System.NonSerialized]
    public bool spawnEventActive;
    //  spawnEventActive を、scenarioNum の値が変化するごとに１回だけ false にする関数に使用する
    //  この関数はmoveObjectが呼び出される前に記述する
    [System.NonSerialized]
    public int count;

    //  キャンパスが移動してからキャンパスを発見するまでの時間を保存
    [System.NonSerialized]
    private float timeCount;
    //  timeCount の Debug.Log() を移動、発見後一回だけ表示させるために、spawnEvent[y] の値を格納しておく
    [System.NonSerialized]
    public int timeDisplay;

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
        TController = textControllerPlace.GetComponent<TextController>();
        cCursole = GetComponent<CircleCursole>();
        //  最初に表示するキャンパスを格納
        CanvasTorpidity = getCanvas[0];

        nowC = 0;

		count = 0;

        choiceswitch = 0;

        //  配列に値を入れる処理
        settBook();
        settChoice();
        setTchoiceText();
        setAnimBook();
        setSpawnPoint();
		//setSpawnChoicePoint ();
        setAllCanvas();
        setGuiArrowActive();

        timeDisplay = spawnEvent[y];

        sysArray.Resize(ref setCanvasPositionInitialValue, setAllCanvas().Length);
        for(int i = 0; i < setAllCanvas().Length; i++)
        {
            setCanvasPositionInitialValue[i] = setAllCanvas()[i].transform.localPosition;
        }
        //  選択画面を何回連続で表示するか、値を習得
        BeforeLenth = choiceLength[j];

        setBookprocessing();
        characterAnimation();

        textSave(System.DateTime.Now.ToString());

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
            /*
            if(aActive[y] == false)
            {
                gArrow.SetActive(false);
            }
            else
            {
                gArrow.SetActive(true);
            }
            */
            setAllCanvas()[nowC].transform.localPosition = spawnPointVector3[y];
            y++;
			spawnEventActive = true;
            timeCount = 0;
        }
        else if(spawnEventActive == false)
        {
            setAllCanvas()[nowC].transform.localPosition = setCanvasPositionInitialValue[nowC];
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

            //  tBookの処理
            setBookprocessing();

            BeforeLenth += choiceLength[j];
        }
        
        #region iranai
        /*
        switch (choiceswitch) {
            case 0:
                choiceCanvasControll();
                break;
            case 1:
                choiceCanvasControll();
                break;
            case 2:
                choiceCanvasControll();
                break;
            case 3:
                choiceCanvasControll();
                break;
            case 4:
                choiceCanvasControll();
                break;
            case 999:
                //  tChoiceの終了処理
                textControllerPlace.SetActive(true);
                getChoiceCanvas[canvasNum1[k - 1]].SetActive(false);
                getChoiceCanvas[canvasNum2[k - 1]].SetActive(false);

                //  tBookの処理
                setBookprocessing();

                BeforeLenth += choiceLength[j];
                break;
                
        }
        */
        /*
        //  チョイスセレクトがオンのときの処理
        if (k < BeforeLenth)
        {
            // クリックしてもキャンバスの文字送りが行われないようにする。
            textControllerPlace.SetActive(false);
            //  tChoiceの処理
			if (k != 0) {
				getChoiceCanvas [canvasNum1 [k - 1]].SetActive (false);
				getChoiceCanvas [canvasNum2 [k - 1]].SetActive (false);
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
        }
        else
        {
            //  tChoiceの終了処理
            textControllerPlace.SetActive(true);
            getChoiceCanvas[canvasNum1[k - 1]].SetActive(false);
            getChoiceCanvas[canvasNum2[k - 1]].SetActive(false);

            //  tBookの処理
            setBookprocessing();

            BeforeLenth += choiceLength[j];
        }
        // */

        #endregion
    }


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
    }


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
                    if (canvasAcrive[i]._isRendered)
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
        sysArray.Resize(ref nextNo1, tcLen);
        sysArray.Resize(ref canvasNum2, tcLen);
        sysArray.Resize(ref fontSize2, tcLen);
        sysArray.Resize(ref nextNo2, tcLen);

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
        sysArray.Resize(ref aActive, spLen);

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
        sysArray.Resize(ref canvasAcrive, sacLen);

        for (int i = 0; i < sacLen; i++)
        {
            canvasAcrive[i] = setAllCanvas()[i].GetComponent<GUIArrowActive>();
        }
    }

    public void textSave(string txt)
    {
        StreamWriter sw = new StreamWriter("LogData.txt", true); //true=追記 false=上書き
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
    }
}
