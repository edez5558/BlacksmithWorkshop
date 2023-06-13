using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore: MonoBehaviour
{
    private static HighScore _instance; 
    private int _highScore = 0;
    private int _currentScore = 0;
    private void Awake(){
        if(_instance == null){
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
            return;
        }
    }

    public int getHighScore(){
        return _highScore;
    }
    public int getCurrentScore(){
        return _currentScore;
    }

    public void insertScore(int score){
        if(score > _highScore){
            _highScore = score;
        }

        _currentScore = score;
    }
    
    public static HighScore getInstance(){
        return _instance;
    }
}
