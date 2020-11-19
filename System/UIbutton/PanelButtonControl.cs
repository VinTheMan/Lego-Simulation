using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButtonControl : MonoBehaviour {

    List<GameObject> hiddenButtonList ;
    List<GameObject> showButtonList ; // the + ;

	// Use this for initialization
	void Start () {

        hiddenButtonList = new List<GameObject>() ;
        showButtonList = new List<GameObject>() ;

	} // Start()
	
    public void JoinToHiddenList( GameObject btn ) {

        hiddenButtonList.Add(btn) ;

    } // JoinH()

    public void JoinToShowList( GameObject btn ) {

        showButtonList.Add(btn) ;

    } // JoinS()

	public void MinAllBtn() {

        for ( int i = 0; i < hiddenButtonList.Count ; i++ ) {

            hiddenButtonList[i].SetActive(false) ;

        } // for
        for ( int i = 0 ; i < showButtonList.Count ; i++ ) {

            showButtonList[i].SetActive(true) ;

        } // for


    } // MinAllBtn()

}
