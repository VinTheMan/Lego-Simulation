using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices; // 

public struct ListOfStep{
    public List<int> contents;
    public List<int> transparentContents ;
    
} // class ListOfStep()

[System.Serializable]
public class SaveDataStep {

    public int[] content = new int[0];
    public int[] step = new int[0];

}