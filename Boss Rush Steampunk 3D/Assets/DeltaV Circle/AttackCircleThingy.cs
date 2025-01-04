using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCircleThingy : MonoBehaviour
{
	public static bool going;

	public RectTransform spinner;

	private float dir;
    // Start is called before the first frame update
    void Start()
    {
		going = false;
    }

	public void startAttack(){
		going = true;
	}

    // Update is called once per frame
    void Update()
    {
		dir = spinner.rotation.eulerAngles.z - 90; 
		if(Input.GetKeyDown("space")){
			going = false;
			if(dir >= 80 && dir <= 100){
				//attack small
				Debug.Log("attack small");
			} else if(dir >= 175 && dir <= 185){
				//attack medium
				Debug.Log("attack medium");
			} else if(dir >= 267.5 && dir <= 272.5){
				//attack strong
				Debug.Log("attack strong");
			}
			//wait a second before reseting
			Invoke("reset", 1f);
		}
    }

	void reset(){
		//reset spinner
		spinner.Rotate(0, 0, ((0 - dir) * 1f));
	}

	void FixedUpdate(){
		//check if going
		if(going){
			//if going, output the direction of the thing for debug purposes
			//then, rotate the spinner
			Debug.Log(dir);
			spinner.Rotate(0, 0, 2f);
			//check if the spinner spun too far, and reset if it has
			if(dir < 0 && dir >= -6){
				going = false;
				//janky math
				spinner.Rotate(0, 0, ((0 - dir) * 0.5f) + 1);
			}
		}
	}
}
