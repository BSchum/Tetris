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
        if (gameObjectInTerrain.GetComponent<ShapeBehaviourScript>().IsItGrounded(gameObjectInTerrain.transform))
        {
            UpdateSpriteNextShape();
            Spawn(ShapeToSpawn[Cursor]);
            Cursor++;
            AfterCursor = Cursor + 1;
        }

        if(Cursor % 100 == 0)
        {
            FillListofShapes();
        }

        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            Debug.Log(HoldedShape);
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
    }
    void HoldAShape()
    {
        GameObject transitionObject;
        if(HoldedShape)// Si on tiens deja un objet
        {
            transitionObject = HoldedShape;// On enregistre le GO tenu dans un objet de transition
            HoldedShape = gameObjectInTerrain;// On enregistre le GO du terrain comme objet tenu
            Spawn(transitionObject);// ON fait spawn l'objet qui etait tenu
        }
        else
        {
            HoldedShape = gameObjectInTerrain;// On recupere le GO du terrain pour l'enregister comme " GameObject Tenu"
            Destroy(gameObjectInTerrain);// On le détruit car si il est Hold il ne doit pas etre dans le terrain
            Spawn(ShapeToSpawn[Cursor]);// On fait spawn la prochaine forme
            Cursor++;// On passe le curseur a la forme d'apres
            AfterCursor = Cursor + 1;
            UpdateSpriteNextShape();// On update le sprite de la prochaine forme qui spawnera
            Debug.Log(HoldedShape);
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
