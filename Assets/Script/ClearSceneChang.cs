using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ClearSceneChang : MonoBehaviour {

    public string ClearSceneName;

    public string SceneName;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var EnemyHp = GetComponent<EnemyHpBarControl>().hp;

        if (EnemyHp <= 0)
        {
            SceneManager.LoadScene(ClearSceneName);
        }

        //if (Input.GetButtonDown("Submit"))
        //{
        //    SceneManager.LoadScene(SceneName);
        //}

    }
}
