using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //仮想スティック運用に必要
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float foreSpeed = 9.0f;
    public float backSpeed = 2.0f;
    public float rotSpeed = 0.5f;
    public float jumpPower = 3.8f;
    public LayerMask GroundLayer; //指定レイヤー
    public Transform EnemyPos;
    Slider slider;
    private float JumpValue = 10f;
    Vector3 velocity;// 移動量

    Animator myAnim;//モーションツリー
    Rigidbody myRB;//リジッドボディ


    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        slider = GameObject.Find("JumpSlider").GetComponent<Slider>();
        myAnim.speed = 1.5f;
        myRB.useGravity = true;
    }

    void Update()
    {
        float h;
        float v;
        float mx;
        float my;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mx = Input.GetAxis("Mouse X");
        my = Input.GetAxis("Mouse Y");



        myAnim.SetFloat("Speed", v);
        // myAnim.SetFloat("Speed", h);
        myAnim.SetFloat("Direction", mx);
        myAnim.SetFloat("Direction", my);
        velocity = new Vector3(h, 0, v);
        velocity = transform.TransformDirection(velocity);

        //Debug.Log(JumpValue);
        slider.value = JumpValue;

        if (v > 0.1)
        {
            velocity *= foreSpeed;
        }
        else if (v < -0.1)
        {
            velocity *= backSpeed;
        }


        transform.localPosition += velocity * Time.deltaTime;
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(0, mx * rotSpeed, 0);
        }

        // transform.LookAt(EnemyPos);
        if (Input.GetKey(KeyCode.Space))
        {
            myRB.useGravity = false;
            transform.localPosition += new Vector3(0, 0.2f, 0);
            StartCoroutine("JumpAction");

        }

        if (!myRB.useGravity)
        {
            JumpValue -= 0.1f;
        }

        if (JumpValue <= 0)
        {
            myRB.useGravity = true;
            transform.localPosition += new Vector3(0, -0.2f, 0);
        }

        if (IsGround())
        {
            JumpValue = 10f;
        }

        if (Input.GetKey(KeyCode.M))
        {
            myRB.useGravity = true;
            AvoidEmergencies();
        }
    }

    bool IsGround()
    { // 接地をブール値で返す関数         
        RaycastHit hit;
        return Physics.SphereCast(  // 下向きに球を投げて指定レイヤーと当たるか？             
    transform.position + Vector3.up * 0.5f, 0.05f, -Vector3.up, out hit, 0.55f, GroundLayer);
    }

    //緊急回避する関数
    void AvoidEmergencies()
    {
        myAnim.SetTrigger("Avoid");
    }

    IEnumerator JumpAction()
    {
        if (IsGround())
        {
            if (!myRB.useGravity)
            {

                myAnim.SetTrigger("Jump");
                //* Time.deltaTime;
                yield return new WaitForSeconds(0.15f);
                // myRB.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            }
        }
    }

    public void jumpTrigger()
    {
        StartCoroutine("JumpAction");
    }
}
