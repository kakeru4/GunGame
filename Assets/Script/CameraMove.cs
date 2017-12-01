using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public float smooth = 3.0f; // カメラ動作のスムーズ係数     
    GameObject NewPos;
    GameObject FirstPerson;
    public Transform Enemy;
    bool FirstPersonMode=false;

    // Use this for initialization
    void Start()
    {
        NewPos = GameObject.Find("CamPos"); //三人称視点用

        FirstPerson = GameObject.Find("FirstPerson");　//一人称視点用

        transform.position = NewPos.transform.position;

        transform.forward = NewPos.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (!FirstPersonMode)
        {
            transform.position = Vector3.Lerp(transform.position, NewPos.transform.position, Time.deltaTime * smooth);
            transform.forward = Vector3.Lerp(transform.forward, NewPos.transform.forward, Time.deltaTime * smooth);
           // transform.LookAt(Enemy);
        }

        if(FirstPersonMode)
        {
            transform.position = Vector3.Lerp(FirstPerson.transform.position, FirstPerson.transform.position, Time.deltaTime * smooth);
            transform.forward = Vector3.Lerp(FirstPerson.transform.forward, FirstPerson.transform.forward, Time.deltaTime * smooth);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            FirstPersonMode = true;
        }

        if(Input.GetButtonDown("Fire4"))
        {
            FirstPersonMode = false;
        }
    }

}
