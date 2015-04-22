using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class itemPageManager : MonoBehaviour {

	public characterScript player;

	public Slider viewSlider;

	public Text textTemplate;

	public itemClassScript[] itemList;

	public Text[] itemStringList;

	public int selectedItemID = 0;

	public bool isDrawn;

	public float textOffset = 100f;

	//HARD CODING VARIABLES


	//The point at which description text will generate
	public GameObject textDescrPoint;

	//The point at which the item list text will generate
	public GameObject textListPoint;


	// Use this for initialization
	void Start () {
	
		isDrawn = false;
		itemStringList = new Text[itemList.Length];

		//Get our list of items
		getItemList();

	}
	
	// Update is called once per frame
	void Update () {
		if(isDrawn == false){
			drawItemList();
			isDrawn = true;
		}

	}

	//Gets the list of items from our player to show;
	public void getItemList(){
		//itemList = player.itemList;
	}

	//Destroys all of the Text objects we have created to properly show the text list.
	public void clearDrawnText(){
		for(int i = 0; i < itemStringList.Length; i++){
			if(!(itemList[i] == null)){
				GameObject.Destroy(itemStringList[i]);
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
		for(int i = 0; i < itemList.Length; i++){
			itemStringList[i] = (Text)GameObject.Instantiate(textTemplate, textListPoint.transform.position, Quaternion.identity);
			itemStringList[i].transform.parent = textListPoint.transform;
			itemStringList[i].transform.localPosition = new Vector3(0,0 - (i * textOffset),0);

			itemStringList[i].text = itemList[i].name;
			//itemStringList[i].transform.position = new Vector3(itemStringList[i].transform.position.x, itemStringList[i].transform.position.y - (i * 10), itemStringList[i].transform.position.z);
		}

	}

}
