
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking ;

public class AccessManager : MonoBehaviour {


    // this four public should be change to private ;
    public bool AccessViewLego ;
    public bool AccessEditLego ;
    public bool AccessExtraEdit ;
    public bool AccessExportImg ;
    public GameObject ACC ;
    public GameObject FCS ;

    public string urlCheck ;
    public bool testing ;

    private ViewA viewA ;
    private EditA editA ;
    private ExportA exportA ;
    private ExtraEditA extraEditA ;

    private void Awake() {

        /*
        viewA = FindObjectOfType<ViewA>() ;
        if ( viewA == null )
            AccessViewLego = false;
        else
            AccessViewLego = true ; 

        editA = FindObjectOfType<EditA>();
        if ( editA == null )
            AccessEditLego = false;
        else
            AccessEditLego = true;

        exportA = FindObjectOfType<ExportA>();
        if ( exportA == null )
            AccessExportImg = false;
        else
            AccessExportImg = true; 

        extraEditA = FindObjectOfType<ExtraEditA>();
        if ( extraEditA == null )
            AccessExtraEdit = false;
        else
            AccessExtraEdit = true;
        */
        if (!testing && Application.absoluteURL != urlCheck)
            Destroy(gameObject);
        else {
            FCS.SetActive(true);
            ACC.SetActive(true);
        } // else

    } // Awake()

    public void SetAccess( string _access ) {

        if ( _access == "student" || _access == "teacher" || _access == "admin" ) {
            AccessViewLego = true ;
            AccessEditLego = false ;
            AccessExtraEdit = false ;
            AccessExportImg = false ;
        } // if
        else if ( _access == "editor") {
            AccessViewLego = true;
            AccessEditLego = true;
            AccessExtraEdit = true;
            AccessExportImg = true;
        } // else if
        else {
            Debug.LogError("Get Error Permission.") ;
        } // else


    } // SetAccess()

    public bool CheckViewLego() {
        return AccessViewLego ;
    } // CheckViewLego()

    public bool CheckEditLego() {
        return AccessEditLego ;
    } // CheckEditLego() 

    public bool CheckExtraEdit() {
        return AccessExtraEdit ;
    } // CheckExtraLego() 

    public bool CheckExportImg() {
        return AccessExportImg ;
    } // CheckExportImg() 

    
}
