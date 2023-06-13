using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{   
    public enum State{
        NORMAL,
        GRABBING,
        FOCUS
    }

    private State _currentState;
    private Stuff _onHand;
    private Transform _grabPoint;
    private PlayerAnimation _myAnimation;
    private void Awake(){
        _myAnimation = GetComponentInChildren<PlayerAnimation>();

        if(_myAnimation == null){
            Debug.LogWarning("Animator no encontrado en el hijo");
        }
    }

    public void setState(State newState){
        _myAnimation.setState(newState);

        _currentState = newState;
    }
    private void Start() {
        setState(State.NORMAL);
        _grabPoint = transform.GetChild(1).transform;
    }
    public void grabStuff(Stuff stuff){
        if(_onHand == null){
            if(stuff.setOnGrab(_grabPoint)){
                _onHand = stuff;
                setState(State.GRABBING);
            }
        }
    }
    public void throwStuff(){
        if(_onHand == null) return;

        setState(State.NORMAL);
        _onHand.removeParent();
        _onHand = null;
    }
    private void cleanHand(){
        if(_onHand == null) return;

        _onHand.removeParent();
        _onHand.destroy();
        _onHand = null;

        if(_currentState == State.GRABBING){
            setState(State.NORMAL);
        }
    }
    public void tryInsertStuff(Table work){
        if(_onHand != null && work.isSpawner()) return;

        if(_onHand == null && !work.isSpawner()){
            work.extractStuff(_onHand,this);
            return;
        }

        if(work.insertStuff(_onHand,this)){
            cleanHand();
        }
    }

    public void tryScore(WishManager sack){
        if(_onHand == null) return;

        if(sack.insertStuff(_onHand)){
            cleanHand();
        }
    }
    
    public State getState(){
        return _currentState;
    }
   
}
