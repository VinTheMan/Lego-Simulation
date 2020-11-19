using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainToggle : MonoBehaviour {

    private int toggleNum ;

    public void SetToggleConnectToUnit( int _toggleNum ) {

        toggleNum = _toggleNum;

    } // SetToggleConnectToUnit

    public int GetToggleNum() {

        return toggleNum;

    } // Get()


}
