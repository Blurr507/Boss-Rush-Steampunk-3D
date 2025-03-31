using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Array2DEditor;

public class TextCutscene : MonoBehaviour
{
	public TMP_Text output;
	public TMP_Text output2;
	public TMP_Text output3;

	public CanSelect[] selectables;

	public GameObject[] outputs;

	public bool currentlyTyping;

	public int section;
	public List<string[]> text2 = new List<string[]>();
	public int speaker;

	public string outp;

	public GameObject[] scenes;

	public int scene;

	public bool dialog;

	private bool done;

	public static TextCutscene me;

	//public Array2DAudioClip lines;

	public AudioClip[] out1sfx;
	public AudioClip[] out2sfx;
	public AudioClip[] out3sfx;

	public AudioSource audsrc;

	public int p;
    // Start is called before the first frame update
    void Start()
    {
		p = 0;
		me = this;
		speaker = 0;
		text2 = new List<string[]>();
		GetTextSetter(SceneManager.GetActiveScene().name, 0, 0, 0);
		scene = 0;
		section = 0;
		if(!dialog){
			for(int i = 0; i < scenes.Length;  i++){
			scenes[i].SetActive(false);
		}
		scenes[scene].SetActive(true);
		} else {
			for(int i = 0; i < selectables.Length; i++){
				selectables[i].canSelect = false;
			}
		}
		BattleStateManager.wait = true;
		done = false;
		CheckCamWarp();
		if(dialog){
			StartCoroutine(Type(text2[section][0]));
		}

    }
	public void startAgain(int t){
		gameObject.SetActive(true);
		p = t;
		text2.Clear();
		Debug.Log(text2.Count);
		//me = this;
		speaker = 0;
		text2 = new List<string[]>();
		BattleStateManager.wait = true;
		text2.Clear();
		GetTextSetter(SceneManager.GetActiveScene().name, 0, 0, t);
		Debug.Log(text2.Count + "/5");
		scene = 0;
		section = 0;
		for(int i = 0; i < outputs.Length; i++){
			outputs[i].SetActive(true);
		}
		if(!dialog){
			for(int i = 0; i < scenes.Length;  i++){
				scenes[i].SetActive(false);
			}
			scenes[scene].SetActive(true);
		} else {
			for(int i = 0; i < selectables.Length; i++){
				selectables[i].canSelect = false;
			}
		}
		done = false;
		Debug.Log("//" + done + "  / " + section + "/" + text2.Count);
		CheckCamWarp();
		if(dialog){
		StartCoroutine(Type(text2[section][0]));
		}

	}

	void CheckCamWarp(){
		if(!done && section < text2.Count){
		if(text2 [section][0].Equals ("1")){
			BattleStateManager.me.SetState(8);
			section++;
		}
		if(text2 [section][0].Equals ("2")){
			BattleStateManager.me.SetState(9);
			section++;
		}
		if(text2 [section][0].Equals ("0")){
			BattleStateManager.me.SetState(0);
			section++;
		}
		}
	}
    // Update is called once per frame
    void Update()
    {	

		if(!done && section < text2.Count){
			//Debug.Log("//" + done + "  / " + section + "/" + text2.Count);
		if(dialog){
				if(speaker != 3){
				for(int i = 0; i < selectables.Length; i++){
					selectables[i].canSelect = false;
				}
				}
			if(speaker == 0){
			output.text = outp;
			output2.text = "";
			output3.text = "";
			} else if(speaker == 1){
					output2.text = outp;
			output.text = "";
			output3.text = "";
				} else {
					for(int i = 0; i < selectables.Length; i++){
						selectables[i].canSelect = true;
					}
					output3.text = outp;
					output2.text = "";
					output.text = "";
				}
		} else {
				output.text = text2[section][0];
		}
		}
    }
	IEnumerator Type(string text){
		currentlyTyping = true;
		string outputText = "";
		outp = "";
		for (int i = 0; i < text.Length; i++) {
			if(currentlyTyping){
				if(!audsrc.isPlaying){
					if(speaker == 0){
						audsrc.clip = out1sfx[Random.Range(0, out1sfx.Length)];
					} else if(speaker == 1){
						audsrc.clip = out2sfx[Random.Range(0, out2sfx.Length)];
					} else if(speaker == 3){
						audsrc.clip = out3sfx[Random.Range(0, out3sfx.Length)];
					}



					audsrc.Play();
				}
				outp = outp + text.Substring(i, 1);
				//output.text = outputText;
				
				yield return new WaitForSeconds(0.025f);
			}
		}
		if (currentlyTyping) {
			next ();
			currentlyTyping = false;
		}
	}
	public void next(){
		//section++;
		if(dialog){
		if(!currentlyTyping){
			//if(section < text2.Count){
				section++;
			CheckCamWarp();
			if(section < text2.Count){
				speaker = int.Parse(text2 [section][1]);
				StartCoroutine(Type(text2[section][0]));
			}

			//}
		} else {
			currentlyTyping = false;
			outp = text2[section][0];

		}
		} else {
			section++;
		}
		//Debug.Log(text2 [section][0]);

		if(!dialog){
			//audsrc.clip = lines.GetCell(scene,section);
		}

		if(section >= text2.Count){
			if(!dialog){
			scene++;
				if(SceneManager.GetActiveScene().name.Equals ("StoryScene")){
			if(scene > 8){
				SceneManager.LoadSceneAsync("KevinScene");
				scene--;
				section = text2.Count - 1;
			}
				} else if(SceneManager.GetActiveScene().name.Equals ("StoryScene2")){
					if(scene > 3){
					SceneManager.LoadSceneAsync("ToBeContinued");
					scene--;
					section = text2.Count - 1;
					}
				}
			} else {
				//this.enabled = false;
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
			GetTextSetter(SceneManager.GetActiveScene().name, scene, 0, p);
			} else {
				BattleStateManager.me.SetState(0);
				BattleStateManager.wait = false;
				done = true;
				section = 0;
				for(int i = 0; i < outputs.Length; i++){
					outputs[i].SetActive(false);
				}
				for(int i = 0; i < selectables.Length; i++){
					selectables[i].canSelect = true;
				}
			}
			gameObject.SetActive(true);
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

	public void GetTextSetter(string name, int in1, int in2, int in3){
		if(name.Equals ("StoryScene")){
			SetText(in1, in2);
		} else if(name.Equals ("OilmancerScene")){
			SetText2(in1, in2);
		} else if(name.Equals ("KevinScene")){
			SetText3(in1, in2);
		} else if(name.Equals ("SpiderScene")){
			SetText4(in1, in2);
		} else if(name.Equals ("CEOScene")){
			SetText5(in1, in2, in3);
		} else if(name.Equals ("StoryScene2")){
			SetText6(in1, in2);
		}
	}
	//CUTSCENE EXPLANATION!
	//Typing "0" on it's own into the text will move the camera into the default position and skip ahead to the next thing
	//Typing "1" will look at the player
	//Typing "2" will look at the enemy
	//Make sure to set the speaker of the cutscene to whoever is the speaker if the camera movement is at the very beginning of the cutscene
	public void SetText6(int day, int ID){
		if (day == 0) {
			text2.Add (new string[]{"Geaux: Oh wow my arm!", "0", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
			text2.Add (new string[]{"Geaux: I'm kinda surprised they didnt secure it.", "1", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
		} else if (day == 1) {
			text2.Add (new string[]{"!!", "1", "0"});
		} else if (day == 2) {
			text2.Add (new string[]{"Geaux: uh oh...", "0", "0"});
			text2.Add (new string[]{"Mr. Haus: Damn.", "0", "0"});
			text2.Add (new string[]{"Mr. Haus: I needed that you know!", "1", "0"});
			//text2.Add (new string[]{"Geaux: (Huh. That chandeleir looks deliberately placed there. Odd...)", "0", "1"});
		} else if (day == 3) {
		//	text2.Add (new string[]{"Mr. Haus: Whatever, I've been living without it for 20 years...", "1", "0"});
			text2.Add (new string[]{"Geaux: Wait a second...", "0", "0"});
			text2.Add (new string[]{"Geaux: That's not your soul, is it?", "0", "0"});
			text2.Add (new string[]{"Mr. Haus: It's a Fake!", "1", "0"});
			text2.Add (new string[]{"Mr. Haus: I still need to find my soul!", "1", "0"});
			text2.Add (new string[]{"Geaux: Well whoever has it probably wants my money too", "1", "0"});
			text2.Add (new string[]{"Mr. Haus: I will find them, and I will kill them.", "1", "0"});
			text2.Add (new string[]{"Geaux: Oui", "1", "0"});
		}
	}

	public void SetText5(int day, int ID, int mid){
		if (day == 0) {
			if(mid == 0){
				text2.Add (new string[]{"1", "0", "0"});
			text2.Add (new string[]{"Hey buddy, give me my arm back!!", "0", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
				text2.Add (new string[]{"2", "0", "0"});
			text2.Add (new string[]{"No.", "1", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
			text2.Add (new string[]{"I'm missing like half of my body, but with your arm with your iron bones, I'll be more powerful than ever!!", "1", "0"});
			text2.Add (new string[]{"HEHEHEHAW", "1", "0"});
			text2.Add (new string[]{"Also, you were in a reduculous amount of debt, I needed something from you at some point.", "1", "0"});
				text2.Add (new string[]{"1", "0", "0"});
			text2.Add (new string[]{"That's fair.", "0", "0"});
			text2.Add (new string[]{"I still want my arm back though.", "0", "0"});
				text2.Add (new string[]{"2", "0", "0"});
			text2.Add (new string[]{"No!", "1", "0"});
				text2.Add (new string[]{"1", "0", "0"});
			text2.Add (new string[]{"By the way, what's Haus's problem with you?", "0", "0"});
				text2.Add (new string[]{"2", "0", "0"});
			text2.Add (new string[]{"We stole his soul.", "1", "0"});
				text2.Add (new string[]{"1", "0", "0"});
			text2.Add (new string[]{"Yeah, I'd be mad too.", "0", "0"});
			text2.Add (new string[]{"Anyway I'll need that arm back and also Haus's soul.", "0", "0"});
				text2.Add (new string[]{"2", "0", "0"});
			text2.Add (new string[]{"You'll need to kill me for it!", "1", "0"});
				text2.Add (new string[]{"1", "0", "0"});
			text2.Add (new string[]{"Okay, I will", "0", "0"});
				text2.Add (new string[]{"2", "0", "0"});
			text2.Add (new string[]{"Wait-", "1", "0"});
			//text2.Add (new string[]{"Geaux: (Huh. That chandeleir looks deliberately placed there. Odd...)", "0", "1"});
			} else if(mid == 1){
				//text2.Clear();
				//Debug.Log("ahusdghoduyagsfydgakyugsdf");
				text2.Add (new string[]{"1", "0", "0"});
				text2.Add (new string[]{"This is way easier than I thought!", "0", "0"});
				//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
				text2.Add (new string[]{"2", "0", "0"});
				text2.Add (new string[]{"YOU'RE WAY STRONGER THAN I THOUGHT YOU WERE", "1", "0"});
				//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
				text2.Add (new string[]{"I should've expected this from what you did to Oilmancer", "1", "0"});
				text2.Add (new string[]{"1", "0", "0"});
				text2.Add (new string[]{"You're cooked bruv", "0", "0"});
				text2.Add (new string[]{"2", "0", "0"});
				text2.Add (new string[]{"YOU WILL PAY FOR THIS!", "1", "0"});
				//text2.Add (new string[]{"Geaux: (Huh. That chandeleir looks deliberately placed there. Odd...)", "0", "1"});
			}else if(mid == 2){
				text2.Add (new string[]{"2", "1", "0"});
				text2.Add (new string[]{"I have been defeated >_<", "1", "0"});
				//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
				text2.Add (new string[]{"What even are you??? Are all of your bones made of iron??", "1", "0"});
				//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
				text2.Add (new string[]{"1", "0", "0"});
				text2.Add (new string[]{"I dunno, I'll have to ask my doctor.", "0", "0"});
				text2.Add (new string[]{"Where's my arm?", "0", "0"});
				text2.Add (new string[]{"2", "0", "0"});
				text2.Add (new string[]{"It's in the back in the box next to haus' soul", "1", "0"});
				//text2.Add (new string[]{"Geaux: (Huh. That chandeleir looks deliberately placed there. Odd...)", "0", "1"});
			}
		}
	}
	public void SetText4(int day, int ID){
		if (day == 0) {
			text2.Add (new string[]{"Woa", "0", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
			text2.Add (new string[]{"T H R E A T   D E T E C T E D", "1", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
			text2.Add (new string[]{"S U R R E N D E R   I M M E D I A T E L Y", "1", "0"});
			text2.Add (new string[]{"uh oh", "0", "0"});
			text2.Add (new string[]{"I've lived in 'Murica long enough to know to NEVER SURRENDER!!!", "0", "0"});
			text2.Add (new string[]{"Y O U   W I L L   B E   O B L I T E R A T E D", "1", "0"});
			//text2.Add (new string[]{"Geaux: (Huh. That chandeleir looks deliberately placed there. Odd...)", "0", "1"});
		}
	}
	public void SetText2(int day, int ID){
		if (day == 0) {
			text2.Add (new string[]{"Hallo", "0", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
			text2.Add (new string[]{"It's you.", "1", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
			text2.Add (new string[]{"Look man, Whatever you're here for, it isnt worth it.", "1", "0"});
			text2.Add (new string[]{"You'd never make it to the CEO, anyway.", "1", "0"});
			text2.Add (new string[]{"nuh uh", "0", "1"});
			text2.Add (new string[]{"(Huh. That chandeleir looks deliberately placed there. Odd...)", "0", "1"});
		}
	}
	public void SetText3(int day, int ID){
		if (day == 0) {
			text2.Add (new string[]{"Hello... kevin", "0", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Blud is NOT allowed in MY area!", "1", "0"});
			text2.Add (new string[]{"Good afternoon, what can I help you with?", "1", "0"});
			//text2.Add (new string[]{"Mr. Oil Mancer: Get out of mein haus!", "1", "0"});
			text2.Add (new string[]{"Do you know where I can find CEO man? I need to kill him.", "0", "0"});
			text2.Add (new string[]{"Sorry, I cant help you with that.", "1", "0"});
			text2.Add (new string[]{"Unfortunately, I need to kill you now.", "1", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): Hey, I'm calling you right now to make sure you know what you're doing.", "3", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): You can click the enemy in order to open up your attack menu!", "3", "1"});
			//text2.Add (new string[]{"Mr. Haus (over phone): Try clicking one of the buttons.", "3", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): You can click to stop the pointer when doing an action (green is the best, red is okay, and neither is the worst)", "3", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): The enemy will attack you after your turn is done.", "3", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): You can also click yourself to open up a different menu.", "3", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): You can figure it out from there.", "3", "1"});
			text2.Add (new string[]{"Mr. Haus (over phone): Good luck!", "3", "1"});
		}
	}
	public void SetText(int day, int ID){
		if (day == 0) {
			//text2.Add (new string[]{"House: Bad news...", "1", "0"});
			text2.Add (new string[]{"Mr. Haus: You have Hemochromatosis.", "1", "0"});
			text2.Add (new string[]{"Geaux: Gosh darnoux!", "1", "0"});
			//text2.Add (new string[]{"Geaux: What can I do about it?", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Since your arms and all of your bones have already solidified into iron...", "1", "0"});
			text2.Add (new string[]{"Mr. Haus: It's untreatable.", "1", "0"});
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
			text2.Add (new string[]{"Mr. Haus: You should be careful, I think the oilers might chop your arm off.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Oh wait, I'm a little late with that.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Anyway", "0", "1"});
		} else if (day == 6) {
			text2.Add (new string[]{"Mr. Haus: I have this time travelling watch for you...", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Dont question where I got it.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Uhhh, I need you to go destroy the oilers.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: They- hello? are you even alive?", "0", "1"});
		} else if (day == 7) {
			text2.Add (new string[]{"Geaux: Huh?", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: I was saying that the oilers are in texas!", "0", "1"});
			text2.Add (new string[]{"Geaux: Why exactly do you want me of all people to destroy them??", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Well they took your arm, and your bones are made of iron.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: I'm sure you can probably not die that much!", "0", "1"});
			text2.Add (new string[]{"Geaux: That isnt very reassuring...", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Also, one last thing!", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: When you travel back in time, everything will happen (usually) exactly as it happened before you did.", "0", "1"});
			//text2.Add (new string[]{"House: The timeline will progress as if you didnt", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: So if you remember what happened, you can try to avoid it!", "0", "1"});
			//text2.Add (new string[]{"House: You can avoid it.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Also remember that the butterfly effect exists, so things might change depending on what you do.", "0", "1"});
			text2.Add (new string[]{"Mr. Haus: Anyway, off you go!", "0", "1"});
		} else if (day == 8) {
			text2.Add (new string[]{"*After not very long...*", "0", "1"});
			text2.Add (new string[]{"Flight Attendant: Your flight to The middle of nowhere, texas, will be landing in 1 hour!", "0", "1"});
			text2.Add (new string[]{"Geaux: Wow this place was suprisingly easy to get to!", "0", "1"});
		}
	}
}
