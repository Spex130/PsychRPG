using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {


	public Color defaultColor;
	public Color selectedColor;
	private Material mat;

	public LayerMask touchInputMask;
	

	private RaycastHit hit;



	void OnTouchDown(){
		mat.color = selectedColor;
		if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary){
			followTouch();
		}
	}

	void OnTouchUp(){
		mat.color = defaultColor;
	}

	void OnTouchStay(){
		mat.color = selectedColor;
	}

	void OnTouchExit(){
		mat.color = defaultColor;
	}


	void followTouch(){
	

		Vector3 curScreenPoint = new Vector3(Input.GetTouch(0).position.x *Time.deltaTime, Input.GetTouch(0).position.y *Time.deltaTime, (float)-1.475328);
		//Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		//Vector3 curPosition = Input.GetTouch (0).deltaPosition;
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

		print(curPosition);
		//transform.position = curPosition;
		transform.position = new Vector3(curPosition.x, curPosition.y, transform.position.z);
		
	}
}
