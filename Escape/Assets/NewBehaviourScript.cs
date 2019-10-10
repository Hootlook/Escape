using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform floor;

    void Start()
    {
        UpdateRoom();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateRoom();
        }

    }

    private void UpdateRoom()
    {
        transform.localScale = new Vector3(Random.Range(5, 20), 1, Random.Range(5, 15));

        foreach (Transform item in transform)
        {
            if (item.tag == "Wall")
            {
                item.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(item.lossyScale.x, 2));

            }

            if (item.tag == "Floor")
            {
                item.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(item.lossyScale.x, item.lossyScale.z));

            }
        }
    }
}
