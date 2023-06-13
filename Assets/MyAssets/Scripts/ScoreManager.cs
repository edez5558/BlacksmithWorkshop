using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    
    [SerializeField]
    private int _score; 
    [SerializeField]
    private int _initTime;
    [SerializeField]
    private float _remainingTime;
    [SerializeField]
    private Scene_Manager _sceneManager;
    private int _lastInt;
    private WishManager _sackSanta;
    private TextMeshProUGUI _textTime;

    private void Awake() {
        _sackSanta = GetComponent<WishManager>();

        if(_sackSanta == null){
            Debug.LogWarning("EL score manager no esta posicionado en una table");
        }else{
            _sackSanta.link(this);
        }

        _textTime = GameObject.FindGameObjectWithTag("MainUI").transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _remainingTime = _initTime;
    }
    public bool isEnd(){
        return _remainingTime <= 0;
    }

    private void Start() {
    }

    private void Update() {
        if(_remainingTime > 0.0f){
            _remainingTime -= Time.deltaTime; 
            if(_lastInt != (int)_remainingTime){
                _textTime.text = string.Format("{0,1}:{1,2}",((int)_remainingTime/60),
                                            ((int)_remainingTime%60).ToString("D2"));
                _lastInt = (int)_remainingTime;
            }
        }else{
            HighScore.getInstance().insertScore(_score);
            _sceneManager.LoadScore();
        }
    }

    public void aumentScore(int score){
        if(!isEnd())
            _score += score;
    }


}
