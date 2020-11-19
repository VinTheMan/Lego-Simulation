using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessNextBtn : MonoBehaviour {

    public Toggle ViewT ;
    public Toggle EditT ;
    public Toggle ExportT ;
    public Toggle ExtraEditT ;

    public GameObject AccessManager ;
    public GameObject CanvasSet ;

    ViewA vA ;
    EditA edA ;
    ExportA expA ;
    ExtraEditA exteA;

    private void Start() {

        vA = FindObjectOfType<ViewA>() ;
        edA = FindObjectOfType<EditA>() ;
        expA = FindObjectOfType<ExportA>() ;
        exteA = FindObjectOfType<ExtraEditA>() ;



        if (vA == null)
            Debug.LogError("AccessNextBtn, vA missing!") ;
        if (edA == null)
            Debug.LogError("AccessNextBtn, edA missing!");
        if (expA == null)
            Debug.LogError("AccessNextBtn, expA missing!");
        if (exteA == null)
            Debug.LogError("AccessNextBtn, exteA missing!");


    } // Start()

    public void btn_AccessNext() {

        if (ViewT.isOn)
            vA.gameObject.SetActive(true);
        else
            vA.gameObject.SetActive(false);

        if (EditT.isOn)
            edA.gameObject.SetActive(true);
        else
            edA.gameObject.SetActive(false);

        if (ExportT.isOn)
            expA.gameObject.SetActive(true);
        else
            expA.gameObject.SetActive(false);

        if (ExtraEditT.isOn)
            exteA.gameObject.SetActive(true);
        else
            exteA.gameObject.SetActive(false);

        AccessManager.SetActive(true) ;
        CanvasSet.SetActive(true) ;
        gameObject.SetActive(false) ;


    } // btn_AccessNext()


}
