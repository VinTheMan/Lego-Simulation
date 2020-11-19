using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainEditComponent : MonoBehaviour {

    public void TurnOffEditFunction() {

        GetComponent<GM>().enabled = false;
        GetComponent<RotateGM>().enabled = false;


    } // TurnOffEditFunction()

    public void TurnOnEditFunction() {

        GetComponent<GM>().enabled = true ;
        GetComponent<RotateGM>().enabled = false;

    } // TurnOnEditFunction() - return to Edit ;


}
