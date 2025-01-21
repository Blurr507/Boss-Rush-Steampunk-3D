using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCutscene : MonoBehaviour
{
	public TMP_Text output;
	public int section;
	public List<string[]> text2 = new List<string[]>();
	public int speaker;

	public GameObject[] scenes;

	public int scene;
    // Start is called before the first frame update
    void Start()
    {
		text2 = new List<string[]>();
		SetText(0, 0);
		scene = 0;
		for(int i = 0; i < 3;  i++){
			scenes[i].SetActive(false);
		}
		scenes[scene].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

		output.text = text2 [section][0];
    }

	public void next(){
		section++;
		if(section >= text2.Count){
			scene++;
			text2 = new List<string[]>();
			section = 0;
			for(int i = 0; i < 3;  i++){
				scenes[i].SetActive(false);
			}
			scenes[scene].SetActive(true);
			SetText(scene, 0);
		}

	}

	public void SetText(int day, int ID){
		if (day == 0) {
			text2.Add (new string[]{"House: Bad news...", "1", "0"});
			text2.Add (new string[]{"House: You have Hemochromatosis.", "1", "0"});
			text2.Add (new string[]{"Geaux: Gosh darnoux!", "1", "0"});
			text2.Add (new string[]{"Geaux: What can I do about it?", "0", "1"});
			text2.Add (new string[]{"House: Well since your right arm has already solidified into iron...", "1", "0"});
			text2.Add (new string[]{"House: Nothing...", "1", "0"});
			text2.Add (new string[]{"Geaux: Bruh", "0", "1"});
			//text2.Add (new string[]{"", "1", "0"});
		}else if (day == 1) {
			text2.Add (new string[]{"Geaux: What am I going to tell my wife and several dozen children of various ages and genders!!", "1", "0"});
			text2.Add (new string[]{"Geaux: How am I going to pay off my debt to president bush and his oilers now... :(", "1", "0"});
			text2.Add (new string[]{"Geaux: At least I have my comically large revolver!", "1", "0"});
		} else if (day == 2) {
			text2.Add (new string[]{"Knock Knock (on the door)", "0", "1"});
			text2.Add (new string[]{"Geaux: whar", "1", "0"});
			text2.Add (new string[]{"Geaux: Oh no! It must be president bush and the oilers!", "1", "0"});
			text2.Add (new string[]{"Geaux: How am I going to tell them that I dont have 7 quindecillion USD right now??", "0", "1"});
		} else if (day == 3) {
			text2.Add (new string[]{"Hello!", "0", "1"});
			text2.Add (new string[]{"...", "2", "0"});
			text2.Add (new string[]{"Your mission remains the same, destroy the enemy", "1", "0"});
			text2.Add (new string[]{"Ok.", "0", "1"});
		} 
	}
}
