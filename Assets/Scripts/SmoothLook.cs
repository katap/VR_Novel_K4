using UnityEngine;
using System;
using System.Collections;

public class SmoothLook : MonoBehaviour
{
    /// <summary>
    /// 目線移動中か
    /// </summary>
    public bool Looking
    {
        get;
        private set;
    }

    // Animator.SetLookAtWeight の目線制御中のパラメータ
    float lookWeight = 1.0f;
    float lookBodyWeight = 0.3f;
    float lookHeadWeight = 0.8f;
    float lookEyeWeight = 0.7f;
    float lookClampWeight = 0.5f;
    float chaseTargetSpeed = 0.1f;

    Animator anim;
    Transform target;
    Transform eyeFocus;
    Transform defaultEyeFocus;
    float currentWeight = 0f;

    #region unity

    void Awake()
    {
        anim = GetComponent<Animator>();

        // 目線を制御するための焦点。これを移動させる事で滑らかに目線移動させる
        eyeFocus = new GameObject("eyeFocus").transform;
        eyeFocus.parent = transform;
    }

    void OnAnimatorIK()
    {
        if (Looking)
        {
            // 目線制御
            anim.SetLookAtWeight(
                currentWeight,
                lookBodyWeight,
                lookHeadWeight,
                lookEyeWeight,
                lookClampWeight
            );
            anim.SetLookAtPosition(eyeFocus.position);
        }

        if (target != null)
        {
            // 指定されたターゲットを目線で追うための処置
            eyeFocus.position += (target.position - eyeFocus.position) * chaseTargetSpeed;
        }
    }

    #endregion

    /// <summary>
    /// 目線を向ける
    /// </summary>
    public void Look(Transform target, float sec = 0.5f)
    {
        Looking = true;
        this.target = target;
        StartCoroutine(ChangeWeight(lookWeight, sec));
    }

    /// <summary>
    /// 目線制御を終える
    /// </summary>
    public void EndLook(float sec = 0.5f)
    {
        target = null;
        StartCoroutine(ChangeWeight(0f, sec, () =>
        {
            Looking = false;
        }));
    }

    /// <summary>
    /// Animator.SetLookAtWeight の weight を滑らかに遷移させる
    /// </summary>
    IEnumerator ChangeWeight(float toValue, float sec, Action onFinished = null)
    {
        if (sec <= 0f)
        {
            sec = 0.01f;
        }
        float plusWeightPerSec = (toValue - currentWeight) / sec;
        while (true)
        {
            currentWeight += plusWeightPerSec * Time.deltaTime;
            if ((plusWeightPerSec > 0f && currentWeight >= toValue)
                || (plusWeightPerSec <= 0f && currentWeight <= toValue))
            {
                currentWeight = toValue;
                if (onFinished != null)
                {
                    onFinished();
                }
                yield break;
            }
            yield return null;
        }
    }
}