using PrimeTween;
using UnityEngine;

public class ItemComponent_New : MonoBehaviour
{
    private ItemController_New itemController;
    [SerializeField] private Collider2D componentCollider;
    public Component componentScriptableObject;
    public ItemComponent_New anchoredComponent;

    private Vector2 currentPlacedPosition;
    public Vector2 focusPosition;

    public void Initialize(ItemComponent_New anchoredComponent, ItemController_New itemController)
    {
        this.anchoredComponent = anchoredComponent;
        this.itemController = itemController;
    }
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
