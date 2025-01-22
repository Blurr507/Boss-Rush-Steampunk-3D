using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
		for(int i = 0; i < 9;  i++){
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
			if(scene > 8){
				SceneManager.LoadSceneAsync("OilmancerScene");
				scene--;
			}
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
			text2.Add (new string[]{"House: Nothing!", "1", "0"});
			text2.Add (new string[]{"Geaux: Bruh.", "0", "1"});
			//text2.Add (new string[]{"", "1", "0"});
		}else if (day == 1) {
			text2.Add (new string[]{"Geaux: What am I going to tell my wife and several dozen children of various ages and genders!!", "1", "0"});
			text2.Add (new string[]{"Geaux: How am I going to pay off my debt to George W Bush and his oilers now... :(", "1", "0"});
			text2.Add (new string[]{"Geaux: At least I have my comically large revolver!", "1", "0"});
		} else if (day == 2) {
			text2.Add (new string[]{"Knock Knock (on the door)", "0", "1"});
			text2.Add (new string[]{"Geaux: Eh?", "1", "0"});
			text2.Add (new string[]{"Geaux: Oh no! It must be George W. Bush and the oilers!", "1", "0"});
			text2.Add (new string[]{"Geaux: How am I going to tell them that I dont have 7 quindecillion USD right now??", "0", "1"});
		} else if (day == 3) {
			text2.Add (new string[]{"Redacted: Heyyyy~", "0", "1"});
			text2.Add (new string[]{"Redacted: Wheres my cash, bucko?", "2", "0"});
			text2.Add (new string[]{"Geaux: I dont have it, my hopital bills are too much! :(", "1", "0"});
			text2.Add (new string[]{"Redacted: Well then I'll take your left arm as payment!", "0", "1"});
		} else if (day == 4) {
			text2.Add (new string[]{"Slice sfx", "0", "1"});
			text2.Add (new string[]{"Geaux: AEUGHHGHHHHHH", "0", "1"});
		} else if (day == 5) {
			text2.Add (new string[]{"House: You should be careful, I think the oilers might chop your arm off.", "0", "1"});
			text2.Add (new string[]{"House: Oh wait, I'm a little late with that.", "0", "1"});
			text2.Add (new string[]{"House: Anyway...", "0", "1"});
		} else if (day == 6) {
			text2.Add (new string[]{"House: I have this time travelling watch for you...", "0", "1"});
			text2.Add (new string[]{"House: Dont question where I got it.", "0", "1"});
			text2.Add (new string[]{"House: Uhh, I need you to go destroy the oilers!", "0", "1"});
			text2.Add (new string[]{"House: They- hello? are you even alive?", "0", "1"});
		} else if (day == 7) {
			text2.Add (new string[]{"Geaux: Huh?", "0", "1"});
			text2.Add (new string[]{"House: I was saying that the oilers are in texas!", "0", "1"});
			text2.Add (new string[]{"Geaux: Why exactly do you want me of all people to destroy them??", "0", "1"});
			text2.Add (new string[]{"House: Well they took your arm, and your bones are made of iron.", "0", "1"});
			text2.Add (new string[]{"House: I'm sure you can beat them!", "0", "1"});
			text2.Add (new string[]{"Geaux: Okay..?", "0", "1"});
		} else if (day == 8) {
			text2.Add (new string[]{"*After not very long...*", "0", "1"});
			text2.Add (new string[]{"Flight Attendant: Your flight to The middle of nowhere, texas, will be landing in 1 hour!", "0", "1"});
			text2.Add (new string[]{"Geaux: Wow this place was suprisingly easy to get to!", "0", "1"});
		}
	}
}
