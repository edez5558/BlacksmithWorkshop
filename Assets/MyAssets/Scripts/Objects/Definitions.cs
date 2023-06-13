using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definitions : MonoBehaviour{
    public enum Stuff{
        TEST,
        HULE,
        HILO,
        ALGODON,
        PELOTA,
        PELUCHE,
        TRUNK,
        STICK,
        IRON_RAW,
        IRON_BAR,
        IRON_SWORD,
        MODEL_SWORD,
        IRON_AXE,
        MODEL_AXE,
        GIFT
    }

    public enum Tables{
        TEST,
        SPAWNER,
        COSEDORA,
        HILADORA,
        HORNO
    }

    private Dictionary<Stuff,GameObject> _dictionary;
    private Dictionary<Stuff,Sprite> _logoDictionary;
    [SerializeField]
    private List<Recipe> _recipes;
    public static Definitions instance;

    public Recipe getRecipeByStuff(Definitions.Stuff result){
        Recipe recipe = _recipes.Find(rec => rec.getTypeResult() == result);
        if(recipe != null)
            return new Recipe(recipe); 
        
        return null;
    }

    public GameObject getPrefab(Stuff stuff){
        return _dictionary[stuff];
    }

    public Sprite getSprite(Stuff stuff){
        return _logoDictionary[stuff];
    }
    private void Awake(){
        if(instance != null){
            Destroy(this);
            return;
        }else{
            instance = this;
        }

        _dictionary = new Dictionary<Stuff, GameObject>();
        _logoDictionary = new Dictionary<Stuff, Sprite>();

        _dictionary.Add(Stuff.HILO,Resources.Load<GameObject>("Prefab/Stuff/Hilo"));
        _dictionary.Add(Stuff.HULE,Resources.Load<GameObject>("Prefab/Stuff/Hule"));
        _dictionary.Add(Stuff.ALGODON,Resources.Load<GameObject>("Prefab/Stuff/Algodon"));
        _dictionary.Add(Stuff.IRON_RAW,Resources.Load<GameObject>("Prefab/Stuff/IronRaw"));
        _dictionary.Add(Stuff.IRON_BAR,Resources.Load<GameObject>("Prefab/Stuff/IronBar"));
        _dictionary.Add(Stuff.MODEL_SWORD,Resources.Load<GameObject>("Prefab/Stuff/ModelSword"));
        _dictionary.Add(Stuff.MODEL_AXE,Resources.Load<GameObject>("Prefab/Stuff/ModelAxe"));
        _dictionary.Add(Stuff.IRON_SWORD,Resources.Load<GameObject>("Prefab/Stuff/IronSword"));
        _dictionary.Add(Stuff.IRON_AXE,Resources.Load<GameObject>("Prefab/Stuff/IronAxe"));
        _dictionary.Add(Stuff.TRUNK,Resources.Load<GameObject>("Prefab/Stuff/Trunk"));
        _dictionary.Add(Stuff.STICK,Resources.Load<GameObject>("Prefab/Stuff/Stick"));


        _logoDictionary.Add(Stuff.HILO,Resources.Load<Sprite>("Sprite/HiloItemLogo"));
        _logoDictionary.Add(Stuff.ALGODON,Resources.Load<Sprite>("Sprite/AlgodonItemLogo"));
        _logoDictionary.Add(Stuff.IRON_BAR,Resources.Load<Sprite>("Sprite/IronLogo"));
        _logoDictionary.Add(Stuff.STICK,Resources.Load<Sprite>("Sprite/StickLogo"));
        _logoDictionary.Add(Stuff.TRUNK,Resources.Load<Sprite>("Sprite/TrunkLogo"));
        _logoDictionary.Add(Stuff.IRON_SWORD,Resources.Load<Sprite>("Sprite/IronSwordLogo"));
        _logoDictionary.Add(Stuff.IRON_AXE,Resources.Load<Sprite>("Sprite/IronAxeLogo"));


    }
}