using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllow : MonoBehaviour{

    public Transform target;
    public float smoothing = 5f; //係数（カメラの位置に関連→始点終点間） 

    Vector3 offset;

    void Start()
    {
        //オフセットの設定
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        //カメラポジションを設定。
        Vector3 targetCamPos = target.position + offset;
        /* カメラ追従の設定。
         * 始点（プレイヤーの位置）から終点（カメラポジション）まで
         * 1フレーム毎の時間（Time.deltaTime）経過による、カメラの動き。
         * 係数を上げると終点に近づく。
         */
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

    }

}