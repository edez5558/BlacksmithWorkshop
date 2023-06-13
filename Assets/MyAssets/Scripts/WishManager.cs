using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WishManager : MonoBehaviour
{
    #region  Definicion
    [System.Serializable]
    class Wishes{
        [SerializeField]
        Definitions.Stuff _gift;
        [SerializeField]
        int _score;
        public Definitions.Stuff getStuffType(){
            return _gift;
        }
        public int getScore(){
            return _score;
        }
    }

    [System.Serializable]
    class WishUI{
        private GameObject _wishui;
        private Image _wishImage;


        public WishUI(Definitions.Stuff type,GameObject wishui,int precio){
            _wishui = wishui;

            Image wishImg = wishui.transform.GetChild(0).GetComponent<Image>();
            Sprite sprite = Definitions.instance.getSprite(type);

            if(sprite != null){
                wishImg.sprite = sprite; 
            }else{
                Debug.LogWarningFormat("No se definio el sprite de {0} en Definitions",type);
            }
            
            TextMeshProUGUI txt_precio = wishui.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            txt_precio.text = string.Format("${0}",precio);
        }

        public void destroyUI(){
            Destroy(_wishui); 
        }
    }

    #endregion

    [SerializeField]
    private List<Definitions.Stuff> _currentWishes;
    private List<WishUI> _UIList;
    [SerializeField]
    private List<Definitions.Stuff> _queueWishes;
    [SerializeField]
    private List<Wishes> _giftScore;
    [SerializeField]
    private float _intervalTime;
    [SerializeField]
    private int _minRequest;
    [SerializeField]
    private int _maxRequest;
    private GameObject _UIParent;
    private GameObject _UIPrefab;
    private ScoreManager _score;
    private void Awake() {
        _UIParent = GameObject.FindGameObjectWithTag("MainUI").transform.GetChild(1).gameObject;     

        if(_UIParent == null){
            Debug.LogWarning("UIWishList no ha sido encontrada");
        }

        _UIPrefab = Resources.Load<GameObject>("Prefab/UI/Wish");

        if(_UIPrefab == null){
            Debug.LogWarning("Prefab de Wish no ha sido encontrado");
        }

        _UIList = new List<WishUI>();
    }

    public void link(ScoreManager scoreM){
        _score = scoreM;
    }
    IEnumerator pushWish(){
        while(!_score.isEnd() && _queueWishes.Count != 0){
            if(_currentWishes.Count < _maxRequest){
                pushRandomRequest();
            }
            yield return new WaitForSeconds(_intervalTime);
        }

        yield return null;
    }
    private int getScoreOfList(Definitions.Stuff type){
        Wishes wish = _giftScore.Find(w => w.getStuffType() == type);

        if(wish != null){
            return wish.getScore();
        }else{
            Debug.LogWarningFormat("Tipo {0} no esta en la lista de score",type);
            return 0;
        }

    }
    private void Start() {
        StartCoroutine(pushWish());
    }
    private void pushRandomRequest(){
        int index = Random.Range(0,_queueWishes.Count - 1);
        
        Definitions.Stuff new_type;
        if(_currentWishes.Count != 0 && _currentWishes[_currentWishes.Count - 1] == _queueWishes[index]){
            new_type = _queueWishes[(index + 1) % _queueWishes.Count];
        }else{
            new_type = _queueWishes[index];
        }

        GameObject wishUi = Instantiate(_UIPrefab);
        wishUi.transform.SetParent(_UIParent.transform);

        _currentWishes.Add(new_type);
        _UIList.Add(new WishUI(new_type,wishUi,getScoreOfList(new_type)));
    }
    private void checkMin(){
        if(_currentWishes.Count <= _minRequest){
            pushRandomRequest(); 
        }
    }
    public bool insertStuff(Stuff stuff){
        if(_currentWishes.Contains(stuff.getStuffType())){
            int index = _currentWishes.FindIndex( c => c == stuff.getStuffType());
            checkMin();

            _currentWishes.RemoveAt(index);
            _UIList[index].destroyUI();
            _UIList.RemoveAt(index);


            _score.aumentScore(getScoreOfList(stuff.getStuffType()));

            return true;
        }
        return false;
    }

}
