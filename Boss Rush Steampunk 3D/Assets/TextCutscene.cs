using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextCutscene : MonoBehaviour
{
	public TMP_Text output;
	public TMP_Text output2;

	public CanSelect[] selectables;

	public GameObject[] outputs;

	public int section;
	public List<string[]> text2 = new List<string[]>();
	public int speaker;

	public GameObject[] scenes;

	public int scene;

	public bool dialog;

	private bool done;

    // Start is called before the first frame update
    void Start()
    {
		speaker = 0;
		text2 = new List<string[]>();
		GetTextSetter(SceneManager.GetActiveScene().name, 0, 0);
		scene = 0;
		section = 0;
		if(!dialog){
		for(int i = 0; i < 9;  i++){
			scenes[i].SetActive(false);
		}
		scenes[scene].SetActive(true);
		} else {
			for(int i = 0; i < selectables.Length; i++){
				selectables[i].canSelect = false;
			}
		}
		done = false;
    }

    // Update is called once per frame
    void Update()
    {	
		if(!done && section < text2.Count){
		if(dialog){
				for(int i = 0; i < selectables.Length; i++){
					selectables[i].canSelect = false;
				}
			if(speaker == 0){
			output.text = text2 [section][0];
			output2.text = "";
			} else {
			output2.text = text2 [section][0];
			output.text = "";
			}
		} else {
		output.text = text2 [section][0];
		}
		}
    }

	public void next(){
		section++;
		if(section < text2.Count){
		speaker = int.Parse(text2 [section][1]);
		}
		if(section >= text2.Count){
			if(!dialog){
			scene++;
			if(scene > 8){
				SceneManager.LoadSceneAsync("OilmancerScene");
				scene--;
				section = text2.Count - 1;
			}
			} else {
				this.enabled = false;
				done = true;
			}
			text2 = new List<string[]>();
			section = 0;
			if(!dialog){
			for(int i = 0; i < scenes.Length;  i++){
				scenes[i].SetActive(false);
			}
			scenes[scene].SetActive(true);
		}
			if(!dialog){
			GetTextSetter(SceneManager.GetActiveScene().name, scene, 0);
			} else {
				done = true;
				section = 0;
				for(int i = 0; i < outputs.Length; i++){
					outputs[i].SetActive(false);
				}
				for(int i = 0; i < selectables.Length; i++){
					selectables[i].canSelect = true;
				}
			}
			//SetText(scene, 0);
		}
		/*if(scene == 4)
		{
			Camera.main.backgroundColor = Color.red;
		}
		else
		{
			Camera.main.backgroundColor = Color.black;
		}*/
	}

	public void GetTextSetter(string name, int in1, int in2){
		if(name.Equals ("Story")){
			SetText(in1, in2);
		} else if(name.Equals ("OilmancerScene")){
			SetText2(in1, in2);
		}
	}
	public void SetText2(int day, int ID){
		if (day == 0) {
			text2.Add (new string[]{"Geaux: Hallo", "0", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
			text2.Add (new string[]{"Mr. Oil Mancer: It's you.", "1", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
			text2.Add (new string[]{"Mr. Oil Mancer: Look, Whatever you're here for, it isnt worth it.", "1", "0"});
			text2.Add (new string[]{"Mr. Oil Mancer: You'd never make it to the CEO, anyway.", "1", "0"});
			text2.Add (new string[]{"Geaux: nuh uh", "0", "1"});
		}
	}
	public void SetText(int day, int ID){
		if (day == 0) {
			//text2.Add (new string[]{"House: Bad news...", "1", "0"});
			text2.Add (new string[]{"House: You have Hemochromatosis.", "1", "0"});
			text2.Add (new string[]{"Geaux: Gosh darnoux!", "1", "0"});
			//text2.Add (new string[]{"Geaux: What can I do about it?", "0", "1"});
			text2.Add (new string[]{"House: Since your right arm and all of your bones have already solidified into iron...", "1", "0"});
			text2.Add (new string[]{"House: It's untreatable.", "1", "0"});
			text2.Add (new string[]{"Geaux: :(", "0", "1"});
			//text2.Add (new string[]{"", "1", "0"});
		} else if (day == 1) {
			text2.Add (new string[]{"Geaux: What am I going to tell my wife and several dozen children!!", "1", "0"});
			text2.Add (new string[]{"Geaux: How am I going to pay off my debt to John Oil and his oilers now... :(", "1", "0"});
			text2.Add (new string[]{"Geaux: At least I have my comically large revolver!", "1", "0"});
		} else if (day == 2) {
			text2.Add (new string[]{"Knock Knock (on the door)", "0", "1"});
			text2.Add (new string[]{"Geaux: Whar?", "1", "0"});
			text2.Add (new string[]{"Geaux: Oh no! It must be John Oil and the oilers!", "1", "0"});
			text2.Add (new string[]{"Geaux: How am I going to tell them that I dont have 7 quindecillion USD right now??", "0", "1"});
		} else if (day == 3) {
			//text2.Add (new string[]{"Oiler: Heyyyy~", "0", "1"});
			text2.Add (new string[]{"Oiler: Wheres my cash, bucko?", "2", "0"});
			text2.Add (new string[]{"Geaux: I don have, my hopital bills are too much! :(", "1", "0"});
			text2.Add (new string[]{"Oiler: Well then I'll take your left arm as payment!", "0", "1"});
		} else if (day == 4) {
			text2.Add (new string[]{"Slice sfx", "0", "1"});
			text2.Add (new string[]{"Geaux: Ouch! My arm :(", "0", "1"});
		} else if (day == 5) {
			text2.Add (new string[]{"House: You should be careful, I think the oilers might chop your arm off.", "0", "1"});
			text2.Add (new string[]{"House: Oh wait, I'm a little late with that.", "0", "1"});
			text2.Add (new string[]{"House: Anyway", "0", "1"});
		} else if (day == 6) {
			text2.Add (new string[]{"House: I have this time travelling watch for you...", "0", "1"});
			text2.Add (new string[]{"House: Dont question where I got it.", "0", "1"});
			text2.Add (new string[]{"House: Uhhh, I need you to go destroy the oilers.", "0", "1"});
			text2.Add (new string[]{"House: They- hello? are you even alive?", "0", "1"});
		} else if (day == 7) {
			text2.Add (new string[]{"Geaux: Huh?", "0", "1"});
			text2.Add (new string[]{"House: I was saying that the oilers are in texas!", "0", "1"});
			text2.Add (new string[]{"Geaux: Why exactly do you want me of all people to destroy them??", "0", "1"});
			text2.Add (new string[]{"House: Well they took your arm, and your bones are made of iron.", "0", "1"});
			text2.Add (new string[]{"House: I'm sure you can probably not die that much!", "0", "1"});
			text2.Add (new string[]{"Geaux: That isnt very reassuring...", "0", "1"});
			text2.Add (new string[]{"House: Also, one last thing!", "0", "1"});
			text2.Add (new string[]{"House: When you travel back in time, everything will happen (usually) exactly as it happened before you did.", "0", "1"});
			//text2.Add (new string[]{"House: The timeline will progress as if you didnt", "0", "1"});
			text2.Add (new string[]{"House: So if you remember what happened, you can try to avoid it!", "0", "1"});
			//text2.Add (new string[]{"House: You can avoid it.", "0", "1"});
			text2.Add (new string[]{"House: Also remember that the butterfly effect exists, so things might change depending on what you do.", "0", "1"});
			text2.Add (new string[]{"House: Anyway, off you go!", "0", "1"});
		} else if (day == 8) {
			text2.Add (new string[]{"*After not very long...*", "0", "1"});
			text2.Add (new string[]{"Flight Attendant: Your flight to The middle of nowhere, texas, will be landing in 1 hour!", "0", "1"});
			text2.Add (new string[]{"Geaux: Wow this place was suprisingly easy to get to!", "0", "1"});
		}
	}
}
