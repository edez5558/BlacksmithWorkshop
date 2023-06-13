using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Table : MonoBehaviour
{
    
    #region Variables Recetas
    [SerializeField]
    private Definitions.Tables _type;
    [SerializeField]
    private List<Definitions.Stuff> _allowStuff;
    [SerializeField]
    private List<Definitions.Stuff> _allRecipe;
    private List<Recipe> _listRecipe;
    private Stack<Definitions.Stuff> _insertedStuff;
    #endregion

    #region Variables Comportamiento
    [SerializeField]
    private bool _isFocus;
    [SerializeField]
    private bool _isSpawn;
    private bool _isWorking;
    [SerializeField]
    private bool _acceptAll;
    #endregion
    private Definitions.Stuff _resultStuff;
    #region Progreso Time
    [SerializeField]
    private float _currentTime;
    private float _totalTime;
    private Image _barraTime; 
    private GameObject _objectBarra;

    #endregion
    private PlayerState _playerWorking;
    private Transform _spawnPoint;
    private void Awake() {
        _insertedStuff = new Stack<Definitions.Stuff>();
    }
    
    private void Start() {
        _isWorking = false;
        _spawnPoint = transform.GetChild(1).transform;
        _objectBarra = transform.GetChild(2).gameObject;
        _barraTime = _objectBarra.transform.GetChild(1).GetComponent<Image>();
        _objectBarra.SetActive(false);

        _listRecipe = new List<Recipe>();
        foreach(Definitions.Stuff stuff in _allRecipe){
            Recipe recipe = Definitions.instance.getRecipeByStuff(stuff);

            if(recipe != null){
                _listRecipe.Add(recipe);
            }else if(_isSpawn){
                _listRecipe.Add(new Recipe(stuff));
            }else{
                Debug.LogWarningFormat("La receta {0} no ha sido especificada en Definitions",stuff);
            }
        }
    }

    public bool getFocus(){
        return _isFocus;
    }
    public bool isSpawner(){
        return _isSpawn;
    }
    private bool checkStuff(Stuff stuff){
        return _allowStuff.Contains(stuff.getStuffType());
    }

    private bool tryRecipe(Recipe recipe,Definitions.Stuff stuff,int offset){
        foreach(PrimMaterials mat in recipe.getListMaterial()){
            if(mat.stuff == stuff){
                mat.sumCurrentInsert(offset);  
                return true;
            }
        }
        return false;
    }

    private void resetRecipe(){
        foreach(Recipe re in _listRecipe){
            re.resetInsert();
        }
    }
    private void finishWorking(){
        GameObject prefab = Definitions.instance.getPrefab(_resultStuff);
        prefab =  Instantiate(prefab,_spawnPoint.position,prefab.transform.rotation);
        if(_playerWorking != null){
            _playerWorking.setState(PlayerState.State.NORMAL);
            _playerWorking.grabStuff(prefab.GetComponent<Stuff>());
            _playerWorking = null;
        }
    }

    private void Update() {
        if(_isWorking){
            _currentTime += Time.deltaTime;
            _barraTime.fillAmount = _currentTime/_totalTime;
            if(_currentTime >= _totalTime){
                _isWorking = false;
                _objectBarra.SetActive(false);
                finishWorking();
            }
        }
    }
    
    private void setRecipeWork(Recipe recipe,PlayerState player){
        if(_isFocus){
            player.setState(PlayerState.State.FOCUS);
            _playerWorking = player;
        }else{
            player.setState(PlayerState.State.NORMAL);
        }
        _resultStuff = recipe.getTypeResult();
        _objectBarra.SetActive(true);
        _currentTime = 0.0f;
        _totalTime = recipe.getTime();
        _isWorking = true;
    }
    public bool extractStuff(Stuff stuff,PlayerState player){
        if(!_isSpawn && !_isWorking && _insertedStuff.Count > 0){
            Definitions.Stuff typeStuff = _insertedStuff.Pop();

            foreach(Recipe recipe in _listRecipe){
                tryRecipe(recipe,typeStuff,-1);
            }

            GameObject prefab = Definitions.instance.getPrefab(typeStuff);
            prefab = Instantiate(prefab,_spawnPoint.position,prefab.transform.rotation);

            player.grabStuff(prefab.GetComponent<Stuff>());
            player.setState(PlayerState.State.GRABBING);

            return true;
        }

        return false; 
    }

    public bool insertStuff(Stuff stuff,PlayerState player){
        bool useStuff = false;

        if(!_isSpawn && (_acceptAll || checkStuff(stuff)) && !_isWorking){
            Definitions.Stuff typeStuff = stuff.getStuffType();
            foreach(Recipe recipe in _listRecipe){
                if(tryRecipe(recipe,typeStuff,1)){
                    if(recipe.isComplete()){
                        resetRecipe();
                        setRecipeWork(recipe,player);
                        _insertedStuff.Clear();
                        return true;
                    }
                    useStuff = true;
                }
            }
        }else if(_isSpawn && _listRecipe.Count == 1){
            setRecipeWork(_listRecipe[0],player);
            return false;
        }

        if(useStuff){
            _insertedStuff.Push(stuff.getStuffType());
        }

        return useStuff;
    }

}
