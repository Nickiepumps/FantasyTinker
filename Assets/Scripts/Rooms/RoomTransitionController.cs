using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
public class RoomTransitionController : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private RoomController[] roomControllerArr;
    private int currentRoomIndex = 0;
    private int maxRoomCount => roomControllerArr.Length;

    [SerializeField] private Button buttonNextRoom;
    [SerializeField] private Button buttonPreviousRoom;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        cam = Camera.main;
        buttonNextRoom.onClick.AddListener(NextRoom);
        buttonPreviousRoom.onClick.AddListener(PreviousRoom);
    }
    private void NextRoom()
    {
        if(currentRoomIndex < maxRoomCount - 1)
        {
            roomControllerArr[currentRoomIndex].Deactivate();
            currentRoomIndex++;
            roomControllerArr[currentRoomIndex].Initialize();
            Tween.LocalPosition(cam.transform, roomControllerArr[currentRoomIndex].camPos.position, 0.5f, Easing.Standard(Ease.OutCubic));
        }
    }
    private void PreviousRoom()
    {
        if (currentRoomIndex > 0)
        {
            roomControllerArr[currentRoomIndex].Deactivate();
            currentRoomIndex--;
            roomControllerArr[currentRoomIndex].Initialize();
            Tween.LocalPosition(cam.transform, roomControllerArr[currentRoomIndex].camPos.position, 0.5f, Easing.Standard(Ease.OutCubic));
        }
    }
}
