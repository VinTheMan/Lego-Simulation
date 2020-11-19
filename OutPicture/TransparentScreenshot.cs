using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentScreenshot : MonoBehaviour {

    public Camera mainCamera ;
    public RenderTexture rt ;

    private void Start() {

        // mainCamera = FindObjectOfType<Camera>() ;
        // if (mainCamera == null)
        //    Debug.LogError("TransparentScreenShot script cannot find mainCamera.") ;

    }


    public void ScreenshotInTransparentBackground() {

        GameObject newCameraOBJ = new GameObject() ;
        GameObject newCamera = Instantiate(newCameraOBJ) as GameObject ;
        newCamera.AddComponent<Camera>() ;

        newCamera.GetComponent<Camera>().CopyFrom(mainCamera) ;

        newCamera.GetComponent<Camera>().targetTexture = rt ;

        newCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor ;
        newCamera.GetComponent<Camera>().backgroundColor = Color.clear ;

        newCamera.gameObject.AddComponent<TransparentBackgroundScreenshotRecorder>() ;
        


    } // ScreenshotInTransparentBackground()


} // TransparentScreenshot()
