using UnityEngine;
using System.Collections;

public class colorBlock : MonoBehaviour
{
	System.Random rnd = new System.Random ();
	int blockColor = 0;
	public GameObject redSphere;
	public GameObject blueSphere;
	public GameObject greenSphere;
	public GameObject yellowSphere;
	public GameObject selectedCursor;
	public bool isSelected = false;
	public bool isInScoreCombo = false;
	public bool shouldTransition = false;//Tells whether or not the block should be moving.

	public float scale = 1;

	public int x = 0;
	public int y = 0;
	public Vector3 vecPos = new Vector3(0,0,0);

	public Vector3 origPos = new Vector3(0,0,0);

	public LayerMask touchInputMask;
	
	public colorBlock(){

	}


	// Use this for initialization
	void Start ()
	{
		disableColors();
		disableCursor();
		randomizeBlock();

		transform.localScale = new Vector3(scale, scale, scale);

	}

	public void setVecPos(Vector3 nuPos){
		vecPos = nuPos;
	}
	
	public Vector3 getVecPos(){
		return vecPos;
	}

	public void setOrigPos(Vector3 nuPos){
		origPos = nuPos;
	}
	
	public Vector3 getOrigPos(){
		return origPos;
	}
	

	public void setXY(int nuX, int nuY){
		x = nuX;
		y = nuY;
	}


	public void disableCursor(){
		selectedCursor.SetActive(false);
	}

	public void enableCursor(){
		foreach (Transform child in this.transform)
		{
				child.gameObject.SetActive(true);
			
		}
	}

	//Decides whether or not the selectedCursor should render or not.
	public void cursorCheck(){
		if(isSelected){
			enableCursor();
		}
		else{
			disableCursor();
		}
	}

	//Returns whether or not the block's isSelected variabe. Tells whether or not the block is selected.
	public bool isBlockSelected(){
		return isSelected;
	}


	//Disables the renderer for ALL of the colors.
	public void disableColors ()
	{
		blockColor = -1;
			redSphere.renderer.enabled = false;
			blueSphere.renderer.enabled = false;
			greenSphere.renderer.enabled = false;
			yellowSphere.renderer.enabled = false;
			

	}

	public void disableColor (int chosenColor)
	{

				if (chosenColor == 0) {
						redSphere.renderer.enabled = false;
				}
				if (chosenColor == 1) {
						blueSphere.renderer.enabled = false;
				}
				if (chosenColor == 2) {
					greenSphere.renderer.enabled = false;
				}
				if (chosenColor == 3) {
						yellowSphere.renderer.enabled = false;
				}

	}

	//Sets renderstate of selected color to true;
	public void enableColor (int chosenColor)
	{

			if (chosenColor == 0) {
					redSphere.renderer.enabled = true;
			}
			if (chosenColor == 1) {
					blueSphere.renderer.enabled = true;
			}
			if (chosenColor == 2) {
					greenSphere.renderer.enabled = true;
			}
			if (chosenColor == 3) {
					yellowSphere.renderer.enabled = true;
			}
			
	}

	//Sets whether the block going to be a part of a chain.
	public void setSelected (bool nuBool)
	{
		isSelected = nuBool;
	}

	//Returns which way the block is currently rotated to.
	public int getColor ()
	{
		return blockColor;
	}

	//Disables all other color renderers to false, and then activates the given color.
	public void setBlockColor (int chosenColor)
	{
			disableColors();
		blockColor = chosenColor;
			enableColor(chosenColor);
	}

	public void setScoreCombo(bool nuState){
		isInScoreCombo = nuState;
	}


	//Tells the block to redraw itself with all of its new changed features.
	public void refresh(){
		setBlockColor(blockColor);
		cursorCheck();
	}


	public void randomizeBlock(){
		int i = 0;
		int top = rnd.Next (2,11);
		while(i < top){
			int yo = rnd.Next (0, 4);
			blockColor = yo;
			//print(yo);
			setBlockColor (yo);
			i++;
		}
	}

	public void releaseBlock(){
		transform.position = vecPos;
	}


	public void selfDelete(){
		Destroy(this.gameObject);
	}

	public void setMyScale(float nuScale){
		scale = nuScale;
	}

	public float getMyScale(){
		return scale;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.localScale = new Vector3(scale, scale, scale);
	}
}
