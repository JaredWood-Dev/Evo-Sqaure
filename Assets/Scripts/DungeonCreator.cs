using System.Collections.Generic;
using Enums;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    
    public GameObject[] dungeonRooms;

    public float roomBuffer;
    public int roomCount;
    public int maxRooms;

    public Dictionary<Vector2, GameObject> DungeonMap = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        GameObject startRoom = Instantiate(dungeonRooms[1], new Vector3(0, 0, 25), Quaternion.identity);
        startRoom.GetComponent<RoomComponent>().east = true;
        startRoom.GetComponent<RoomComponent>().roomLocation = new Vector2(0, 0);
        DungeonMap.Add(new Vector2(0,0), startRoom);
        
        Camera.main.GetComponent<CameraController>().currentRoom = DungeonMap[new Vector2(0,0)];

        /*
        for (int i = 1; i < 5; i++)
        {
            GameObject newRoom = Instantiate(dungeonRooms[0], new Vector3(roomBuffer * i, 0, 25), Quaternion.identity);
            newRoom.GetComponent<RoomComponent>().east = true;
            newRoom.GetComponent<RoomComponent>().west = true;
            newRoom.GetComponent<RoomComponent>().roomLocation = new Vector2(i, 0);
            DungeonMap.Add(new Vector2(i,0), newRoom);
        }
        */
        
        InitalizeRoom(Direction.East, new Vector2(1, 0));
    }

    bool InitalizeRoom(Direction inComingDirection, Vector2 roomLocation)
    {
        roomCount++;
        
        //Create and Add the Room to the Map
        GameObject newRoom = Instantiate(dungeonRooms[0], new Vector3(roomLocation.x * roomBuffer, roomLocation.y * roomBuffer, 25), Quaternion.identity);
        RoomComponent newRoomComponent = newRoom.GetComponent<RoomComponent>();
        newRoomComponent.roomLocation = roomLocation;
        DungeonMap.Add(roomLocation, newRoom);
        
        //Initalize opposite path
        switch (inComingDirection)
        {
            case Direction.North:
                newRoomComponent.south = true;
                break;
            case Direction.South:
                newRoomComponent.north = true;
                break;
            case Direction.East:
                newRoomComponent.west = true;
                break;
            case Direction.West:
                newRoomComponent.east = true;
                break;
        }
        
        //1/2 chance to enable/disable paths
        int directionChoice = Random.Range(1, 4);
        if (directionChoice == 1 && !newRoomComponent.north && roomCount < maxRooms)
        {
            newRoomComponent.north = true;
            InitalizeRoom(Direction.North, roomLocation + new Vector2(0, 1));
        }
        if (directionChoice == 2 && !newRoomComponent.east && roomCount < maxRooms)
        {
            newRoomComponent.east = true;
            InitalizeRoom(Direction.East, roomLocation + new Vector2(1, 0));
        }
        if (directionChoice == 3 && !newRoomComponent.south && roomCount < maxRooms)
        {
            newRoomComponent.south = true;
            InitalizeRoom(Direction.South, roomLocation + new Vector2(0, -1));
        }
        if (directionChoice == 4 && !newRoomComponent.west && roomCount < maxRooms)
        {
            newRoomComponent.west = true;
            InitalizeRoom(Direction.West, roomLocation + new Vector2(-1, 0));
        }
        
        return true;
    }
}
