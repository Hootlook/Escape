using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int row = 10;
    public int col = 10;

    public int order = 0;

    [SerializeField]
    Transform floor = null;
    [SerializeField]
    Transform wall;

    Transform currentObject;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(UnityEngine.Random.Range(1, 3));
            UpdateRoom();
        }
    }

    private void UpdateRoom()
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                if (order == 1)
                {
                    Instantiate(floor, transform).position -= Vector3.forward * r - Vector3.right * c;

                }
                if (UnityEngine.Random.Range(1, 5) == 1)
                {

                    /*
                    switch (order)
                    {
                        case 0:
                            currentObject = Instantiate(wall, transform);
                            currentObject.position -= Vector3.right;
                            currentObject.rotation = Quaternion.Euler(Vector3.up * 90);
                            break;

                        case 1:
                            if (r == 0)
                            {
                                currentObject = Instantiate(wall, transform);
                                currentObject.position += Vector3.forward;
                                currentObject.rotation = Quaternion.Euler(Vector3.up * 180);
                            }
                            if (r == row)
                            {
                                currentObject = Instantiate(wall, transform);
                                currentObject.position -= Vector3.forward;
                            }
                            break;

                        case 2:
                            currentObject = Instantiate(wall, transform);
                            currentObject.position += Vector3.right;
                            currentObject.rotation = Quaternion.Euler(Vector3.up * -90);
                            break;
                    }

                    currentObject = null;
                    */
                    order++;
                }
            }
            order = 0;
        }
    }
}
