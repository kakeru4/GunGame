using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarControl : MonoBehaviour
{

    Slider slider;
    public float hp = 10f;
  

    void Start()
    {
        // スライダーを取得する
        slider = GameObject.Find("PlayerSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update () {
        // HPゲージに値を設定
        slider.value = hp;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            hp -= 1.0f;
            //Destroy(Player);
        }
        //slider.value = hp;
    }

}
