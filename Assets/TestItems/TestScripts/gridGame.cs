using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
	* DropGame is the class that handles all of the interactions with the arrows that drop into the playing field. 
	*/
public class gridGame : MonoBehaviour
{
	
	public colorBlock[,] colorBlockArray;
	Random random = new Random ();
	public GameObject dropSpot;
	public colorBlock ColorBlock;
	public removalBlock RemovalSphere;

	public enemyManager eMan;

    public enum State
    {
        Normal,
        Dragging,
        Matching,
        GameOver
    };
    
    public int ySize = 5;
    public int xSize = 5;
    public GameObject playerCursor;
    public GameObject selectCursor;

    // Text representing time remaining in the level
    public GUIText timerText;

    // Text representing the current level score
    public GUIText scoreText;

    // Time remaining in the level
    [System.NonSerialized]
    public float timer;
    
    // Starting timer for the level
    public int LEVEL_TIMER;

    // Score accrued for each tile destroyed
    public int TILE_SCORE;


    // Current score for the level
    [System.NonSerialized]
    public int score;

	public bool turnIsFinished = false;


	public bool up = false;
	public bool down = false;
	public bool left = false;
	public bool right = false;


	public bool allowTouch = true;
	public int stepIndex = 0; //The step of the entire end-turn process we are at.
	public float transitionPercent = 0f;//How far in our lerp we are
	public int blockSet = 0;//Helps for less homogenized randomness generation.

	//TouchInput's Stuff
	
	public LayerMask touchInputMask;
	
	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;
	
	private RaycastHit hit;
	
	//public colorBlock selected;
	public colorBlock selected;
	public Camera camera;

	/// <summary>
	/// Used to check whether or not we are close enough to another colorBlock to switch with it.
	/// </summary>
	public float checkDistance = .5f;
	public float xDistCheck = 0f;
	public float yDistCheck = 0f;
	public Vector3 origColorBlockPoint = new Vector3();
	//End TouchInput
	
	
	//LinkedList Stuff
	public LinkedList<colorBlock> colorList = new LinkedList<colorBlock> ();
	
	public colorBlock getlistHead ()
	{
		return colorList.First.Value;
	}
	
	
	
	
	//End LinkedList Stuff

	
	//Selected Block stuff
	
	/*
	 * Sets whether or not a block is 'selected' (Also sets whether or not it should show its pink cursor.) 
	 * This is from the times when this program was still using a Keyboard and we didn't know it was supposed to be mobile.
	*/
	void setColorBlockSelection (int x, int y, bool selection)
	{
		if (selection) {
			colorBlockArray [x, y].setSelected (selection);
			colorBlockArray [x, y].enableCursor ();
		} else {
			colorBlockArray [x, y].setSelected (selection);
			colorBlockArray [x, y].disableCursor ();
		}
	}
	
	//Tells the block to refresh itself with all of its new options activated.
	void refreshColorBlock (int x, int y)
	{
		if(colorBlockArray[x,y] != null){
			colorBlockArray [x, y].refresh ();
		}
	}
	


    // Current state of the level
    [System.NonSerialized]
    public State currentState;

    [System.NonSerialized]
    public int cursorLocX = 0;

    [System.NonSerialized]
    public int cursorLocY = 0;
    bool hasBlockSelected = false; //Determines whether or not a block shoud be moving with the cursor.



    //Cursor Stuff
	// Legacy Keyboard stuff that isn't as relevant now.

    /*
     * Sets the cursor's location
	*/
    void setCursorLoc(int x, int y)
    {
        playerCursor.transform.position = getArrayLocationVector3(x, y);
    }

    //Moves the cursor up a square, if it can.
    void moveUpCheck(int x, int y)
    {
        if (colorBlockArray.GetLength(1) - 1 >= y + 1)
        {
            cursorLocY++;
        }
        setCursorLoc(cursorLocX, cursorLocY);
    }

    //Moves the cursor down a square, if it can
    void moveDownCheck(int x, int y)
    {
        if (y - 1 >= 0)
        {
            cursorLocY--;
        }
        setCursorLoc(cursorLocX, cursorLocY);
    }


    //Moves the cursor left a square, if it can.
    void moveRightCheck(int x, int y)
    {
        if (colorBlockArray.GetLength(0) - 1 >= x + 1)
        {
            cursorLocX++;
        }
        setCursorLoc(cursorLocX, cursorLocY);
    }



    void moveLeftCheck(int x, int y)
    {
        if (x - 1 >= 0)
        {
            cursorLocX--;
        }
        setCursorLoc(cursorLocX, cursorLocY);
    }



    //End Cursor Stuff

	/*
	 * Swaps
	 * These methods swap a sphere with another sphere.
	*/


	void swapUp(int x, int y){//We take the spot we are given and move it up one.
		if (colorBlockArray.GetLength (1) - 1 >= y + 1) {//If we are in our Array
			//Do a temp based swap
			colorBlock temp = colorBlockArray[x, y];
			colorBlockArray[x, y] = colorBlockArray[x, y+1];
			colorBlockArray[x, y+1] = temp;
			
			colorBlockArray[x, y].setVecPos(getArrayLocationVector3(x,y));
			colorBlockArray[x, y].setXY(x, y);
			colorBlockArray[x, y].transform.position =getArrayLocationVector3(x,y);
			
			
			colorBlockArray[x, y+1].setVecPos(getArrayLocationVector3(x,y+1));
			colorBlockArray[x, y+1].setXY(x, y+1);
			
			
			refreshColorBlock(x,y);
			refreshColorBlock(x,y+1);
			
		}
		up = false;
		
		
	}


	void swapDown (int x, int y)
	{
		if (y - 1 >= 0) {

			//Do a temp based swap
			colorBlock temp = colorBlockArray[x, y];
			colorBlockArray[x, y] = colorBlockArray[x, y-1];
			colorBlockArray[x, y-1] = temp;
			
			colorBlockArray[x, y].setVecPos(getArrayLocationVector3(x,y));
			colorBlockArray[x, y].setXY(x, y);
			colorBlockArray[x, y].transform.position =getArrayLocationVector3(x,y);
			
			colorBlockArray[x, y-1].setVecPos(getArrayLocationVector3(x,y-1));
			colorBlockArray[x, y-1].setXY(x, y-1);
			
			
			refreshColorBlock(x,y);
			refreshColorBlock(x,y-1);

		}
		down = false;
	}
	
	void swapRight (int x, int y)
	{
		if (colorBlockArray.GetLength (0) - 1 >= x + 1) {

			//Do a temp based swap
			colorBlock temp = colorBlockArray[x, y];
			colorBlockArray[x, y] = colorBlockArray[x+1, y];
			colorBlockArray[x+1, y] = temp;
			
			colorBlockArray[x, y].setVecPos(getArrayLocationVector3(x,y));
			colorBlockArray[x, y].setXY(x, y);
			colorBlockArray[x, y].transform.position =getArrayLocationVector3(x,y);
			
			colorBlockArray[x+1, y].setVecPos(getArrayLocationVector3(x+1,y));
			colorBlockArray[x+1, y].setXY(x+1, y);
			
			
			refreshColorBlock(x,y);
			refreshColorBlock(x+1,y);

		}
		right = false;
	}
	
	void swapLeft (int x, int y)
	{
		if (x - 1 >= 0) {

			//Do a temp based swap
			colorBlock temp = colorBlockArray[x, y];
			colorBlockArray[x, y] = colorBlockArray[x-1, y];
			colorBlockArray[x-1, y] = temp;
			
			colorBlockArray[x, y].setVecPos(getArrayLocationVector3(x,y));
			colorBlockArray[x, y].setXY(x, y);
			colorBlockArray[x, y].transform.position =getArrayLocationVector3(x,y);
			
			colorBlockArray[x-1, y].setVecPos(getArrayLocationVector3(x-1,y));
			colorBlockArray[x-1, y].setXY(x-1, y);
			
			
			refreshColorBlock(x,y);
			refreshColorBlock(x-1,y);
		}
		left = false;	
	}

	//End Selected Block Stuff
	
	
	//Fills the starting block array full to the brim with blocks of randomly assigned colors.
	void initBlocksRandomly ()
	{
		
		//Iterate through the 2-layer array here
		for (int j = 0; j < colorBlockArray.GetLength(0); j++) {
			
			for (int i = 0; i < colorBlockArray.GetLength (1); i++) {
				
				
				//dropBlock(Random.Range(0,dropBlockArray.Length-1), true)
				//ColorBlock.randomizeBlock ();
				colorBlock block = (colorBlock)Instantiate (ColorBlock);
				block.randomizeBlock ();
				block.transform.position = getArrayLocationVector3 (j, i);
				block.setVecPos(getArrayLocationVector3 (j, i));


				blockSetIncrement();
				block.setBlockColor(blockSet);
				blockSetIncrement();
				colorBlockArray [j, i] = block as colorBlock;
				colorBlockArray[j, i].setXY(j, i);
				colorBlockArray [j, i].setScoreCombo(false);
				
			}
		}
	}


    // Use this for initialization
    void Start()
    {
		//mat = renderer.material;//Freddy's Code Thing



        currentState = State.Normal;

        score = 0;
        timer = 64;
        UpdateGUI();

        playerCursor = (GameObject)Instantiate(playerCursor);
		eMan = (enemyManager)Instantiate (eMan);


        colorBlockArray = new colorBlock[xSize, ySize];
        initBlocksRandomly();

	}

	/*
	 * Used to help randomize the random block selection
	*/
	void blockSetIncrement(){
		if(blockSet == 3){
			blockSet = 0;
		}
		else{
			blockSet++;
		}
	}


	//Finds all of the empty blocks and fills them with a new randomized block
	void initEmptyBlocks(){
		//Iterate through the 2-layer array here
		for (int j = 0; j < colorBlockArray.GetLength(0); j++) {
			
			for (int i = 0; i < colorBlockArray.GetLength (1); i++) {
				
				if(!(colorBlockArray[j, i] != null)){
					//dropBlock(Random.Range(0,dropBlockArray.Length-1), true)
					//ColorBlock.randomizeBlock ();

					colorBlock block = (colorBlock)Instantiate (ColorBlock);
					colorBlockArray [j, i] = block as colorBlock;

					blockSetIncrement();
					colorBlockArray [j, i].setBlockColor(blockSet);
					blockSetIncrement();
					//colorBlockArray [j, i].randomizeBlock();
					colorBlockArray [j, i].transform.position = getArrayLocationVector3 (j, i);
					colorBlockArray [j, i].setVecPos (getArrayLocationVector3 (j, i));
					colorBlockArray [j, i].setXY(j,i);
					colorBlockArray [j, i].setScoreCombo(false);

					//RemovalBlocks are the graphical removing effect.
					removalBlock rBlock = (removalBlock)Instantiate (RemovalSphere);
					rBlock.transform.position = colorBlockArray [j, i].transform.position;
				}
			}
		}
		
	}
	
	//Takes in an X and Y position, spits out a Vector3 containing the gameSpace location of that point.
	Vector3 getArrayLocationVector3 (int xLoc, int yLoc)
	{
		Vector3 temp = new Vector3 (dropSpot.transform.position.x + xLoc - (float)(colorBlockArray.GetLength (0) / 2.0), dropSpot.transform.position.y + yLoc, dropSpot.transform.position.z);
		return temp;
	}
	
	//Arranges the blocks in the DropBlockArray for you, on command.
	void arrangeBlocks ()
	{
		for (int j = 0; j < colorBlockArray.GetLength(0); j++) {
			for (int i = 0; i < colorBlockArray.GetLength(1); i++) {
				
				if (!(colorBlockArray [j, i] != null)) {
					//If spot is null, do nothing
				} else {//If spot is a dropBlock, place the dropBlock
					colorBlockArray [j, i].transform.position = colorBlockArray [j, i].vecPos;
					
					//dropBlockArray[i].transform.Rotate(Vector3.right, 90 * DropBlock.getCurrentDirection ());
				}
			}
		}
	}
	
	
	
	//Removes a single block from the array and sets its space to null.
	void removeBlock (int xIndex, int yIndex)
	{
		if (xIndex >= colorBlockArray.GetLength (0) || yIndex >= colorBlockArray.GetLength (1)) {
			// Don't do anything if the index given is bigger than the amount of spaces in the array.
		} else {//Otherwise
			
			//Destroy what's in the location, and then set that space to null;
			if ((colorBlockArray [xIndex, yIndex] != null)) {
				removalBlock rBlock = (removalBlock)Instantiate (RemovalSphere);
				rBlock.transform.position = colorBlockArray [xIndex, yIndex].transform.position;
				GameObject.Destroy (colorBlockArray [xIndex, yIndex].gameObject);
			}
			colorBlockArray [xIndex, yIndex] = null;
			
			
		}
		
	}
	
	//We go to the location (which should be empty), then  starting here set each block to the block above it, then set the topmost block to null.
	//Meant to move all blocks down by 1
	//It does not change their physical location, however. That part is handled in step 2.
	bool dropRowOnce (int xIndex, int yIndex)
	{
		bool didDrop = false;
		for (int i = yIndex; i < colorBlockArray.GetLength(1)-1; i++) {
			if(!(colorBlockArray[xIndex, i] != null)){//If the spot we are at is null..
				if(colorBlockArray[xIndex, i+1] != null){//..And the spot we want to drop is not null.

					colorBlockArray [xIndex, i] = colorBlockArray [xIndex, i + 1];
					colorBlockArray [xIndex, i+1] = null;

					//print("XInded = "+xIndex+", i = "+i);
					colorBlockArray[xIndex, i].setXY (xIndex, i);
					colorBlockArray[xIndex, i].setOrigPos(colorBlockArray[xIndex, i].transform.position);
					colorBlockArray[xIndex, i].setVecPos(getArrayLocationVector3(xIndex,i));


					colorBlockArray[xIndex, i].shouldTransition = true;

					//colorBlockArray [xIndex, i+1].setXY (xIndex, i+1);
					//colorBlockArray[xIndex, i+1].setVecPos(getArrayLocationVector3(xIndex,i+1));
					//colorBlockArray[xIndex, i+1].transform.position = colorBlockArray[xIndex, i+1].getVecPos();


					didDrop = true;
				}
			}
			
		}
		return didDrop;	
	}
	
	//Drops a row down until either a block falls into place or nothing happens.
	bool dropRow(int xIndex)
	{
		bool didDrop = false;
		int temp = 0;
		bool bTemp = false;
		while (temp < ySize) {
			temp++;
			bTemp = dropRowOnce(xIndex, 0);
			if(bTemp){
				didDrop = true;
			}
		}
		return didDrop;
	}
	
	
	bool dropAllRows(){
		bool didDrop = false;
		bool bTemp = false;
		for(int x = 0; x < colorBlockArray.GetLength (0);x++){
			bTemp = dropRow(x);
			if(bTemp){
				didDrop = true;
			}
		}
		return didDrop;
	}


	//Sets the boolean values up, down, left, and right, back to false
	void resetDirectionCheck(){
		//print("DIRECTION RESET");
		up = false;
		down = false;
		left = false;
		right = false;
	}

	//Checks for special debug keys!
	void keyCheck(){
		if (Input.GetKeyDown (KeyCode.D)) {
			//print("DROPPIN' ROWS");
			dropAllRows();	
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			//print("FILLIN' ROWS");
			initEmptyBlocks();	
		}
		
		if (Input.GetKeyDown (KeyCode.C)) {
			//print("FILLIN' ROWS");
			checkForCombos();	
		}

		if (Input.GetKeyDown (KeyCode.Keypad3)) {
			//print("FILLIN' ROWS");
			stepIndex = 3;
		}
		if (Input.GetKeyDown (KeyCode.Keypad4)) {
			//print("FILLIN' ROWS");
			bool combo = hasCombos();
			print("HasCombo? "+combo);
		}
	}

	//Checks whether or not the block should swap with its neighbor or not if correct.
	void swapCheck ()
	{
		
		
		


		if (up || down || left || right) {
			
			
			if (up) {
				if(selected != null){
					//print("(UpSwap: "+selected.x+" "+selected.y+")");
					swapUp(selected.x, selected.y);
					//resetDirectionCheck();
				}

				
			}
			if (down) {
				if(selected != null){
					swapDown(selected.x, selected.y);
					//resetDirectionCheck();
				}

			}
			if (left) {
				if(selected != null){
					swapLeft(selected.x, selected.y);
					//resetDirectionCheck();
				}

			}
			if (right) {
				if(selected != null){
					swapRight(selected.x, selected.y);
					//resetDirectionCheck();
				}

			}
			
			
		}
		
	}
	
	
	
	//START DEBUGGING HERE, MAKE SURE THIS WORKS.
	
	/// Checks for combos, then sets the guilty party's isInCombo booleans to true.
	
	bool checkForCombos(){
		bool combosFound = false;
		//print("COMBOS GETTIN CHECKED");
		//check horizontal combos
		for (int y = 0; y < colorBlockArray.GetLength(1); y++) {

			for (int x = 0; x < colorBlockArray.GetLength(0)-2; x++) {
				if((colorBlockArray[x, y] != null)){
					int checkColor = colorBlockArray[x,y].getColor();

					if(colorBlockArray[x+1,y] != null && colorBlockArray[x+2,y] != null){
						if(checkColor == colorBlockArray[x+1,y].getColor() && (checkColor == colorBlockArray[x+2,y].getColor())){
							colorBlockArray[x,y].setScoreCombo(true);
							colorBlockArray[x+1,y].setScoreCombo(true);
							colorBlockArray[x+2,y].setScoreCombo(true);
							print("Combo found at: ("+x+", "+y+"), "+"("+(x+1)+", "+y+"), "+"("+(x+2)+", "+y+"), ");
							combosFound = true;
						}
					}
				}
				else{
					print("("+x+", "+y+") IS NULL");
				}
			}
		}
		//Then check vertical combos
		
		for(int x = 0; x < colorBlockArray.GetLength(0); x++) {
			for (int y = 0; y < colorBlockArray.GetLength(1)-2; y++) {
				if((colorBlockArray[x, y] != null)){	
					int checkColor = colorBlockArray[x,y].getColor();

					if(colorBlockArray[x,y+1] != null && colorBlockArray[x,y+2] != null){
						if(checkColor == colorBlockArray[x,y+1].getColor() && (checkColor == colorBlockArray[x,y+2].getColor())){
							colorBlockArray[x,y].setScoreCombo(true);
							colorBlockArray[x,y+1].setScoreCombo(true);
							colorBlockArray[x,y+2].setScoreCombo(true);

							print("Combo found at: ("+x+", "+y+"), "+"("+x+", "+(y+1)+"), "+"("+x+", "+(y+2)+"), ");
							combosFound = true;
						}
					}
				}
				else{
					print("("+x+", "+y+") IS NULL");
				}
			}
			
			
		}
		print("Thing was found!? = "+combosFound);
		return combosFound;
	}

	//Same as checkForCombos, but doesn't change anything. Just returns a boolean.
	bool hasCombos(){
		bool combosFound = false;
		//print("COMBOS GETTIN CHECKED");
		//check horizontal combos
		for (int y = 0; y < colorBlockArray.GetLength(1); y++) {
			
			for (int x = 0; x < colorBlockArray.GetLength(0)-2; x++) {
				if((colorBlockArray[x, y] != null)){
					int checkColor = colorBlockArray[x,y].getColor();
					
					if(colorBlockArray[x+1,y] != null && colorBlockArray[x+2,y] != null){
						if(checkColor == colorBlockArray[x+1,y].getColor() && (checkColor == colorBlockArray[x+2,y].getColor())){

							print("Combo found at: ("+x+", "+y+"), "+"("+(x+1)+", "+y+"), "+"("+(x+2)+", "+y+"), ");
							combosFound = true;
						}
					}
				}
				else{
					print("("+x+", "+y+") IS NULL");
				}
			}
		}
		//Then check vertical combos
		
		for(int x = 0; x < colorBlockArray.GetLength(0); x++) {
			for (int y = 0; y < colorBlockArray.GetLength(1)-2; y++) {
				if((colorBlockArray[x, y] != null)){	
					int checkColor = colorBlockArray[x,y].getColor();
					
					if(colorBlockArray[x,y+1] != null && colorBlockArray[x,y+2] != null){
						if(checkColor == colorBlockArray[x,y+1].getColor() && (checkColor == colorBlockArray[x,y+2].getColor())){

							
							print("Combo found at: ("+x+", "+y+"), "+"("+x+", "+(y+1)+"), "+"("+x+", "+(y+2)+"), ");
							combosFound = true;
						}
					}
				}
				else{
					print("("+x+", "+y+") IS NULL");
				}
			}
			
			
		}
		print("Thing was found!? = "+combosFound);
		return combosFound;
	}


	//CLEARS OUT WHAT NEEDS TO BE CLEARED OUT.
	//It also handles doling out scores.
	//It also passes damage to be applied to enemies in the enemy manager.
	void clearComboed(){
		// Amount of tiles destroyed
		int hits = 0;

		int[] damageArray = new int[4];

		for(int x = 0; x < colorBlockArray.GetLength(0); x++) {
			
			for (int y = 0; y < colorBlockArray.GetLength(1); y++) {
				if(colorBlockArray[x,y] != null){
					if(colorBlockArray[x,y].isInScoreCombo){



						hits++;//Increment number of hits.
						TILE_SCORE+= 2;//Increment our score

						damageArray[colorBlockArray[x,y].getColor ()] += hits * TILE_SCORE; //Tell how much damage we will be doing to this color

						removeBlock(x,y);//Kill the Batman.
					}
				}
			}
		}
		score += hits * TILE_SCORE;
		TILE_SCORE = 0;
		eMan.applyDamage(damageArray);

	}

	bool checkForCombos2(){
		bool combosFound = false;
		//print("COMBOS GETTIN CHECKED");
		//check horizontal combos
		for (int x = 0; x < colorBlockArray.GetLength(0); x++) {
			
			for (int y = 0; y < colorBlockArray.GetLength(1); y++) {
				if((colorBlockArray[x, y] != null)){


					//Check Horizontal first
					if(isInArray(x-1, y) && isInArray(x+1, y)){
						int checkColor = colorBlockArray[x,y].getColor();
						int checkColorPrev = colorBlockArray[x-1,y].getColor();
						int checkColorNext = colorBlockArray[x+1,y].getColor();

						//If they all match..
						if(checkColor == checkColorPrev && checkColor == checkColorNext){
							colorBlockArray[x,y].setScoreCombo(true);
							colorBlockArray[x-1,y].setScoreCombo(true);
							colorBlockArray[x+1,y].setScoreCombo(true);

							combosFound = true;
						}

					}


					//Then check Vertical
					if(isInArray(x, y-1) && isInArray(x, y+1)){
						int checkColor = colorBlockArray[x,y].getColor();
						int checkColorPrev = colorBlockArray[x,y-1].getColor();
						int checkColorNext = colorBlockArray[x,y+1].getColor();
						
						//If they all match..
						if(checkColor == checkColorPrev && checkColor == checkColorNext){
							colorBlockArray[x,y].setScoreCombo(true);
							colorBlockArray[x,y-1].setScoreCombo(true);
							colorBlockArray[x, y+1].setScoreCombo(true);
							combosFound = true;
						}
						
					}

				}
			
			}
	
		}
		return combosFound;
	}

	bool isInArray(int x, int y){
		if(x >= 0 && x < colorBlockArray.GetLength(0) && y >= 0 && y < colorBlockArray.GetLength(1)){
			return true;
		}
		return false;
	}


	//Maintains the touch input system. Everything having to do with that loop is here, more or less.
	void touchCheck(){

		if(allowTouch){

			if(Input.touchCount>0){
				
				touchesOld = new GameObject[touchList.Count];
				touchList.CopyTo(touchesOld);
				touchList.Clear();
				
				
				
				//Here is where we check each individual touch and see what it is doing.
				foreach(Touch touch in Input.touches){
					
					Ray ray = camera.ScreenPointToRay (touch.position);
					
					
					
					if(Physics.Raycast(ray, out hit, touchInputMask)){//If we send out a ray and it hits something in the touchInputMask...



						//GameObject recipient = hit.transform.gameObject;
						//touchList.Add (recipient);
						
						
						
						if (touch.phase == TouchPhase.Began){
							
							if(!(selected != null)){

								//WE HIT SOMETHING! Start the timer.
								currentState = State.Dragging;

								//Also, take note of the thing that we found.
								colorBlock foundthing = hit.transform.GetComponent<colorBlock>();
								selected = foundthing;
							}
							
						}
						
						else if (touch.phase == TouchPhase.Ended){
							//recipient.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);

							currentState = State.Matching;

							//selected.transform.position = selected.gameObject.GetComponent<colorBlock>().gridPos;
							if(selected != null){
								selected.SendMessage ("releaseBlock", SendMessageOptions.DontRequireReceiver);
								selected = null;
								//print("released");
								stepIndex = 1;
							}
							
							
						}
						
						else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
							//recipient.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
							
						}
						
						
						else if (touch.phase == TouchPhase.Canceled){
							//recipient.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);

							currentState = State.Matching;

							selected.SendMessage ("releaseBlock", SendMessageOptions.DontRequireReceiver);
							//print("released");
							selected = null;
							stepIndex = 1;
							
						}
						else if(touch.phase == TouchPhase.Ended){

							currentState = State.Matching;

							selected.SendMessage ("releaseBlock", SendMessageOptions.DontRequireReceiver);
							selected = null;
							stepIndex = 1;
						}
					



					}
					
				}

			}
			
			
			//If we have something selected, move it.
			if(selected != null){
				Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				selected.transform.position = new Vector3(curPosition.x, curPosition.y, selected.transform.position.z);
				swapDistCheck();
			}

		}
	}


	/// <summary>
	/// Finds the distance between the touched point and the original point of the colorBlock held.
	/// It then sets whether or not you should swap up, down, left, or right based on those ideas.
	/// </summary>
	/// <returns>The distance.</returns>
	void swapDistCheck(){
		if(selected!= null){
			Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);



			float xDist = touchPos.x - selected.vecPos.x;
			xDistCheck = xDist;
			float yDist = touchPos.y - selected.vecPos.y;
			yDistCheck = yDist;

			if(Mathf.Abs(xDist) > checkDistance){
				if(xDist > 0){
					right = true;
				}
				if(xDist < 0){
					left = true;
				}
			}

			if(Mathf.Abs(yDist) > checkDistance){
				if(yDist > 0){
					up = true;
				}
				if(yDist < 0){
					down = true;
				}
			}
		}
		swapCheck();

	}

	//Transitions any colorBlocks to their correct position via lerp every frame.
	public void lerpTransitionals(){

		for (int i = 0; i < colorBlockArray.GetLength(0)-1; i++){
			for(int j = 0; i < colorBlockArray.GetLength(1)-1; i++){
				if(colorBlockArray[i,j] != null){
					if(colorBlockArray[i,j].shouldTransition){
						colorBlockArray[i, j].transform.position = Vector3.Lerp (colorBlockArray[i,j].origPos, colorBlockArray[i,j].vecPos, transitionPercent);

				}
			}
		}
		transitionPercent+=.1f;
		if(transitionPercent > 1){
				transitionPercent = 0;
				stepIndex = 3;
			}
		}
	}

	public void breakBadGeneration(){

	}

	// Update is called once per frame
	void Update ()
	{			
		bool restart;
		UpdateGUI();


		if(stepIndex == 0){//The player takes their turn.
			keyCheck();
			touchCheck();
			timerCheck();

			//If we're done, set the stepIndex to the next step, and change our state as such
			if(allowTouch == false){
				stepIndex = 1;
				currentState = State.Matching;
			}
		}


		//buttonCheck ();
		if(stepIndex == 1){
			checkForCombos2();//We find everything that needs to get changed here.
			clearComboed();//Get rid of everythng that needed to be changed here
			dropAllRows();//Then we drop all of the rows until there are no more rows to drop.


			stepIndex = 2;
		}

		if(stepIndex == 2){

			lerpTransitionals();

		}

		if (stepIndex == 3){
			
			initEmptyBlocks();
			
			arrangeBlocks ();

			print("Restart Check!");
			restart = hasCombos();

			//restart = checkForCombos();
			print("Restart: "+restart);

			restart = checkForCombos2();
			print("Second opinion: "+restart);
			if(restart){
				//allowTouch = false;

				stepIndex = 1;
				print("I'MMA DO IT AGAIN");
			}
			else{
				//allowTouch = true;
				checkForCombos2();//We find everything that needs to get changed here.
				clearComboed();//Get rid of everythng that needed to be changed here
				dropAllRows();//Then we drop all of the rows until there are no more rows to drop.

				currentState = State.Normal;
				stepIndex = 0;
				print("I'm not doing it again.");
			}

		}
		
	}
//=======

    

    /// <summary>
    /// Updates the GUI Elements for the match-3 game (Score and Timer)
    /// </summary>
    void UpdateGUI()
    {
        // Update Score
        scoreText.text = "Score: " + score;

        // Update Timer
        timerText.text = "Timer: " + timer.ToString("0.00");
    }
	
    void endTurn()
    {
        bool noMoreCombos = true;
        while (noMoreCombos != false)
        {
            checkForCombos();
            noMoreCombos = dropAllRows();
            initEmptyBlocks();
        }

        currentState = State.Normal;
    }

    //Checks whether or not the arrow input was correct or not and removes the bottom block if correct.
    void buttonCheck()
    {
        UpdateGUI();

        if (currentState == State.GameOver)
        {
            // Nothing
        }

        else
        {
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;

            // Decrease our timer if the user is currently swapping tiles
			timerCheck();

            //Debug that gives the state of the array
            /*
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
            */
            if (Input.GetKeyDown(KeyCode.D))
            {
                print("DROPPIN' ROWS");
                dropAllRows();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                print("FILLIN' ROWS");
                initEmptyBlocks();
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("SPACE PRESSED");
                if (colorBlockArray[cursorLocX, cursorLocY] != null)
                {
                    if (!colorBlockArray[cursorLocX, cursorLocY].isBlockSelected())
                    {
                        currentState = State.Dragging;

                        print("BLOCK SET TO TRUE");
                        setColorBlockSelection(cursorLocX, cursorLocY, true);
                        hasBlockSelected = true;
                    }
                    else
                    {
                        currentState = State.Matching;

                        print("BLOCK SET TO FALSE");
                        setColorBlockSelection(cursorLocX, cursorLocY, false);
                        hasBlockSelected = false;
                        endTurn();

                    }
                    refreshColorBlock(cursorLocX, cursorLocY);
                }
            }



            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                up = true;
                print("UP PRESSED");

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                down = true;
                print("DOWN PRESSED");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                left = true;
                print("LEFT PRESSED");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                right = true;
                print("RIGHT PRESSED");
            }


            if (up || down || left || right)
            {


                if (up)
                {
                    if (hasBlockSelected)
                    {
                        swapUp(cursorLocX, cursorLocY);
                    }
                    moveUpCheck(cursorLocX, cursorLocY);

                }
                if (down)
                {
                    if (hasBlockSelected)
                    {
                        swapDown(cursorLocX, cursorLocY);
                    }
                    moveDownCheck(cursorLocX, cursorLocY);
                }
                if (left)
                {
                    if (hasBlockSelected)
                    {
                        swapLeft(cursorLocX, cursorLocY);
                    }
                    moveLeftCheck(cursorLocX, cursorLocY);
                }
                if (right)
                {
                    if (hasBlockSelected)
                    {
                        swapRight(cursorLocX, cursorLocY);
                    }
                    moveRightCheck(cursorLocX, cursorLocY);
                }

            }
        }

        

        

    }

    // TODO: Test mobile functionality for these!

    /// <summary>
    /// 
    /// </summary>
    void pickupBlock()
    {
        currentState = State.Dragging;
    }

	/*
	 * This is Freddy's code to check whether or not the timer should be decrementing.
	 * Ported to its own method because buttonCheck() isn't being used any more.
	*/
	void timerCheck(){
		if (currentState == State.Dragging)
		{
			timer -= Time.deltaTime;
			
			if (timer <= 0)
			{
				// Drop the block at current location
				setColorBlockSelection(cursorLocX, cursorLocY, false);
				hasBlockSelected = false;
				endTurn();
				
				currentState = State.GameOver;
				timer = 0.0f;
			}
		}
	}

    /*
    void releaseBlock()
    {
        currentState = State.Matching;

        // call colorblock's "releaseBlock" method ?
    }*/
}
