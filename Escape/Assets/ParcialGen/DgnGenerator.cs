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
    int roomIteration = 0;

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
        roomIteration++;

        foreach (var door in previousRoom.snap)
        {
            CurrentRoom = Instantiate(rooms[0].GetRoomType((int)RoomType.Common), transform).GetComponent<Room>();

            SnapRooms(CurrentRoom, previousRoom);
        }

        if (roomIteration >= 10) return;

        CreateRoomsRecursively();
    }

    void SnapRooms(Room room1, Room room2)
    {
        Transform snap1 = room1.GetRandomSnap();
        Transform snap2 = room2.GetRandomSnap();

        room1.transform.position =  room2.transform.position;

        room1.transform.position = room2.transform.position - snap2.position;

        room1.transform.position = room2.transform.position - snap1.position;

        room1.transform.RotateAround(snap1.position, snap1.up, snap2.eulerAngles.y - 180 - snap1.eulerAngles.y);

        int x;

        Int32.TryParse(snap1.name, out x);

        room1.snap.RemoveAt(x);

        //Destroy(room1.transform.Find(snap1.name).gameObject);
    }
}
