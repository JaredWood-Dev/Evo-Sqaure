using UnityEngine;
using Enums;
using Unity.VisualScripting;

public class RoomComponent : MonoBehaviour
{
    [Header("Path Openings")] 
    public bool north;
    public bool east;
    public bool south;
    public bool west;
    
    [Header("Paths")]
    public GameObject northPath;
    public GameObject eastPath;
    public GameObject southPath;
    public GameObject westPath;
    
    [Header("Doors")]
    public GameObject northDoor;
    public GameObject eastDoor;
    public GameObject southDoor;
    public GameObject westDoor;

    [Header("Dungeon Map")] 
    public Vector2 roomLocation;

    public void SetPath(Direction direction, bool open)
    {
        switch (direction)
        {
            case Direction.North:
                northDoor.SetActive(!open);
                northPath.SetActive(open);
                break;
            case Direction.East:
                eastDoor.SetActive(!open);
                eastPath.SetActive(open);
                break;
            case Direction.South:
                southDoor.SetActive(!open);
                southPath.SetActive(open);
                break;
            case Direction.West:
                westDoor.SetActive(!open);
                westPath.SetActive(open);
                break;
        }
    }

    void Start()
    {
        SetPath(Direction.North, north);
        SetPath(Direction.East, east);
        SetPath(Direction.South, south);
        SetPath(Direction.West, west);
    }
}
