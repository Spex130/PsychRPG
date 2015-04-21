using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class textWrapper : MonoBehaviour {

	public Text text;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public string getText(){
		return text.text;
	}

	public void setText(string nuText){
		text.text = nuText;
	}
}
