using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null) instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return instance;
        }
    }

    public Sprite cat;
    public Sprite chicken;
    public Sprite dog;
    public Sprite lion;
    public Sprite penguin;

    public MeshFilter catBody;
    public MeshFilter catHead;
    public MeshFilter catLegBL;
    public MeshFilter catLegBR;
    public MeshFilter catLegFL;
    public MeshFilter catLegFR;
    //public MeshFilter catTail;

    public MeshFilter chickenBody;
    public MeshFilter chickenHead;
    public MeshFilter chickenLegL;
    public MeshFilter chickenLegR;

    public MeshFilter dogBody;
    public MeshFilter dogHead;
    public MeshFilter dogLegBL;
    public MeshFilter dogLegBR;
    public MeshFilter dogLegFL;
    public MeshFilter dogLegFR;
    public MeshFilter dogTail;

    public MeshFilter lionBody;
    public MeshFilter lionHead;
    public MeshFilter lionLegBL;
    public MeshFilter lionLegBR;
    public MeshFilter lionLegFL;
    public MeshFilter lionLegFR;
    public MeshFilter lionTail;

    public MeshFilter penguinBody;
    public MeshFilter penguinWingL;
    public MeshFilter penguinWingR;

    public RuntimeAnimatorController animatorChicken;
    public RuntimeAnimatorController animatorCat;
    public RuntimeAnimatorController animatorDog;
    public RuntimeAnimatorController animatorLion;
    public RuntimeAnimatorController animatorPenguin;
}
