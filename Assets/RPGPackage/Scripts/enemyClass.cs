using UnityEngine;
using System.Collections;

public class enemyClass : MonoBehaviour {

	public int hp = 10;
	public int hpMax;
	public int def = 10;
	public int atk = 10;

	public string name = "Opponent";

	public bool deathState = false;

	public int attack(){
		return atk;
	}

	public void takeDamage(int damage){
		hp -= damage;
		deathCheck();
	}

	public void deathCheck(){
		if (hp <= 0){
			deathState = true;
		}
	}

	public bool isDead(){
		return deathState;
	}

	// Use this for initialization
	void Start () {
		hpMax = hp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
