using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEffectShow : MonoBehaviour {

    public GameObject selectEffectCube;

    public void ShowSelected() {

        selectEffectCube.SetActive(true);

    } // ShowSelected()

    public void CancelShowSelected() {

        selectEffectCube.SetActive(false);

    } // CancelShowSelected()

}
