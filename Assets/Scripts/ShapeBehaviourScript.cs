using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBehaviourScript : MonoBehaviour {


    const float constFallingSpeed = 0.5f;
    float fallingSpeed;
    float lastFallingMovement;

    static int numberOfShapes;
    public int shapeId;
    GameObject preview;
    public bool isGrounded = false;
	// Use this for initialization
	void Start () {
        lastFallingMovement = 0;
        fallingSpeed = constFallingSpeed;
        numberOfShapes++;
        shapeId = numberOfShapes;
        preview = Instantiate(this.gameObject,new Vector3(1000,1000),new Quaternion(0,0,0,0));        
        preview.GetComponent<ShapeBehaviourScript>().enabled = false;
        preview.transform.position = this.transform.position;
        SpriteRenderer[] cubes = preview.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer cube in cubes)
        {
            cube.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    // Update is called once per frame
    void Update () {
        if (!IsItGrounded(this.transform))
        {
            MoveHorizontally();
            Fall();
            RotateShape();
            PreviewPosition();
            InstantMoveToPreview();
        }
        else
        {
            RegisterShapeInGrid(this.transform);
            Destroy(preview);
        }
	}
    void InstantMoveToPreview()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            if(Input.GetAxisRaw("Vertical") < 0)
            {
                this.transform.position = preview.transform.position;
            }
        }
    }
    void PreviewPosition()
    {
        preview.GetComponent<ShapeBehaviourScript>().enabled = false;
        preview.transform.position = this.transform.position;
        while (!IsItGrounded(preview.transform))
        {
            preview.transform.position += new Vector3(0, -1);
        }
        preview.transform.rotation = this.transform.rotation;

    }
    void MoveHorizontally()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") > 0 && VerifiyWallRight(this.transform))
            {
                this.transform.position += new Vector3(1, 0);
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && VerifiyWallLeft(this.transform))
            {
                this.transform.position += new Vector3(- 1, 0);
            }
        }
    }
    bool VerifiyWallRight(Transform shape)
    {
        foreach(Transform cube in shape)
        {
            if(cube.position.x == 10 
                || (GameScript.Shapes[(int)Mathf.Round(cube.position.x),(int)Mathf.Round(cube.position.y)] != null 
                && GameScript.Shapes[(int)Mathf.Round(cube.position.x), (int)Mathf.Round(cube.position.y)].GetComponentInParent<ShapeBehaviourScript>().shapeId != this.shapeId))
            {
                return false;
            }
        }
        return true;
    }

    bool VerifiyWallLeft(Transform shape)
    {
        foreach (Transform cube in shape)
        {
            // If i'm the far i can at left OR there is a cube at my left and it's not mine.
            if (Mathf.Round(cube.position.x) == 1 
                ||( GameScript.Shapes[(int)Mathf.Round(cube.position.x - 2), (int)Mathf.Round(cube.position.y)] != null 
                && GameScript.Shapes[(int)Mathf.Round(cube.position.x - 2), (int)Mathf.Round(cube.position.y)].GetComponentInParent<ShapeBehaviourScript>().shapeId != this.shapeId))
            {
                return false;
            }
        }
        return true;
    }

    void Fall()
    {
        float nextFallTime = lastFallingMovement + fallingSpeed;
        if(Time.time > nextFallTime)
        {
            this.transform.position += new Vector3(0,- 1);
            lastFallingMovement = Time.time;
        }
    }
    
    public bool IsItGrounded(Transform shape)
    {
        int i = 0;
        foreach(Transform cube in shape)
        {
            i++; 
            if (Mathf.Round(cube.position.y) == 1 || GameScript.Shapes[(int)Mathf.Round(cube.position.x-1), (int)Mathf.Round(cube.position.y - 1)] != null )
            {
                isGrounded = true;
                return true;
            }
        }
        return false;
    }

    void RotateShape()
    {
        if (Input.GetButtonDown("Jump"))
        {
            this.transform.Rotate(0, 0, -90.000000f);
            int i = 1;
            if(this.gameObject.name.Contains("I"))
            {
                i++;
            }
            if(this.transform.position.x == 1)
            {
                this.transform.position += new Vector3(i, 0);
            }
            if (this.transform.position.x == 10)
            {
                this.transform.position += new Vector3(-i, 0);
            }
        }
    }

    void RegisterShapeInGrid(Transform shape)
    {
        foreach(Transform cube in shape)
        {
            GameScript.Shapes[(int)Mathf.Round(cube.position.x-1), (int)Mathf.Round(cube.position.y)] = cube;  
        }
        this.enabled = false;
    }
    
}
