using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarControl : MonoBehaviour {

    Slider slider;
    public float hp = 10f;
    Animator anim;

    void Start()
    {
        // スライダーを取得する
        slider = GameObject.Find("EnemySlider").GetComponent<Slider>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // HPゲージに値を設定
        slider.value = hp;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp -= 1.0f;
            anim.SetTrigger("Damage");
        }
        slider.value = hp;
    }

}
