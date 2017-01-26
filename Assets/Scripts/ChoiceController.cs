using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using sysArray = System.Array;

[RequireComponent(typeof(CentralController))]
public class ChoiceController : MonoBehaviour
{
    #region inspecter

    

    CentralController cCon;
    TransitionChoiceText tChoiceText;

    #endregion


    #region TransitionChoice習得用関数群

    //  TransitionChoice習得用関数群
    private int[] cNo;
    private int[] canvasNum1;
    private int[] fontSize1;

    private int[] NextNo1;
    public int[] nextNo1
    {
        get { return NextNo1; }
        private set { NextNo1 = value; }
    }

    private int[] canvasNum2;
    private int[] fontSize2;

    private int[] NextNo2;
    public int[] nextNo2
    {
        get { return NextNo2; }
        private set { NextNo2 = value; }
    }

    TransitionChoice tChoice;

    //  transitionChoiceが何行実行されるかを管理する。
    private int BeforeLenth;

    //  現在の TransitionChoice の行数
    //  CircleCursole 内でプラス処理を行う
    [System.NonSerialized]
    public int k;

    //  TransitionChoice の Back 処理
    [System.NonSerialized]
    public List<int> cBack = new List<int>();
    //  TransitionChoice が有効になっている際に、Back処理が行われても
    //  最終的に辿ってきた FontSize の正常な値を返すために一時保存するための関数
    [System.NonSerialized]
    public List<int> cBackSize = new List<int>();

    //  TransitionChoiceText習得用関群
    private string[] choiceText;

    [System.NonSerialized]
    public int choiceswitch;
    [System.NonSerialized]
    public int beforeswitch;


    #endregion


    #region choiceStartMove

    public void choiceStartMove()
    {
        cCon = GetComponent<CentralController>();

        setTchoiceText();
        settChoice();
        // アクティブなキャンバスを非アクティブにする

        for (int i = 0; i < cCon.getChoiceCanvas.Length; i++)
        {
            cCon.getChoiceCanvas[i].SetActive(false);
        }

        //  選択画面を何回連続で表示するかの値を習得
        BeforeLenth = cCon.choiceLength[cCon.j];
        cBack.Add(k);

    }

    #endregion


    #region ChoiceMainMove

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
            mouseclick();
        }
        else
        {
            choiceCanvasFinish();
        }
    }

    /**
     * <summary>
     * tChoiceの終了処理
     * </summary>
     */
    public void choiceCanvasFinish()
    {
        cCon.textControllerPlace.SetActive(true);
        cCon.getChoiceCanvas[canvasNum1[beforeswitch]].SetActive(false);
        cCon.getChoiceCanvas[canvasNum2[beforeswitch]].SetActive(false);

        for (int i = 0; i < cBackSize.Count; i++)
        {
            cCon.textSave("Fontsize : " + cBackSize[i]);
            Debug.Log("Fontsize : " + cBackSize[i]);
        }

        //  tBookの処理
        cCon.setBookprocessing();

        BeforeLenth += cCon.choiceLength[cCon.j];
    }


    /**
     * <summary>
     * choiceCanvas の中央制御部
     * </summary>
     */
    public void choiceCanvasControll()
    {
        // クリックしてもキャンバスの文字送りが行われないようにする。
        cCon.textControllerPlace.SetActive(false);
        //  tChoiceの処理
        if (k != 0)
        {
            cCon.getChoiceCanvas[canvasNum1[k - 1]].SetActive(false);
            cCon.getChoiceCanvas[canvasNum2[k - 1]].SetActive(false);
        }
        cCon.getChoiceText[canvasNum1[k]].fontSize = fontSize1[k];
        cCon.getChoiceText[canvasNum2[k]].fontSize = fontSize2[k];
        cCon.getChoiceCanvas[canvasNum1[k]].SetActive(true);
        cCon.getChoiceCanvas[canvasNum2[k]].SetActive(true);
        cCon.nowC = canvasNum1[k] + 1 + cCon.getCanvas.Length;
        cCon.getChoiceText[canvasNum1[k]].text = choiceText[cNo[k]];
        cCon.getChoiceText[canvasNum2[k]].text = choiceText[cNo[k]];
        //  moveChoiceNoSelectが2なら、キャンバスと選択肢を同時に表示する
        if (cCon.moveChoiceNoSelect[cCon.j] != 2)
        {
            cCon.getCanvas[cCon.moveCanvas[cCon.j]].SetActive(false);
            cCon.CanvasTorpidity.SetActive(false);
        }
    }

    public void mouseclick()
    {
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


    #region transition

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
        tChoice = cCon.transitionPlace.GetComponent<TransitionChoice>();

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
        tChoiceText = cCon.transitionPlace.GetComponent<TransitionChoiceText>();

        var tctLen = tChoiceText.choiceTextData.Length;

        sysArray.Resize(ref choiceText, tctLen);

        for (int i = 0; i < tctLen; i++)
        {
            choiceText[i] = tChoiceText.choiceTextData[i].choiceText;
        }
    }

    #endregion
}