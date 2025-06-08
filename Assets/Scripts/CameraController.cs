using UnityEngine;
using Enums;

public class CameraController : MonoBehaviour
{
    public GameObject currentRoom;
    public float cameraMoveSpeed;
    public GameObject gameManager;
    public Vector2 currentRoomPosition = new Vector2(0, 0);

    private DungeonCreator _dc;
    public float _cameraTime = 0.0f;
    private Vector3 currentCamPos;
    private Vector3 targetCamPos;
    

    void Start()
    {
        _dc = gameManager.GetComponent<DungeonCreator>();
        currentCamPos = new Vector3(0, 0, -10);
        targetCamPos = new Vector3(0, 0, -10);
    }

    void Update()
    {
        _cameraTime += Time.deltaTime * cameraMoveSpeed;
        _cameraTime = Mathf.Clamp(_cameraTime, 0.0f, 3.0f);
        gameObject.transform.position = Vector3.Lerp(currentCamPos, targetCamPos, Mathf.Sin(_cameraTime - (Mathf.PI / 2)) * 0.5f + 0.5f);
    }

    public void ShiftCamera(Vector2 nextRoom)
    {
        print("moving to room " + nextRoom);
        
        GameObject targetRoom = _dc.DungeonMap[nextRoom];
        
        currentCamPos = new Vector3(transform.position.x, transform.position.y, -10);
        targetCamPos = new Vector3(targetRoom.transform.position.x, targetRoom.transform.position.y, -10);
        _cameraTime = 0;
        
        currentRoom = _dc.DungeonMap[nextRoom];
        
    }
}
