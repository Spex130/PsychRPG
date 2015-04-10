using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class profileStatListener : MonoBehaviour {

	public characterScript player;

	public Text health;
	public Text attack;
	public Text pDef;
	public Text magic;
	public Text speed;
	public Text dex;
	public Text mDef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		health.text = "Max Health: "+player.curHealth;
		attack.text = "Attack: "+player.atk;
		pDef.text = "Phys. Def.: "+player.pDef;
		magic.text = "Magic: "+player.mag;
		speed.text = "Speed: "+player.spd;
		dex.text = "Dexterity: "+player.dex;
		mDef.text = "Magic Def.: "+player.mDef;
	}
}
