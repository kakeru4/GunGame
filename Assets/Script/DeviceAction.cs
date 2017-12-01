using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceAction : MonoBehaviour {

    static public bool isMobile;

	// Use this for initialization
	void Start () {
        isMobile = false;
        if(Application.platform==RuntimePlatform.IPhonePlayer||Application.platform==RuntimePlatform.Android)
        {
            isMobile = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
