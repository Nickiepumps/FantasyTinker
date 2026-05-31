using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;
public class DragAndDropInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 currentMouseWorldPosition => Camera.main.ScreenToWorldPoint(playerInput.Player.MousePosition.ReadValue<Vector2>());

    private bool isDrag = false;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.DragComponent.performed += OnDragging;
        playerInput.Player.DragComponent.canceled += OnEndDragging;
    }
    private void OnDisable()
    {
        playerInput.Player.DragComponent.performed -= OnDragging;
        playerInput.Player.DragComponent.canceled -= OnEndDragging;
        playerInput.Player.Disable();
    }
    private void OnDragging(InputAction.CallbackContext context)
    {
        Collider2D hit = Physics2D.OverlapPoint(currentMouseWorldPosition);
        if(hit != null && hit.gameObject == gameObject)
        {
            Tween.Scale(transform, 1.3f, 0.2f, Easing.Standard(Ease.OutCubic));
            isDrag = true;
        }
    }
    private void OnEndDragging(InputAction.CallbackContext context)
    {
        if(isDrag == true)
        {
            isDrag = false;
            Tween.Scale(transform, 1f, 0.2f, Easing.Standard(Ease.OutCubic));
        }
    }
    private void Update()
    {
        if(isDrag == true)
        {
            transform.position = currentMouseWorldPosition;
        }
        else
        {
            return;
        }
    }
}
