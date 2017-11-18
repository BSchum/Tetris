using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {
    const int gridWidth = 10;
    const int gridHeight = 22;
    public static Transform[,] Shapes = new Transform[gridWidth, gridHeight];
    public GameObject shapesParent;
    public Text UIScore;
    int score;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        VerifyLines(Shapes);
        UpdateScore(score, UIScore);
	}
    /// <summary>
    /// Verify lines in 2D Array Shapes, if a line ( 10 cube ) is full of cube,
    /// register his index in a List<Int>, and call a function to destroy this line
    /// </summary>
    /// <param name="Shapes"> Array with all position of cube in the game</param>
    void VerifyLines(Transform[,] Shapes)
    {
        List<int> indexesToDelete = new List<int>();
        for(int y = 0; y < gridHeight; ++y)
        {
            int cubeNumber = 0;
            for (int x = 0; x < gridWidth; ++x)
            {
                if (Shapes[x,y] != null)
                {
                    cubeNumber++;
                }
            }
            if(cubeNumber == 10)
            {
                indexesToDelete.Add(y);
            }
        }
        if(indexesToDelete.Count > 0)
        {
            DestroyLines(indexesToDelete);

        }
    }
    /// <summary>
    /// Destroy all transform in the Array Shapes from the indexes.
    /// </summary>
    /// <param name="indexes">Indexes of line to destroy</param>
    void DestroyLines(List<int> indexes)
    {
        for(int y = 0; y <= indexes.Count-1; y++)
        {
            score += 100;

            for (int x = 0; x < gridWidth; x++)
            {
                Destroy(Shapes[x, indexes[y]].gameObject);
            }
        }
        GetAllCubeDownAfterDestroyLine(indexes.Count, indexes[indexes.Count-1]);
    }
    /// <summary>
    /// Replace all cube after destroying a lines.
    /// </summary>
    /// <param name="numberOfLineDestroyed"></param>
    /// <param name="maxIndex">Higher line in index list</param>
    void GetAllCubeDownAfterDestroyLine(int numberOfLineDestroyed, int maxIndex)
    {
        for(int y = maxIndex; y<gridHeight;y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if (Shapes[x, y] != null)
                {
                    Shapes[x, y].position += new Vector3(0, (-numberOfLineDestroyed));
                    Shapes[x, y - numberOfLineDestroyed] = Shapes[x, y];
                    Shapes[x, y] = null;


                }
            }
        }
    }

    void UpdateScore(int score, Text textToUpdate)
    {
        textToUpdate.text = score.ToString();
    }
}
