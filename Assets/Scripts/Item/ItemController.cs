using UnityEngine;
using PrimeTween;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemComponent[] itemComponentArr;
    public bool assemblyStatus = true;
    private bool fixingStatus = false;
    private Vector2 currentPosition;
    private void Start()
    {
        currentPosition = transform.position;
    }
    public void StartFocusRotate()
    {
        Tween.Rotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 90f), 1f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, Vector2.zero, 1f, Easing.Standard(Ease.OutCubic));
        foreach(ItemComponent item in itemComponentArr)
        {
            item.Initialize();
        }
    }
    public void EndFocusRotate()
    {
        Tween.Rotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 1f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, currentPosition, 1f, Easing.Standard(Ease.OutCubic));
    }
    public bool IsPartOfThisItem(GameObject clickObject)
    {
        return clickObject.transform.IsChildOf(transform);
    }
}
