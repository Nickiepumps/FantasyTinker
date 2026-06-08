using PrimeTween;
using UnityEngine;

public class ItemComponent_New : MonoBehaviour
{
    [SerializeField] private Collider2D componentCollider;
    [SerializeField] private Component componentScriptableObject;

    [SerializeField] private ComponentSlotController[] componentSlotArr;

    private Vector2 currentPlacedPosition;
    public Vector2 focusPosition;

    public void OnStartDragging()
    {
        Tween.Scale(transform, 1.3f, 0.2f, Easing.Standard(Ease.OutCubic));
    }
    public void OnDragging(Vector3 mousePos)
    {
        transform.position = mousePos;
    }
    private void OnEndDragging()
    {
        Tween.Scale(transform, 1f, 0.2f, Easing.Standard(Ease.OutCubic));
    }
    public void StartFocusRotate()
    {
        Tween.LocalRotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 90f), 1f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, new Vector3(focusPosition.x, 0f, -5f), 1f, Easing.Standard(Ease.OutCubic));
    }
    public void EndFocusRotate()
    {
        Tween.LocalRotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 0.3f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, new Vector3(focusPosition.x, 0f, -5f), currentPlacedPosition, 1f, Easing.Standard(Ease.OutCubic));
        Tween.Scale(transform, Vector3.one, 0.3f, Easing.Standard(Ease.OutCubic));
    }
}
