using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShow : MonoBehaviour {

	// Use this for initialization
	void Start () {

        FindObjectOfType<PanelButtonControl>().JoinToShowList(gameObject) ;

    }

}
