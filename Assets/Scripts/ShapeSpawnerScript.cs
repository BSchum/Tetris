using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSpawnerScript : MonoBehaviour {
    public GameObject[] Shape;
    public Sprite[] ShapePics;
    public GameObject shapeParent;
    List<GameObject> ShapeToSpawn;
    List<int> IndexShapeToSpawn;

    int Cursor = 0;
    int AfterCursor = 1;

    GameObject gameObjectInTerrain;
    //-------Variable for SpawnPosition
    Vector2 spawnPosition = new Vector2(7f, 20f);
    Quaternion spawnRotation = new Quaternion(0, 0, 0, 0);

    public Image HoldedShapePic;
    public Image nextShapePreview;

    GameObject HoldedShape;

    bool isFirstHold;
    bool isTradable;

    float firstGrounded;
    float delayFirstGroundedForSpawn;
    // Use this for initialization
    void Start () {

        // Declaration
        IndexShapeToSpawn = new List<int>();
        ShapeToSpawn = new List<GameObject>();

        //First Shapes in List
        IndexShapeToSpawn.Add(Random.Range(0,Shape.Length));
        ShapeToSpawn.Add(Shape[IndexShapeToSpawn[0]]);

        //Fill the rest
        FillListofShapes();

        //Spawn first shape
        UpdateSpriteNextShape();
        Spawn(ShapeToSpawn[Cursor]);
        Cursor++;
        AfterCursor = Cursor + 1;


    }

    // Update is called once per frame
    void Update() {
        if (gameObjectInTerrain.GetComponent<ShapeBehaviourScript>().IsItGrounded(gameObjectInTerrain.transform) && gameObjectInTerrain.GetComponent<ShapeBehaviourScript>().isDelayedAfterGounded())
        {
            UpdateSpriteNextShape();
            Spawn(ShapeToSpawn[Cursor]);
            Cursor++;
            AfterCursor = Cursor + 1;
            isTradable = true;
        }

        if(Cursor % 100 == 0)
        {
            FillListofShapes();
        }

        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
      
            HoldAShape();
        }
        
     }

    void UpdateSpriteNextShape()
    {
        nextShapePreview.sprite = ShapePics[IndexShapeToSpawn[AfterCursor]];
    }

    void Spawn(GameObject shape)
    {
        gameObjectInTerrain = Instantiate(shape, spawnPosition, spawnRotation, shapeParent.transform);
        gameObjectInTerrain.SetActive(true);
    }
    void HoldAShape()
    {
        GameObject transitionObject;
        if(HoldedShape)// Si on tiens deja un objet
        {
            if (isTradable)
            {
                transitionObject = Instantiate(HoldedShape);// On enregistre le GO tenu dans un objet de transition
                transitionObject.SetActive(false);
                HoldedShape = Instantiate(gameObjectInTerrain);// On enregistre le GO du terrain comme objet tenu
                HoldedShape.SetActive(false);
                Destroy(gameObjectInTerrain);
                Spawn(transitionObject);// ON fait spawn l'objet qui etait tenu
                UpdateSpriteHoldedShape();
                isTradable = false;
            }
        }
        else
        {
            HoldedShape = Instantiate(gameObjectInTerrain);// On recupere le GO du terrain pour l'enregister comme " GameObject Tenu"
            Destroy(gameObjectInTerrain);// On le détruit car si il est Hold il ne doit pas etre dans le terrain
            Spawn(ShapeToSpawn[Cursor]);// On fait spawn la prochaine forme
            Cursor++;// On passe le curseur a la forme d'apres
            AfterCursor = Cursor + 1;
            UpdateSpriteNextShape();// On update le sprite de la prochaine forme qui spawnera
            UpdateSpriteHoldedShape();
            HoldedShape.SetActive(false);

        }
    }
    void UpdateSpriteHoldedShape()
    {
        if (isFirstHold)
        {
            HoldedShapePic.sprite = ShapePics[IndexShapeToSpawn[Cursor - 1]];
            isFirstHold = false;
        }
        else
        {
            HoldedShapePic.sprite = ShapePics[IndexShapeToSpawn[Cursor - 2]];
            isFirstHold = true;
        }
    }
    //Preparing data
    void FillListofShapes()
    {
        for(int i = 0; i <= 100; i++)
        {
            ShapeToSpawn.Add(RandomShape(Shape));
        }
    }
    GameObject RandomShape(GameObject[] ShapeTypes)
    {
        int Index = 0;
        do
        {
            Index = Random.Range(0, ShapeTypes.Length);
        } while (Index == IndexShapeToSpawn[IndexShapeToSpawn.Count - 1]);


        IndexShapeToSpawn.Add(Index);//On ajoute l'index de la forme ( dans le tableau de forme , donc de 0 a 6 ( 7 formes))
        return ShapeTypes[IndexShapeToSpawn[IndexShapeToSpawn.Count-1]];//On retourne le gameobject lié a la forme dont on a choisi l'index ( le dernier de la liste IndexShapeToSpawn )
    }
    //End of preparing data

}
