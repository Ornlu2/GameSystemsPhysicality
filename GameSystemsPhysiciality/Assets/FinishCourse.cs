using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCourse : MonoBehaviour {
    public GameObject finishtext;

    // Use this for initialization
    void Start () {
       finishtext =  GameObject.Find("FinishText");
        finishtext.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag =="Player")
        {
            finishtext.SetActive(true);
        }
    }
}
