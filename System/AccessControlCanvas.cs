using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessControlCanvas : MonoBehaviour {

    public GameObject ViewAccess ;
    public GameObject EditAccess ;
    public GameObject ExtraEditAccess ;
    public GameObject ExportAcess ;
    private AccessManager accessManager ;
    private bool Exporting ;

	// Use this for initialization
	void Awake () {

        accessManager = FindObjectOfType<AccessManager>();
        if (accessManager == null) {
            // AccessManager isn't access or exist, then destory all the UI Canvas ;

            Debug.LogError("AccessManager not found!");
            Destroy(ViewAccess) ;
            Destroy(EditAccess) ;
            Destroy(ExtraEditAccess) ;
            Destroy(ExportAcess) ;

        }

        if (ViewAccess == null)
            Debug.LogError("Canvas Access ViewAccess not asign");
        if (EditAccess == null)
            Debug.LogError("Canvas Access EditAccess not asign");
        if (ExtraEditAccess == null)
            Debug.LogError("Canvas Access ExtraEditAccess not asign");
        if (ExportAcess == null)
            Debug.LogError("Canvas Access ExportAcess not asign");
        Exporting = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (accessManager.CheckViewLego())
            ViewAccess.SetActive(true);
        else
            ViewAccess.SetActive(false);

        if ( accessManager.CheckEditLego() && !Exporting )
            EditAccess.SetActive(true);
        else
            EditAccess.SetActive(false);
        if (accessManager.CheckExtraEdit())
            ExtraEditAccess.SetActive(true);
        else
            ExtraEditAccess.SetActive(false);
        if (accessManager.CheckExportImg())
            ExportAcess.SetActive(true);
        else
            ExportAcess.SetActive(false);
    }

    public void NowGoExport() {
        Exporting = true ;
    } // nowGoExport()

    public void NowExitExport() {
        Exporting = false ;
    }
}
