using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    
    private void Start() {
        TextMeshProUGUI txtCurrent = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtHigh = transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        txtHigh.text = string.Format("{0,5}",HighScore.getInstance().getHighScore().ToString("D5"));
        txtCurrent.text = string.Format("{0,5}",HighScore.getInstance().getCurrentScore().ToString("D5"));
    }
}
