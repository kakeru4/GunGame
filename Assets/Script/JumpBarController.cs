using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBarController : MonoBehaviour
{

    Slider slider;

    public float value = 10f;

    // Use this for initialization
    void Start ()
    {
        // スライダーを取得する
        slider = GameObject.Find("JumpSlider").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(value);
        slider.value = value;
    }
}
