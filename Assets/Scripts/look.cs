// define LOOKATをコメント化でルックアット方式、コメント化で角度指定で回転
#define LOOKAT

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class look : MonoBehaviour
{

#if LOOKAT
    //[Tooltip("追跡する対象")]
    //public Transform target;

#else
	[Tooltip("本体の回転")]
	public int transformAngle = 0;

#endif
    [Tooltip("青線の回転")]
    public int targetAngle = 90;

    [Tooltip("青線を基準とした緑線の回転")]
    public int extraTargetAngle = 0;

    public GameObject animationCommancPlace;
    Animation_Command animCom;

    void Start()
    {
        animCom = animationCommancPlace.GetComponent<Animation_Command>();
    }

    void Update()
    {

#if LOOKAT
        // 対象方向を向く

        var diff = (animCom.setAllCanvas()[animCom.nowC].transform.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        

#else
		// スプライトをtransformAngleで指定角に回転

		transformAngle = transformAngle % 360;
		transform.rotation = Quaternion.AngleAxis(transformAngle, -transform.forward);

#endif
    }

    void OnDrawGizmos()
    {
        // 角度をリセットする

        targetAngle = targetAngle % 360;
        extraTargetAngle = extraTargetAngle % 360;

        // 向いている方向のベクトルを取得

        var dir1 = transform.up;
        DrawRay(dir1, Color.red);

        // 青線をtargetAngleで指定した角度に回転
        // 上をむいたベクトルを指定角の行列で回転させる

        var blueLineAngle = Quaternion.AngleAxis(targetAngle, -transform.forward);
        var dir2 = blueLineAngle * dir1;
        DrawRay(dir2, Color.blue);

        // 青線に角度を追加して緑線を描画。二種類の方法
        //
        //  		(#1)青線のベクトルをさらに行列で回転させる。　
        //		(#2)行列を合成して回転させる 

        var qt2 = Quaternion.AngleAxis(extraTargetAngle, -transform.forward);

        var dir3 = qt2 * dir2;                      //  (#1)
                                                    //var dir3 = (blueLineAngle * qt2) * dir1;		//  (#2)

        DrawRay(dir3, Color.green);
    }

    /// <summary>
    /// 現在地から線を引く
    /// </summary>
    /// <param name="direction">Direction.</param>
    /// <param name="color">Color.</param>
    void DrawRay(Vector3 direction, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(transform.position, direction);
    }
}