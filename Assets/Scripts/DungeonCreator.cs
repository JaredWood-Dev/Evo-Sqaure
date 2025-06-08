using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    
    public GameObject[] dungeonRooms;

    public float roomBuffer;

    public Dictionary<Vector2, GameObject> DungeonMap = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        GameObject startRoom = Instantiate(dungeonRooms[1], new Vector3(0, 0, 25), Quaternion.identity);
        startRoom.GetComponent<RoomComponent>().east = true;
        startRoom.GetComponent<RoomComponent>().roomLocation = new Vector2(0, 0);
        DungeonMap.Add(new Vector2(0,0), startRoom);
        
        Camera.main.GetComponent<CameraController>().currentRoom = DungeonMap[new Vector2(0,0)];

        for (int i = 1; i < 5; i++)
        {
            GameObject newRoom = Instantiate(dungeonRooms[0], new Vector3(roomBuffer * i, 0, 25), Quaternion.identity);
            newRoom.GetComponent<RoomComponent>().east = true;
            newRoom.GetComponent<RoomComponent>().west = true;
            newRoom.GetComponent<RoomComponent>().roomLocation = new Vector2(i, 0);
            DungeonMap.Add(new Vector2(i,0), newRoom);
        }
        
        print(DungeonMap);
    }
}
