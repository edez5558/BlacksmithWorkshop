using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _playerAnimator;

    private void Awake() {
        _playerAnimator = GetComponentInChildren<Animator>();

        if(_playerAnimator==null){
            Debug.LogWarning("El primer hijo del jugador no tiene animator");
        }else{
            _playerAnimator.SetFloat("Speed",0.0f);
        }
    }

    public void SetSpeed(float speed){
        _playerAnimator.SetFloat("Speed",speed);
    }
    public void setState(PlayerState.State state){
        int _state = 0;

        switch(state){
            case PlayerState.State.NORMAL:
                _state = 0;
                break;
            case PlayerState.State.GRABBING:
                _state = 1;
                break;
            case PlayerState.State.FOCUS:
                _state = 2;
                break;
        }

        _playerAnimator.SetInteger("State",_state);
    }
}
