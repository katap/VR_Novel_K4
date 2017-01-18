using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CircleCursole : MonoBehaviour {

    Image image;
    Image backImage;
    Image ChoiceImage;
    [Tooltip("サークルゲージに使う Image を入れてください。")]
    public Image circleGauge;
    [Tooltip("ゲージに使う Image をを入れるキャンパスを設定してください。 (ScreenSpace)")]
    public Canvas GaugeCanvas;
    [Tooltip("しばらく見ることで選択することが出来る、 テキストの場所を選択してください。 尚、Collider(Sphere推奨)が アタッチされている必要があります。")]
    public Text[] ChoiceText;
    public GameObject menuUI;
    public GameObject playerpos;
    public GameObject animCommand;
    private CanvasGroup cGroup;

    [HideInInspector]
    public bool gageMax;

    private bool menuActive;
    private bool menuAlpha;
    private bool one;
    Animation_Command animCom;

    RaycastHit hit;

    // Use this for initialization
    void Start () {

        renameArrayImage();

        image.fillAmount = 0;
        menuUI.SetActive(false);
        cGroup = menuUI.GetComponent<CanvasGroup>();
        cGroup.alpha = 0;
        one = false;
        animCom = animCommand.GetComponent<Animation_Command>();
    }

    // Update is called once per frame
    void Update () {
        CCController();
    }

    /**
     *  <summary>
     *  メイン処理部分
     *  </summary>
     */
     public void CCController()
    {

        var frontPosition = playerpos.transform.position + playerpos.transform.forward * 3.0f;
        frontPosition.y = playerpos.transform.position.y;

        setRay();

        bottonamountMax(frontPosition);
        backamountMax();
        choiceamountMax();
    }

    /**
     *  <summary>
     *      配列に入れられた画像に名前をつけます
     *  </summary>
     */
    public void renameArrayImage()
    {
        var gCan = Instantiate(GaugeCanvas);

        image = Instantiate(circleGauge);
        image.transform.SetParent(gCan.transform,false);

        backImage = Instantiate(circleGauge);
        backImage.transform.SetParent(gCan.transform, false);

        ChoiceImage = Instantiate(circleGauge);
        ChoiceImage.transform.SetParent(gCan.transform, false);
    }

    /**
     * <summary>
     *   Rayを発生させます
     *  </summary>
     */
    public void setRay()
    {
        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.rotation *
            Vector3.forward);

        //  視線の先にオブジェクトがあるか
        if (Physics.Raycast(ray, out hit))
        {
            catchObject();
        }
        else
        {
            image.fillAmount -= 0.02f;
            backImage.fillAmount -= 0.02f;
            ChoiceImage.fillAmount -= 0.02f;
        }
    }

    /**
     * <summary>
     *      視線の先にオブジェクトがあったときの動作
     * </summary>
     */
    public void catchObject()
    {
        //メニューが開いていないとき
        if (menuActive == false)
        {
            amountImage("Button", image);
        }

        amountImage("Back", backImage);

        amountImage("Choice", ChoiceImage);
    }

    /**
     * <summary>
     *      Bottonタグがついたオブジェクトをしばらく見た後の操作
     * </summary>
     */
    public void bottonamountMax( Vector3 frontPosition )
    {
        //"Botton"タグが付いたオブジェクトをしばらく見たとき
        //配列の0番目の画像を使用する
        if (image.fillAmount >= 1)
        {
            gageMax = true;
        }
        else
        {
            gageMax = false;
        }
        if (gageMax)
        {
            menuUI.SetActive(true);
            menuUI.transform.position = frontPosition;
            menuActive = true;
            image.fillAmount = 0;
        }
        if (menuActive)
        {
            cGroup.alpha += 0.02f;
        }
    }

    /**
     * <summary>
     *      Backタグが付いたオブジェクトをしばらく見た後の操作
     * </summary>
     */
    public void backamountMax()
    {
        //"Back"タグが付いたオブジェクトをしばらく見たとき
        //配列の1番目の画像を使用する
        if (backImage.fillAmount >= 1)
        {
            menuAlpha = true;
            menuActive = false;
        }

        if (menuAlpha)
        {
            cGroup.alpha -= 0.02f;
            image.fillAmount = 0;
            backImage.fillAmount = 0;
        }

        if (cGroup.alpha == 0)
        {
            menuAlpha = false;
            menuUI.SetActive(false);
        }
    }

    /**
     * <summary>
     *      Choiceタグが付いたオブジェクトをしばらく見た後の操作
     * </summary>
     */
    public void choiceamountMax()
    {
        //"Choice"タグが付いたオブジェクトをしばらく見たとき
        //配列の2番目の画像を使用する
        if (ChoiceImage.fillAmount >= 1)
        {
            one = true;
            ChoiceImage.fillAmount = 0;
        }
        if (one)
        {
            one = false;

            for (int i = 0; i < ChoiceText.Length; i++)
            {
                if (hit.collider.gameObject.name == ChoiceText[i].name)
                {
                    string objectName = hit.collider.gameObject.name;
                    Debug.Log( objectName + ":" + ChoiceText[i].fontSize);
                    animCom.k += 1;
                }
            }
        }
    }

    /**
     *  <summary>
     *      画像の中心を起点に、360度になるまで少しづつ表示させる
     *  </summary>
     * 
     *  <param name = "str">
     *  タグの名前を入力します
     *  </param>
     *  
     *  <param name = "img">
     *  表示させる画像の場所を指定します(重複不可)
     *  </param>
     * 
     */
    public void amountImage( string str, Image img )
    {
        if (hit.transform.gameObject.tag == str )
        {
            img.fillAmount += 0.02f;
        }
        else
        {
            img.fillAmount -= 0.02f;
        }
    }
}
