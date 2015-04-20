using UnityEngine;
using System.Collections;

public class characterScript : MonoBehaviour {

	//Our character's class
	public charClass myClass;

	//Our heath
	public int maxHealth = 500;
	public int curHealth = 500;

	//Our attributes;
	public int atk = 10;
	public int pDef = 10;
	public int spd = 10;
	public int dex = 10;
	public int mDef = 10;
	public int mag = 10;
	public string status;

	public bool deathState= false;

	public itemClassScript[] itemList;// = new itemClassScript[20];

	public void takeDamage(int damage){
		curHealth-=damage;
		deathCheck();
	}

	public void deathCheck(){
		if(curHealth <= 0){
			deathState = true;
		}
	}

	public bool isDead(){
		return deathState;
	}

	public int attack(){
		return atk;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
