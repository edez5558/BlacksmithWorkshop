using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    [SerializeField]
    private Definitions.Stuff _type;
    private Renderer rend;
    private Quaternion _initRot;
    private float _initY;
    private bool _onGrab;

    private void Awake(){
        rend = GetComponent<Renderer>();
        enableRotate();
        _initRot = transform.rotation;
        _initY = transform.position.y;
    }

    private void enableRotate(){
        rend.material.SetFloat("_isRotate",1.0f);
    }

    private void disableRotate(){
        rend.material.SetFloat("_isRotate",0.0f);
    }
    public void destroy(){
        Destroy(this.gameObject);
    }

    public bool setOnGrab(Transform grabPoint){
        if(_onGrab) return false;

        transform.SetParent(grabPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x,0,transform.localEulerAngles.z);
        _onGrab = true;
        disableRotate();

        return true;
    }
    public Definitions.Stuff getStuffType(){
        return _type;
    }

    public void removeParent(){
        transform.parent = null;
        transform.rotation = _initRot;
        transform.position = new Vector3(transform.position.x,_initY,transform.position.z);
        _onGrab = false;
        enableRotate();
    }
}
