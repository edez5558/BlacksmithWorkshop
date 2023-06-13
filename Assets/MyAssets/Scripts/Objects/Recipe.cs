using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe 
{
    [SerializeField]
    private Definitions.Stuff _result;
    [SerializeField]
    private float _time;
    [SerializeField]
    private List<PrimMaterials> _parameters;
    
    public Recipe(){}

    public Recipe(Definitions.Stuff result){
        _result = result;
    }
    public Recipe(Recipe other){
        _result = other._result;
        _time = other._time;
        _parameters =  new List<PrimMaterials>(other._parameters); 
    }

    public List<PrimMaterials> getListMaterial(){
        return _parameters;
    }
    public float getTime(){
        return _time;
    }
    public Definitions.Stuff getTypeResult(){
        return _result;
    }
    public bool isComplete(){
        foreach(PrimMaterials pm in _parameters){
            if(!pm.isComplete()){
                return false;
            }
        }

        return true;
    }
    public void resetInsert(){
        foreach(PrimMaterials pm in _parameters){
            pm.resetInsert();
        } 
    }
}
