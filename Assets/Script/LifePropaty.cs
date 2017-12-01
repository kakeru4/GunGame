using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePropaty : MonoBehaviour {
    public float Life = 1.0f;
    private float _maxLife = 1.0f;
    public float MaxLife
    {
        get { return _maxLife; }
    }

    // Use this for initialization
    void Start()
    {
        //ライフの初期値を最大値ということにして記憶。
        _maxLife = Life;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //回復処理はこの関数を通す。ライフ値が初期のライフ値以上にならないようにしている
    public void Recover(float inc)
    {
        Life += inc;
        Life = (Life > MaxLife) ? MaxLife : Life;
    }

}
