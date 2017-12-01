using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour {

    // bullet prefab
    public GameObject bullet;

    public GameObject flash;

    // 弾丸発射点
    public Transform muzzle;

    // 弾丸の速度
    public float speed = 1000;

    //弾の最大装填数
    public int gun_num = 10;

    //ここに指定された速度以下になった、このオブジェクトは削除される。
    public float minLimit = 100;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // z キーが押された時
        if (Input.GetButtonDown("Fire1") && gun_num != 0)
        {
           // Instantiate(flash, muzzle.position,Quaternion.identity);
            (flash.GetComponent<ParticleSystem > () as ParticleSystem).Play();
            // 弾丸の複製
            GameObject bullets = GameObject.Instantiate(bullet) as GameObject;
            // 弾丸の位置を調整
            bullets.transform.position = muzzle.position;
            Vector3 force;
            force = this.gameObject.transform.forward * speed;
            //Rigidbodyに力を加えて発射
            bullets.GetComponent<Rigidbody>().AddForce(force);
            gun_num--;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            gun_num = 10;
        }

        ////現在速度を取得する。
        //Vector3 nowSpeed = GetComponent<Rigidbody>().velocity;

        ////現在の速度が指定値以下になったら
        //if (nowSpeed.sqrMagnitude <= minLimit)
        //{
        //    //Destroy(bullet);
        //}
    }
}
