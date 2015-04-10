using UnityEngine;
using System.Collections;

/*
 * dropBlock is the standard Black class that stands for the arrows which the player deals with on a regular basis.
 */
public class dropBlock : MonoBehaviour {

	enum direction{UP, RIGHT, DOWN, LEFT};
	public int currentDirection = 0;
	bool isSelectedBlock = false;
	public GameObject block;
	
	public dropBlock(int currDirec, bool isSelected){
		currentDirection = currDirec;
		isSelectedBlock = isSelected;
	}

	//Tells how the block should be rotated.
	public void setDirection(int nuDirection){
		if(nuDirection > 3){
			currentDirection = 0;
		}
		else{
			currentDirection = nuDirection;
		}
	}

	public void selfDelete(){
		Destroy(this.gameObject);
	}

	//Sets whether the block is at the bottom of the pile, the block that the player is currently dealing with.
	public void setSelected(bool nuBool){
		isSelectedBlock = nuBool;
	}

	//Returns which way the block is currently rotated to.
	public int getCurrentDirection(){
		return currentDirection;
	}


	//Randomizes which way the block is facing. Must be told whether or not the block is the currently selected block.
	public void randomizeBlock(bool nuIsSelected){
		currentDirection = Random.Range(0,3);
		isSelectedBlock = nuIsSelected;
	}
}
