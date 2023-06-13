using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDetection : MonoBehaviour
{
    private bool _grabRequest;
    private bool _doRequest;
    private PlayerManager _player;
    private void Start() {
        _player = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
        _grabRequest = false;     
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            switch(_player.getState()){
                case PlayerState.State.NORMAL:
                    _grabRequest = true;
                    break;
                case PlayerState.State.GRABBING:
                    if(this.gameObject == _player.getDetection()){
                        _player.tryThrow();
                    }
                    break;
            }
        }


        if(Input.GetKeyUp(KeyCode.LeftShift)){
            _grabRequest = false;
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            _doRequest = true;
        }

        if(Input.GetKeyUp(KeyCode.Space)){
            _doRequest = false;
        }
    }
    private void OnTriggerStay(Collider collision) {
        if(this.gameObject == _player.getDetection()){
            if(_grabRequest && collision.gameObject.CompareTag("GrabObject")){
                _player.tryGrab(collision.GetComponent<Stuff>());
                _grabRequest = false;
            }else if(_doRequest && collision.gameObject.CompareTag("WorkTable")){
                _player.tryDo(collision.GetComponent<Table>());
                _doRequest = false;
            }else if(_doRequest && collision.gameObject.CompareTag("SantaSack")){
                _player.tryScore(collision.GetComponent<WishManager>());
                _doRequest = false;
            }
        }
    }
    
}
