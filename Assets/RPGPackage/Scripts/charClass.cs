using UnityEngine;
using System.Collections;

public class charClass : MonoBehaviour {

	enum Class{Warrior, Tanker, Sorcerer, Druid, Thief};

	//We use this to hold the skills that this class has.
	//public skillScript[] mySkillList;

	//We award this many points when we level up using this class.
	public int atk = 10;
	public int pDef = 10;
	public int spd = 10;
	public int dex = 10;
	public int mDef = 10;
	public int mag = 10;


	//The more we level this up, the more skills we will be able to use.
	public int classLevel = 1;

	//Skills have a classlevel requirement.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
