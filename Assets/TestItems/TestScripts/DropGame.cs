using UnityEngine;
using System.Collections;

/*
 * DropGame is the class that handles all of the interactions with the arrows that drop into the playing field. 
*/
public class DropGame : MonoBehaviour {
	
	public dropBlock[] dropBlockArray;
	Random random = new Random();
	public GameObject dropSpot;
	public dropBlock DropBlock;
	public int arraySize = 5;


	// Use this for initialization
	void Start () {
		dropBlockArray = new dropBlock[arraySize];

		initBlocksRandomly();
	}


	//Fills the starting block array full to the brim with blocks of randomly assignd directions.
	void initBlocksRandomly(){

		/*
	//GameObject assigned to Hex public variable is cloned
				GridNode hex = (GridNode)Instantiate(Hex);
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				hex.transform.position = calcWorldCoord(gridPos);
				//hex.transform.parent = hexGridGO.transform;
				hexArray[(int)x, (int)y] = hex as GridNode;
		 * */
		for(int i = 0; i < dropBlockArray.Length; i++){
			if(i==0){
				//dropBlock(Random.Range(0,dropBlockArray.Length-1), true)
				DropBlock.randomizeBlock(true);
				dropBlock block = (dropBlock)Instantiate(DropBlock);
				block.transform.position = new Vector3(dropSpot.transform.position.x, dropSpot.transform.position.y + i, dropSpot.transform.position.z);
				block.transform.Rotate(Vector3.right, 90 * DropBlock.getCurrentDirection ());
				dropBlockArray[i] = block as dropBlock;
			}
			
			else{
				DropBlock.randomizeBlock(false);
				dropBlock block = (dropBlock)Instantiate(DropBlock);
				block.transform.position = new Vector3(dropSpot.transform.position.x, dropSpot.transform.position.y + i, dropSpot.transform.position.z);
				block.transform.Rotate(Vector3.right, 90 * DropBlock.getCurrentDirection ());
				dropBlockArray[i] = block as dropBlock;
			}
		}
	}

	//Arranges the blocks in the DropBlockArray for you, on command.
	void arrangeBlocks(){
		for(int i = 0; i < dropBlockArray.Length; i++){

			if(!(dropBlockArray[i] != null)){
				//If spoit is null, do nothing
			}
			else{//If spot is a dropBlock, place the dropBlock
				dropBlockArray[i].transform.position = dropSpot.transform.position;
				dropBlockArray[i].transform.position = new Vector3(dropBlockArray[i].transform.position.x, dropSpot.transform.position.y + i, dropBlockArray[i].transform.position.z);


				//dropBlockArray[i].transform.Rotate(Vector3.right, 90 * DropBlock.getCurrentDirection ());
			}
		}
	}

	//Removes a single block from the array and moves all of the blocks above it down.
	void removeBlock(int index){
		if(index >= dropBlockArray.Length){
			// Don't do anything if the index given is bigger than the amount of spaces in the array.
		}
		else{//Otherwise
			if((dropBlockArray[index] != null)){
				GameObject.Destroy(dropBlockArray[index].gameObject);
			}
			dropBlockArray[index] = null;



			//We set each block to the block above it, then set the topmost block to null.
			for(int i = index; i < dropBlockArray.Length-1; i++){

				dropBlockArray[i] = dropBlockArray[i+1];
				dropBlockArray[i+1] = null;

				/*
				if(i == dropBlockArray.Length - 1){
					if(dropBlockArray[i] != null){
						GameObject.Destroy(dropBlockArray[i].gameObject);
						//dropBlockArray[i] = null;
						print("ARRAY: "+i+" DESTROYED");
					}

				}
				else{

					dropBlockArray[i] = dropBlockArray[i+1];
					/*
					dropBlockArray[i].setDirection(dropBlockArray[i+1].getCurrentDirection());
					dropBlockArray[i].transform.rotation = dropBlockArray[i+1].transform.rotation;
					if(i == 0){
						dropBlockArray[i].setSelected(true);
					}
					else{
						dropBlockArray[i].setSelected(false);
					}


				}
				*/
			}
		}

	}

	//Checks whether or not the input given to it matches the direction of the block at the bottom of the stack.
	bool isInputCorrect(int input){
		if(dropBlockArray[0] != null){
			return (input == dropBlockArray[0].getCurrentDirection());
		}
		return true;//Returns true if the block does not exist, as to not impede progress.
	}

	//Checks whether or not the arrow input was correct or not and removes the bottom block if correct.
	void buttonCheck(){
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;

		if(Input.GetKeyDown (KeyCode.A)){
			for(int i = 0; i < dropBlockArray.Length; i++){
				if(dropBlockArray[i] != null){
					print("ARRAY "+i+" DIRECTION: "+dropBlockArray[i].getCurrentDirection());
				}
				else{
					print("ARRAY "+i+"IS NULL");
				}
			
			}
		}

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			up = true;
			print("UP PRESSED");

		}
		else if(Input.GetKeyDown(KeyCode.DownArrow)){
			down = true;
			print("DOWN PRESSED");
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			left = true;
			print("LEFT PRESSED");
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow)){
			right = true;
			print("RIGHT PRESSED");
		}
		
		
		if(up || down || left || right){
			if(up){
				if(isInputCorrect(0) == true){
					removeBlock(0);
				}
			}
			if(down){
				if(isInputCorrect(2) == true){
					removeBlock(0);
				}
			}
			if(left){
				if(isInputCorrect(3) == true){
					removeBlock(0);
				}
			}
			if(right){
				if(isInputCorrect(1) == true){
					removeBlock(0);
				}
			}

		}
	}

	// Update is called once per frame
	void Update () {
		buttonCheck();
		arrangeBlocks();

	}
}
