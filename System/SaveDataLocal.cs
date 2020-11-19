using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO ;
using UnityEngine;
using System.Runtime.InteropServices ; // 



[System.Serializable]
public class SaveDataLocal {
    public string[] legoNames =  new string[0];
    public Vector3[] positions = new Vector3[0] ;
    public Quaternion[] rotations = new Quaternion[0] ;
    public int[] step = new int[0] ;
    public int[] whichGroup = new int[0] ;
    public int[] groupType = new int[0] ;
    // scale is the same ;
}


