using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;


public class NetworkControl : MonoBehaviour {

    private AccessManager accessManager;
    private UnitManager unitManager;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string GetPermission() ;

    [DllImport("__Internal")]
    private static extern string GetSaveData() ;

    [DllImport("__Internal")]
    private static extern void Test_ShowGetPermissionName( string pName ) ;

    [DllImport("__Internal")]
    private static extern void SendSaveData(string save) ;

    [DllImport("__Internal")]
    private static extern void ImageDownloader(string str, string fn);

#else
    private static string GetPermission() {
        return "editor" ;
    }
    private static void Test_ShowGetPermissionName( string pName ) {
        Debug.Log("Now running in offine, " + pName + " permission.");
    }
    private static string GetSaveData() {
        return "{\"legoNames\":[\"Lego033\"],\"positions\":[{\"x\":0.0,\"y\":0.0,\"z\":0.0}],\"rotations\":[{\"x\":0.0,\"y\":0.0,\"z\":0.0,\"w\":1.0}],\"step\":[0],\"whichGroup\":[0],\"groupType\":[0]}" ;
        // return null ;
    }
    private static void SendSaveData( string save ){
        Debug.Log("Save contain:" + save ) ;
    } // SendSaveData()
    private static void ImageDownloader(string str, string fn) {
        System.IO.File.WriteAllBytes(fn, System.Convert.FromBase64String(str) );
        Debug.Log("Now running in offline mode.") ;
    } // ImageDownloader()
    #endif

    private void Start() {

        accessManager = FindObjectOfType<AccessManager>() ;
        if ( accessManager == null )
            Debug.Log("NetworkControl cannot find AccessManager.") ;

        unitManager = FindObjectOfType<UnitManager>() ;
        if (unitManager == null)
            Debug.Log("NetworkControl cannot find UnitManager.") ;

        #if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("Now running in browser") ;
        #else
        accessManager.SetAccess(GetPermission());
        Test_ShowGetPermissionName(GetPermission());
        // unitManager.LoadSaveDataAtStart(GetSaveData());
        #endif
        // StartCoroutine(TryThrowing());

        
        // accessManager.SetAccess("editor") ; // this line in final must be removed ! ;


    } // Start()

    public void SaveTheProject( string _save ) {
        Application.ExternalCall("SendSaveData", _save) ;
        
    } // SaveTheProject()

    public void SavePicture( byte[] _bytes, string _filename, int _index ) {
        /*
        // 1 . This way is create a download link ; 
        ImageDownloader( System.Convert.ToBase64String(_bytes), _filename ) ;
        */

        // 2 . This way is send to browser js, and send value to backend ; 
        #if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall("SendSavePicture", _bytes, (_index+1) ) ;
        
        #else
        System.IO.File.WriteAllBytes( _filename , _bytes) ;

        #endif

    } // SavePicture()

    /*
    IEnumerator GetPermission() {

        UnityWebRequest www = UnityWebRequest.Get("") ;
        yield return www.SendWebRequest() ;
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {

        }


    }

    IEnumerator GetProjectData() {

        UnityWebRequest www = UnityWebRequest.Get("");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {

        }

    }

    IEnumerator TryThrowing() {
        UnityWebRequest www = UnityWebRequest.Get("http://mimir-env.hk2qb94ebi.ap-northeast-1.elasticbeanstalk.com/checkpermission");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

    }
    */
}
