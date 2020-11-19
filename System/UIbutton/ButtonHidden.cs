using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHidden : MonoBehaviour {

	// Use this for initialization
	void Start () {

        FindObjectOfType<PanelButtonControl>().JoinToHiddenList(gameObject) ;

	}

}
