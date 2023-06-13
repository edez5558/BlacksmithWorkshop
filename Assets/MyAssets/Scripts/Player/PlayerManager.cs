using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    

    [SerializeField]
    private List<GameObject> _players;
    private GameObject _currentGrab;
    private GameObject _currentPlayer;
    private PlayerState _playerState;
    private PlayerAnimation _playerAnimation;
    private bool _isPoint;
    private int _currentIndex;
    private void Awake() {
        if(_players.Count == 0){
            Debug.LogWarning("Lista de jugadores no especificado");
        }else{
            _currentIndex = 0;
            updateCurrent();
        }
    }

    private void updateCurrent(){
        if(_currentPlayer != null){
            setAnimationFloat(0.0f);
        }

        _currentPlayer = _players[_currentIndex];
        _playerState = _currentPlayer.GetComponent<PlayerState>();
        _playerAnimation = _currentPlayer.GetComponent<PlayerAnimation>();
        _currentGrab = _currentPlayer.transform.GetChild(0).gameObject;
    }

    public void switchPlayer(bool right){
        if(right){
            _currentIndex = (_currentIndex+1)%_players.Count;
        }else{
            _currentIndex = ((--_currentIndex) < 0? _players.Count - 1 : _currentIndex);
        }
        updateCurrent();
    }

    public void tryGrab(Stuff stuff){
        _playerState.grabStuff(stuff);
    }

    public void tryThrow(){
        _playerState.throwStuff();
    }

    public void tryDo(Table work){
        _playerState.tryInsertStuff(work);
    }
    public void tryScore(WishManager sack){
        _playerState.tryScore(sack);
    }

    public GameObject getPlayer(){
        return _currentPlayer;
    }
    
    public PlayerState.State getState(){
        return _playerState.getState();
    }

    public void setState(PlayerState.State state){
        _playerState.setState(state);
    }

    public GameObject getDetection(){
        return _currentGrab;
    }
    
    public void setAnimationFloat(float speed){
        _playerAnimation.SetSpeed(speed);
    }
}
