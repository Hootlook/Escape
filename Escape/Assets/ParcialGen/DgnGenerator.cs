using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rooms;

[Serializable]
public class Rooms
{
    public enum RoomType
    {
        Start,
        End,
        Common,
        Connector,
        DeadEnd,
        TreasureRoom
    }

    public GameObject[] startRoom;
    public GameObject[] endRoom;
    public GameObject[] commonRoom;
    public GameObject[] connector;
    public GameObject[] deadEnd;
    public GameObject[] treasureRoom;

    public GameObject GetRoomType(int roomType, int index = 0)
    {
        switch (roomType)
        {
            case 0:
                return startRoom[index];
            case 1:
                return endRoom[index];
            case 2:
                return commonRoom[index];
            case 3:
                return connector[index];
            case 4:
                return deadEnd[index];
            case 5:
                return treasureRoom[index];
            default:
                return null;
        }
    }
}
public class DgnGenerator : MonoBehaviour
{
    public Rooms[] rooms;

    Room previousRoom;
    Room currentRoom;
    Room CurrentRoom
    {

        get
        {
            return currentRoom;
        }
        set
        {
            previousRoom = currentRoom;
            currentRoom = value;
        }
    }
    int i = 0;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateNewDungeon();
        }
    }

    private void GenerateNewDungeon()
    {
        foreach (Transform item in transform) Destroy(item.gameObject);

        previousRoom = Instantiate(rooms[0].GetRoomType((int)RoomType.Start), transform).GetComponent<Room>();

        currentRoom = previousRoom;

        CreateRoomsRecursively();
    }

    private void CreateRoomsRecursively()
    {
        i++;

        for (int door = 0; door < previousRoom.snap.Length; door++)
        {
            CurrentRoom = Instantiate(rooms[0].GetRoomType((int)RoomType.Connector), transform).GetComponent<Room>();

            SnapRooms(CurrentRoom, previousRoom);

        }

        if (i >= 2) return;

        CreateRoomsRecursively();
    }

    void SnapRooms(Room room1, Room room2)
    {
        Transform snap1 = room1.GetRandomSnap();
        Transform snap2 = room2.GetRandomSnap();

        room1.transform.eulerAngles = room2.transform.TransformDirection(snap2.eulerAngles) - snap1.localEulerAngles;

        Vector3 distance = snap1.position - snap2.position;

        room1.transform.position = room2.transform.position - distance;
    }
}
