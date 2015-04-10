using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyManager : MonoBehaviour {

	System.Random rnd = new System.Random ();

	/*TODO Complete this class
	 * Meant to contain all enemies the user fights all at once on the screen.
	 * Uses variable Size Array List that holds EnemyBlocks so you can apply anything to them all at once.
	 * Automatically handles placement of EnemyBlocks, using itself as the center and placing them on both sides.
	 * Compresses as its inhabitants die
	 * Update runs two checks constantly: Lerping our EnemyBlocks to their spots and wither or not an enemyBlock should be removed (is dead).
	 * There is also a method to give damage to the entire arrayList that takes in an array and then doles out damage to the appropriate colors. 
	 * Game should end when all enemies inside the arraylist are "dead"
	*/

	List<enemyBlock> enemyList = new List<enemyBlock>();
	public float incrementSize = 1;

	public enemyBlock EnemyBlock = new enemyBlock();

	public enemyBlock[] enemyArray;

	bool gameIsFinished = false;

	void randomizeEnemyList(){
		int enmNum = rnd.Next (1, 4);
		for(int i = 0; i < enmNum; i++){
			enemyList.Add(new enemyBlock());
		}
	}

	void initBlocksRandomly ()
	{
		
		//Fill the List at random.
		int enmNum = rnd.Next (1, 5);
		for(int j = 0; j < enmNum; j++){

				enemyBlock block = (enemyBlock)Instantiate (EnemyBlock);
				block.randomizeEnemy();
				block.transform.position = new Vector3(-.5f, 10f, 0f);

				block.setOurLocation(block.transform.position);
				block.setLerpLocation(getArrayLocationVector3 (j));

				
				
				
				
				enemyList.Add(block);
				
		}
		convertToEnemyArray();
	}

	//Takes in an X and Y position, spits out a Vector3 containing the gameSpace location of that point.
	Vector3 getArrayLocationVector3 (int xLoc)
	{
		Vector3 temp = new Vector3((gameObject.transform.position.x + xLoc - (float) enemyArray.Length/ 2.0f), gameObject.transform.position.y, transform.position.z);
		return temp;
	}

	void arrangeBlocks(){
		for(int i = 0; i < enemyArray.Length; i++){
			enemyArray[i].transform.position = getArrayLocationVector3(i);
		}
	}

	void convertToEnemyArray(){
		enemyArray = enemyList.ToArray();

	}

	public void applyDamage(int damage){
		enemyList = new List<enemyBlock>();

		for(int i = 0; i < enemyArray.Length; i++){
			enemyArray[i].takeDamage(damage);
			enemyList.Add(enemyArray[i]);
		}
	}

	public void applyDamage(int[] damageArray){
		enemyList = new List<enemyBlock>();
		
		for(int i = 0; i < enemyArray.Length; i++){
			enemyBlock myblock = enemyArray[i];
			int type = myblock.getType();
			//RED->BLUE->GREEN->YELLOW
			enemyArray[i].takeDamage(damageArray[type]);
			enemyList.Add(enemyArray[i]);
		}

		deathCheck();
		gameCheck();
	}

	public void gameCheck(){
		if(enemyArray.Length == 0){
			gameIsFinished = true;
		}
	}

	//Removes dead blocks. Somewhat expensive.
	public void deathCheck(){
		enemyList = new List<enemyBlock>();
		for(int i = 0; i < enemyArray.Length; i++){
			
			if(enemyArray[i].isDead == false){
				enemyList.Add(enemyArray[i]);
			}
			else{
				GameObject.Destroy(enemyArray[i].gameObject);
			}
		}
		convertToEnemyArray();
	}

	// Use this for initialization
	void Start (){
		initBlocksRandomly();
	}
	
	// Update is called once per frame
	void Update () {
		arrangeBlocks();
	}
}
