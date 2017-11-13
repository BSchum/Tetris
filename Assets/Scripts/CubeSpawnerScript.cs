using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeSpawnerScript : MonoBehaviour {
     //Public
     public GameObject[] Shapes;
     public GameObject shapesParent;
     public Sprite[] PicsShapes;

     //Private

     //-------Variable for SpawnPosition
     Vector2 spawnPosition = new Vector2(7f, 20f);
     Quaternion spawnRotation = new Quaternion(0, 0, 0, 0);
     //-------Variable for Last shape spawned
     int lastShapeSpawnedIndex;
     GameObject lastShapeSpawned;
     //-------Variable for Next Shape
     bool nextShapeChoosed;
     GameObject nextShape;
     public Image nextShapePreview;
     int nextShapeIndex;
     //-------Variable for Actual Shape
     int actualShapeIndex;
     //-------Variable for Holding Shape
     GameObject registeredShape;
     public Image HoldedShapePic;

     // Use this for initialization
     void Start() {
         nextShapeIndex = 0;
         Spawn(ChooseShape(Shapes));
         actualShapeIndex = lastShapeSpawnedIndex;


     }

     // Update is called once per frame
     void Update() {
         if(nextShapeChoosed == false)
         {
             nextShape = ChooseShape(Shapes);
             nextShapePreview.sprite = PicsShapes[nextShapeIndex];
             nextShapeChoosed = true;
         }
         //HoldAShape();
         if (lastShapeSpawned.GetComponent<ShapeBehaviourScript>().enabled == false)
         {
             Spawn(nextShape);
             nextShapeChoosed = false;
             actualShapeIndex = nextShapeIndex;
         }

     }
     void HoldAShape()
     {
         if (Input.GetButtonDown("Vertical"))
         {
             if (Input.GetAxisRaw("Vertical") > 0)
             {

                 HoldedShapePic.sprite = PicsShapes[actualShapeIndex];
                 Destroy(lastShapeSpawned);

             }
         }
     }
     GameObject ChooseShape(GameObject[] Shapes)
     {
         nextShapeIndex = 0;
         do
         {
             nextShapeIndex = Random.Range(0, Shapes.Length - 1);
         } while (lastShapeSpawnedIndex == nextShapeIndex);
         lastShapeSpawnedIndex = nextShapeIndex;
         return Shapes[nextShapeIndex];
     }

     void Spawn(GameObject Shape)
     { 
         lastShapeSpawned = Instantiate(Shape, spawnPosition, spawnRotation,shapesParent.transform);
     }

}
