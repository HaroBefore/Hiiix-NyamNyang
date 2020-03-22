using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType {
    Meat,
    Powder,
    Sauce,
}

public class IngredientManager : MonoBehaviour {

    public static IngredientManager instance;

    public Dictionary<int, Ingredient> meatDic;
    public Dictionary<int, Ingredient> powderDic;
    public Dictionary<int, Ingredient> sauceDic;



    void Awake() {
        if (!instance) instance = this;
        LoadIngredientPrefab();
    }

    void Start() {
        // 초기값 세팅.
        if (PlayerPrefs.GetInt("NyamNyangIngre") != 1049) {
            meatDic[401].IsAvailable = true;
            meatDic[402].IsAvailable = true;
            meatDic[403].IsAvailable = true;
            meatDic[404].IsAvailable = true;
            powderDic[501].IsAvailable = true;
            powderDic[502].IsAvailable = true;
            powderDic[503].IsAvailable = true;
            sauceDic[601].IsAvailable = true;
            sauceDic[602].IsAvailable = true;
            sauceDic[603].IsAvailable = true;
            PlayerPrefs.SetInt("NyamNyangIngre", 1049);
        }
        else {
            foreach (int i in meatDic.Keys) meatDic[i].SetData();
            foreach (int i in powderDic.Keys) powderDic[i].SetData();
            foreach (int i in sauceDic.Keys) sauceDic[i].SetData();
        }
    }

    private void LoadIngredientPrefab() {

        meatDic = new Dictionary<int, Ingredient>();
        powderDic = new Dictionary<int, Ingredient>();
        sauceDic = new Dictionary<int, Ingredient>();

        //GameObject ingParent = new GameObject("IngredientPrefabs");

        //GameObject[] objects = Resources.LoadAll<GameObject>("Prefabs/Ingredient/") as GameObject[];
        //Ingredient[] objs = new Ingredient[objects.Length];
        //for(int i = 0; i < objects.Length; i++) {
        //    GameObject o = Instantiate(objects[i]);
        //    o.transform.parent = ingParent.transform;
        //    objs[i] = objects[i].GetComponent<Ingredient>();
        //}

        //ingParent.transform.localScale = Vector3.zero;

        Ingredient[] objs = Resources.LoadAll<Ingredient>("Prefabs/Ingredient/") as Ingredient[];

        for (int i = 0; i < objs.Length; i++) {
            switch (objs[i].type) {
                case IngredientType.Meat:
                    meatDic.Add(objs[i].index, objs[i]);
                    break;
                case IngredientType.Powder:
                    powderDic.Add(objs[i].index, objs[i]);
                    break;
                case IngredientType.Sauce:
                    sauceDic.Add(objs[i].index, objs[i]);
                    break;
            }
        }
    }
    

}