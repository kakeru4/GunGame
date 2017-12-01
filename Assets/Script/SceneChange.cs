using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public string SceneName;

    public string ClearSceneName;

    public string OverSceneName;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        //var PlayerHp= GetComponent<PlayerHpBarControl>().hp;

        //var EnemyHp = GetComponent<EnemyHpBarControl>().hp;

        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneName);
        }

        //if(PlayerHp<=0)
        //{
        //    SceneManager.LoadScene(ClearSceneName);
        //}

        //if(EnemyHp<=0)
        //{
        //    SceneManager.LoadScene(OverSceneName);
        //}
    }
}
