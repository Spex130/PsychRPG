using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class itemPageManager : MonoBehaviour {

	public characterScript player;

	public Slider viewSlider;

	public Text textTemplate;

	public itemClassScript[] itemList;

	public string[] itemStringList;

	public int selectedItemID = 0;


	//The point at which description text will generate
	public GameObject textDescrPoint;

	//The point at which the item list text will generate
	public GameObject texListPoint;


	// Use this for initialization
	void Start () {
	
		//Get our list of items
		getItemList();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Gets the list of items from our player to show;
	public void getItemList(){
		//itemList = player.itemList;
	}

	//Destroys all of the Text objects we have created to properly show the text list.
	public void clearDrawnText(){

		for(int i = 0; i < itemList.Length; i++){
			if(itemList[i] != null){
				//GameObject.Destroy(itemStringList[i]);
			}
		}

		/*
		for(int i = 0; i < itemList.Length; i++){
			if(itemList[i] != null){
				GameObject.Destroy(itemList[i]);
			}
		}//*/
	}

	public void drawItemList(){

		float verticalOffset = -150;

		clearDrawnText();
		for(int i = 0; 0 < itemList.Length; i++){
			//itemStringList[i] = (Text)GameObject.Instantiate(textTemplate, textDescrPoint.transform.position, Quaternion.identity);
			//itemList[i].
		}
	}

}
