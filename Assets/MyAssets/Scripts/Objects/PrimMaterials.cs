using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrimMaterials
{
    [SerializeField]
    int count;
    private int currentInsert;
    [SerializeField]
    public Definitions.Stuff stuff;

    public void sumCurrentInsert(int value){
        currentInsert += value;
    }
    public void resetInsert(){
        currentInsert = 0;
    }

    public bool isComplete(){
        return count == currentInsert;
    }
}
