using UnityEngine;
using System.Collections;

public class enemyBlock : MonoBehaviour {
	
	System.Random rnd = new System.Random ();
	
	public enum Type
	{
		Red,
		Blue,
		Green,
		Yellow,
		None
	};
	
	public Type currentType = Type.None;
	
	public int health = 1000;
	
	private int originalHealth = 1;
	
	public bool isDead = false;
	
	//This is where we started.
	public static Vector3 ourLocation = new Vector3(0,0,0);
	public Vector3 lerpToThis = new Vector3(0,0,0);
	
	//How fast the lerp will be
	public float timerIncrement = .2f;
	
	//What we use to keep track of where we are in our lerp.
	public float timer = 0;
	
	public bool shouldLerp = false;
	
	public void setOurLocation(Vector3 nuLoc){
		ourLocation = nuLoc;
		transform.position = ourLocation;
	}
	
	public Vector3 getOurLocation(){
		return ourLocation;
	}
	
	public void setLerpLocation(Vector3 nuLoc){
		lerpToThis = nuLoc;
	}
	
	public Vector3 getLerpLocation(){
		return lerpToThis;
	}
	
	public enum State
	{
		
		Still,
		Moving
		
	}
	
	State currentState = State.Still;
	public GameObject redSphere;
	public GameObject blueSphere;
	public GameObject greenSphere;
	public GameObject yellowSphere;
	
	
	//Increments our personal timer and tells this enemyBlock whether or not it should be moving.
	void incrementTimer(){
		if(timer == 0){
			if(gameObject.transform.position != lerpToThis){
				shouldLerp = true;
			}
			
			if(shouldLerp == true){
				currentState = State.Moving;
			}
			if(shouldLerp == false){
				currentState = State.Still;
			}
			
		}
		
		lerpCheck();
		
		timer+= timerIncrement;
		print(""+timer);
		if(timer>=1){
			gameObject.transform.position = lerpToThis;
			shouldLerp = false;
			currentState = State.Still;
			//timer = 0;
		}
		
		
		
		
	}
	
	void lerpCheck(){
		if(currentState == State.Moving){
			transform.position = Vector3.Lerp(ourLocation, lerpToThis, timer);
		}
	}
	
	public enemyBlock(){
		//disableColors();
		originalHealth = health;
		randomizeEnemy();
	}
	
	public enemyBlock(Type typeSet){
		//disableColors();
		originalHealth = health;
		setType(typeSet);
		randomizeHealth();
	}
	
	public enemyBlock(int healthSet){
		//disableColors();
		health = healthSet;
		originalHealth = health;
		randomizeType();
	}
	
	public enemyBlock(Type typeSet, int healthSet){
		//disableColors();
		health = healthSet;
		originalHealth = health;
		setType(typeSet);
	}
	
	//Subtracts damage from health. If the health is less than 0, it is set to 0 and the flag for death is triggered.
	public void takeDamage(int damage){
		health-= damage;
		
		if(health < 0){
			health = 0;
			isDead = true;
		}
	}
	
	//Uses a simple algorithm to create a new health total within 1.5 of our original health in either direction.
	public void randomizeHealth(){
		int divisor = rnd.Next(2,5);//We will either be dividing our total by 2 or 3 or 4.
		
		int healthChunk = originalHealth/divisor; //We will be adding this number to our original health.
		
		//We will either be adding or subtracting
		int neg = rnd.Next (0,2);
		if(neg == 1){
			neg = -1;
		}
		else{
			neg = 1;
		}
		
		//Lastly, we modify our health by this number so that we get semi-randomized health chunks!
		health = originalHealth + (healthChunk * neg); 
	}
	
	//Selects a type for our enemyBlock at random. Works even when our type has not been declared yet.
	public void randomizeType(){
		
		
		int selection = rnd.Next (0,4);
		if(selection == 0){
			currentType = Type.Red;
		}
		else if(selection == 1){
			currentType = Type.Blue;
		}
		else if(selection == 2){
			currentType = Type.Green;
		}
		else if(selection == 3){
			currentType = Type.Yellow;
		}
		else{
			currentType = Type.Red;
		}
		
	}
	
	public void randomizeEnemy(){
		randomizeType();
		randomizeHealth();
	}
	
	
	//Disables the renderer for ALL of the colors.
	/*
	public void disableColors ()
	{
		currentType = Type.None;

		redSphere.renderer.enabled = false;
		blueSphere.renderer.enabled = false;
		greenSphere.renderer.enabled = false;
		yellowSphere.renderer.enabled = false;
		
		
	}*/
	
	public void disableColor (int chosenColor)
	{
		
		if (chosenColor == 0) {
			redSphere.GetComponent<Renderer>().enabled = false;
		}
		if (chosenColor == 1) {
			blueSphere.GetComponent<Renderer>().enabled = false;
		}
		if (chosenColor == 2) {
			greenSphere.GetComponent<Renderer>().enabled = false;
		}
		if (chosenColor == 3) {
			yellowSphere.GetComponent<Renderer>().enabled = false;
		}
		
	}
	
	
	public void enableColor (int chosenColor)
	{
		
		if (chosenColor == 0) {
			redSphere.GetComponent<Renderer>().enabled = true;
		}
		if (chosenColor == 1) {
			blueSphere.GetComponent<Renderer>().enabled = true;
		}
		if (chosenColor == 2) {
			greenSphere.GetComponent<Renderer>().enabled = true;
		}
		if (chosenColor == 3) {
			yellowSphere.GetComponent<Renderer>().enabled = true;
		}
		
	}
	
	//Getters&Setters
	
	//Setter for health
	void setHealth(int value){
		health = value;
	}
	
	//Returns current health
	int getHealth(){
		return health;
	}
	
	//Returns whether or not our enemyBlock is dead or not.
	bool getDead(){
		return isDead;
	}
	
	//Setter for whether or not we are dead.
	void setDead(bool deathState){
		isDead = deathState;
	}
	
	//Getter for type. Returns an ENUM, not an int.
	public int getType(){
		if(currentType ==Type.Red){
			return 0;
		}
		else if(currentType ==Type.Blue){
			return 1;
		}
		else if(currentType ==Type.Green){
			return 2;
		}
		else if(currentType ==Type.Yellow){
			return 3;
		}
		
		return 0;
		
		
	}
	

	//Setter for Type. Give it an Enum.
	void setType(Type type){
		currentType = type;
		
	}
	
	// Use this for initialization
	void Start () {
		ourLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		incrementTimer();
	}
}
