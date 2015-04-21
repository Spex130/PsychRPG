using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class combatManager : MonoBehaviour {

	//How long our game has been running.
	public int turnCount = 0;

	//For our combat state machine.
	public enum combatState{activate, deactivate, inactive, fightStart, playerTurn, enemyTurn, win, lose};
	public combatState fightState = combatState.inactive;


	//Our player
	public characterScript player;

	//The enemies that we have a random chance of instantiating at fight start
	public enemyClass[] enemyPool = new enemyClass[1];
	enemyClass opponent;
	public GameObject enemySpawn;

	//Whether or not we should use the package of enemies included with the constructor.
	public bool useEnemyPackage = false;

	//StatusBar stuff
	//public int statusBarLength = 3;
	//The things that the status bar will read out!
	public string[] statusBar = new string[3];
	//Show
	public Text line1;
	public Text line2;
	public Text line3;

	//Healthbar handling
	public Image playerHealthImg;
	public Text playerHealthText;
	
	public Image enemyHealthImg;
	public Text enemyHealthText;


	//Endstate buttons and points
	public GameObject endPoint;
	public GameObject winPoint;
	public GameObject losePoint;

	//Constructor
	public combatManager(enemyClass[] enemyPackage){
		enemyPool = enemyPackage;
		useEnemyPackage = true;
	}


	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag("Player");
		populateStatusBar();

		if(useEnemyPackage == false){
			//enemyPool[0] = new enemyClass();
			//enemyPool[0] = (enemyClass)GameObject.Instantiate(enemyPool[0], enemySpawn.transform.position, enemySpawn.transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void populateStatusBar(){
		for(int i = 0; i < statusBar.Length; i++){
			statusBar[i] = "";
		}
	}

	public void updateStatusBar(){
		line1.text = statusBar[0];
		line2.text = statusBar[1];
		line3.text = statusBar[2];
	}

	public void statusBarSay(string line){

		for(int i = statusBar.Length - 1; i > 0; i--){
			statusBar[i] = statusBar[i-1];
		}
		statusBar[0] = line;
	}

	//Viewing of healthbar stuff
	
	public void healthbarUpdate(){
		playerHealthImg.fillAmount = (float)player.curHealth/(float)player.maxHealth;
		playerHealthText.text = player.curHealth + "/" + player.maxHealth;

		if(opponent.isDead()){
			enemyHealthImg.fillAmount = 0;
			enemyHealthText.text = "Dead";
		}
		else{
			enemyHealthImg.fillAmount = (float)opponent.hp/(float)opponent.hpMax;
			enemyHealthText.text = opponent.hp+"/"+opponent.hpMax;
		}
	}

	public bool isActive(){
		return fightState != combatState.inactive;
	}

	public bool shouldDeactivate(){
		return fightState == combatState.deactivate;
	}

	public void activate(){
		fightState = combatState.fightStart;
	}

	public void runAway(){
		if(fightState == combatState.playerTurn){
			fightState = combatState.deactivate;
			if(opponent != null){
				GameObject.Destroy(opponent.gameObject);
			}
		}
	}

	public void enemyAttack(){
		int giveDamage = opponent.attack ();
		player.takeDamage (giveDamage);
		statusBarSay("You take "+giveDamage+" damage!");
		if(player.isDead()){
			losePoint.transform.localPosition = new Vector3(0,0,5000);
			fightState = combatState.lose;
		}
	}

	public void setToInactive(){
		fightState = combatState.inactive;
	}

	public void playerAttack(){
		if(fightState == combatState.playerTurn){
			int giveDamage = player.attack();
			opponent.takeDamage (giveDamage);
			statusBarSay("You attack for "+giveDamage+" damage!");
			turnCount++;
			if(opponent.isDead()){
				GameObject.Destroy(opponent.gameObject);
				fightState = combatState.win;
				statusBarSay("You win!");
			}
			else if(player.isDead()){
				losePoint.transform.localPosition = new Vector3(0,0,5000);
				fightState = combatState.lose;
			}
			else{
				fightState = combatState.enemyTurn;
			}
		}
	}

	public void win(){
		fightState = combatState.deactivate;
	}

	public void lose(){
		fightState = combatState.deactivate;
	}

	public void menuLoop(){
		switch(fightState){
		case combatState.fightStart:
			//Fill the Status bar with empty stuff
			int num = Random.Range(0,4);
			populateStatusBar();

			winPoint.transform.localPosition = new Vector3(0,5000,0);
			losePoint.transform.localPosition = new Vector3(-30,5000,0);


			opponent = (enemyClass)GameObject.Instantiate(enemyPool[num], enemySpawn.transform.position, enemySpawn.transform.rotation);
			opponent.transform.parent = enemySpawn.transform;
			opponent.transform.localPosition = new Vector3(0,0,0);
			statusBarSay("An enemy "+opponent.name+" appeared!");
			fightState = combatState.playerTurn;
			break;
		case combatState.playerTurn:
				
			break;
		case combatState.enemyTurn:
			enemyAttack();
			if(player.isDead()){
				GameObject.Destroy(opponent.gameObject);
				statusBarSay("You lose!");
				fightState = combatState.lose;
			}
			else{
				fightState = combatState.playerTurn;
			}
			break;
		case combatState.win:
			winPoint.transform.localPosition = new Vector3(0,0,0);
			break;
		case combatState.lose:

			losePoint.transform.localPosition = new Vector3(0,0,0);
			break;
		
		}
		updateStatusBar();
		healthbarUpdate();
	}
}
