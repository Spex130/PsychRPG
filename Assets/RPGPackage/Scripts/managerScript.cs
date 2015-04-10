using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class managerScript : MonoBehaviour {

	public GameObject spawnPoint;
	public Canvas gameCanvas;
	//public GameObject mainMenuPoint; 




	//These are the overarching gamestates
	public enum state{menuScreen, optionScreen, combatScreen, profileScreen, itemScreen};
	public state gameState = state.menuScreen;

	//player variables
	public characterScript player;

	//Main menu variables

	//This specificall handles our main menu state
	public enum stateMenu{inactive, activate, active, deactivate};
	//All the gameObject that holds our main Menu
	public GameObject mainMenu;
	public stateMenu menuState = stateMenu.activate;
	public bool isMainMenuActive = true;

	//This specificall handles our profile menu state
	public enum stateProfile{inactive, activate, active, deactivate};
	//All the gameObject that holds our main Menu
	public GameObject profileMenu;
	public stateProfile profileState = stateProfile.inactive;
	public bool isProfileActive = false;

	//Combat Menu variablws
	public combatManager combatMenu;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(gameState){
		
		case state.menuScreen:
				menuStateUpdate();
			break;
		
		case state.optionScreen:
			break;
		
		case state.combatScreen:
			if(!combatMenu.isActive ()){
				//combatMenu = (combatManager)GameObject.Instantiate(combatNode,spawnPoint.transform.position, spawnPoint.transform.rotation);
				combatMenu.transform.parent = spawnPoint.transform;
				combatMenu.transform.position = new Vector3(0,0,0);
				combatMenu.transform.localPosition = new Vector3(0,0,0);
				combatMenu.transform.localScale = new Vector3(1,1,1);
				combatMenu.activate();
			}
			else if(combatMenu.shouldDeactivate()){
				combatMenu.transform.localPosition = new Vector3(300,0,0);
				combatMenu.setToInactive();
				gameState = state.menuScreen;
				menuState = stateMenu.activate;
			}
			else{
				combatMenu.menuLoop();
			}
			break;
		
		case state.profileScreen:
			profileStateUpdate();
			break;
		
		case state.itemScreen:
			break;
		}

	}

	public void profileStateUpdate(){
		switch(profileState){
		case stateProfile.activate:
				profileMenu.transform.parent = spawnPoint.transform;
				profileMenu.transform.position = new Vector3(0,0,0);
				profileMenu.transform.localPosition = new Vector3(0,0,0);
				profileMenu.transform.localScale = new Vector3(1,1,1);
				
			profileState = stateProfile.active;
				
				break;
				
		case stateProfile.deactivate:
				
				break;
		}
	}

	public void menuStateUpdate(){
		switch(menuState){
		case stateMenu.activate:
			player.curHealth = player.maxHealth;
				//mainMenu = (GameObject)GameObject.Instantiate(mainMenuPoint,spawnPoint.transform.position, spawnPoint.transform.rotation);
				mainMenu.transform.parent = spawnPoint.transform;
				mainMenu.transform.position = new Vector3(0,0,0);
				mainMenu.transform.localPosition = new Vector3(0,0,0);
				mainMenu.transform.localScale = new Vector3(1,1,1);
				//print(mainMenu.transform.position);
				//mainMenu.transform.localScale = (
				menuState = stateMenu.active;

		break;
		
		case stateMenu.deactivate:
				
		break;
		}
	}



	//Swapping functions

	public void switchToCombat(){
		mainMenu.transform.localPosition = new Vector3(-500,0,0);
		print("MOVE TO -500");
		menuState = stateMenu.inactive;
		gameState = state.combatScreen;
	}

	public void switchToProfile(){
		mainMenu.transform.localPosition = new Vector3(-500,0,0);
		print("MOVE TO -500");
		menuState = stateMenu.inactive;
		gameState = state.profileScreen;
		profileState = stateProfile.activate;
	}

	public void switchProfileToMenu(){
		profileState = stateProfile.deactivate;
		profileMenu.transform.localPosition = new Vector3(-500,0,0);
		gameState = state.menuScreen;
		menuState = stateMenu.activate;
	}


}
