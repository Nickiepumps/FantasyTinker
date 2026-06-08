using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform camPos;
    public void Initialize()
    {
        EnableRoom();
    }
    public void Deactivate()
    {
        DisableRoom();
    }
    protected virtual void EnableRoom()
    {
        //gameObject.SetActive(true);
    }
    protected virtual void DisableRoom()
    {
        //gameObject.SetActive(false);
    }
}
