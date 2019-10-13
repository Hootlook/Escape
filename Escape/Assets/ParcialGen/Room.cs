using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Transform> snap;

    public Transform GetRandomSnap()
    {
        int index = Random.Range(0, snap.Count);
        snap[index].name = index.ToString();
        return snap[index];
    }
}
