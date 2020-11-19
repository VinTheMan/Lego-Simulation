using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScriptRot : MonoBehaviour {

	public GameObject GM ; 
	public bool isActive ; // 現在在旋轉的零件
	void Start () {
		GM = GameObject.FindGameObjectWithTag("GM") ; 
		isActive = false ;
	} //Start
	
	// Update is called once per frame
	void Update () {
		if( GM.GetComponent<GM>().enabled == true ) {
            GetComponent<MouseScriptZ>().enabled = true;
            GetComponent<MouseScriptZ>().isActive = false;
            GetComponent<MouseScriptZ>().mouseFu = false;
            GetComponent<MouseScriptZ>().TurnOffOtherMouseScript();
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        } // if

		if (Input.GetMouseButtonDown(0) && isActive) {
            isActive = false ;
            GetComponent<Outline>().enabled = false ;
        } // if

        if( isActive ) {
            // do nothing 
        } // if
        else
        {
            // do nothing 
        } // else
	} // update

	public void TurnOffOtherMouseScript() {
        GetComponent<MouseScriptZ>().isActive = false;
		GetComponent<MouseScriptZ>().enabled = false;
	} //TurnOffOtherMouseScript

 void changeIsActive2(){
    isActive = true ;
    GetComponent<Outline>().enabled = false ;
 }// Change!


} // end of class
