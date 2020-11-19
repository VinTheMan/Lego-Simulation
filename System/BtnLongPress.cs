using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using UnityEngine.Events ;
using UnityEngine.EventSystems ;

public class BtnLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public RotatingCameraScript camera ;

    public string cmd ;
    public bool PressDown ;

	
    public void OnPointerDown(PointerEventData eventData) {
        PressDown = true ;
    } // OnPointerDown()
    public void OnPointerUp(PointerEventData eventData) {
        Reset() ;
    } // OnPointerDown()

    
    void Update () {

        if (PressDown) {

            if (cmd == "u")
                camera.Btn_CameraMoveUp();
            else if (cmd == "r")
                camera.Btn_CameraMoveRight();
            else if (cmd == "l")
                camera.Btn_CameraMoveLeft();
            else if (cmd == "d")
                camera.Btn_CameraMoveDown();
            else if (cmd == "+")
                camera.Btn_CameraBigger();
            else if (cmd == "-")
                camera.Btn_CameraSmaller();

        } // if
		
	} // Update()

    private void Reset() {
        PressDown = false ;
    } // Reset()


}
