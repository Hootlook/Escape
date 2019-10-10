using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] snap;

    public Transform GetRandomSnap()
    {
        return snap[Random.Range(0, snap.Length)];
    }
}
