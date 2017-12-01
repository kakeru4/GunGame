using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverScene : MonoBehaviour {


    public string OverSceneName;

    public string SceneName;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var PlayerHp = GetComponent<PlayerHpBarControl>().hp;

        if (PlayerHp <= 0)
        {
            SceneManager.LoadScene(OverSceneName);
        }

        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneName);
        }

    }
}
