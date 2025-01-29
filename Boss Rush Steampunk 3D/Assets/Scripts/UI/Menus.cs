using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void toMain(){
		SceneManager.LoadSceneAsync("StoryScene");
	}


    // Update is called once per frame
    void Update()
    {
        
    }
}
