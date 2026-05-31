using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum InputState
{
    NonFocus,
    Focus
}

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController instance;

    private PlayerInput playerInput;
    private InputState currentInputstate;
    private Ray currentMouseToRay => Camera.main.ScreenPointToRay(playerInput.Player.MousePosition.ReadValue<Vector2>());
    private Vector2 currentMouseWorldPosition => Camera.main.ScreenToWorldPoint(playerInput.Player.MousePosition.ReadValue<Vector2>());
    private Vector2 currentMouseScroll => playerInput.Player.CameraZoom.ReadValue<Vector2>();

    private ItemController currentItem;
    private bool isDragging = false;
    private ItemComponent dragItemTarget;

    [SerializeField] private Button buttonExitFocus;
    private void Awake()
    {
        playerInput = new PlayerInput();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ChangeInputState(InputState.NonFocus);
        buttonExitFocus.onClick.AddListener(ExitFocus);
    }
    private void Update()
    {
        if(isDragging == true)
        {
            dragItemTarget.OnDragging(currentMouseWorldPosition);
        }
        if(currentMouseScroll.y != 0)
        {
            // Scale object up/down based on mouse scroll
        }
    }
    private void ExitFocus()
    {
        if(currentItem != null)
        {
            currentItem.EndFocusRotate();
            currentItem = null;
            ChangeInputState(InputState.NonFocus);
            buttonExitFocus.gameObject.SetActive(false);
        }
    }
    private void OnEndClicking(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(currentMouseToRay);
        if (hit.collider != null)
        {
            currentItem = hit.collider.GetComponentInParent<ItemController>();
            if (currentItem != null && currentItem.assemblyStatus == true)
            {
                currentItem.StartFocusRotate();
                buttonExitFocus.gameObject.SetActive(true);
                ChangeInputState(InputState.Focus);
            }
        }
    }
    private void OnDragPerform(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(currentMouseToRay);
        if (hit.collider != null && hit.collider.TryGetComponent<ItemComponent>(out ItemComponent item))
        {
            if(item.IsReadyToDisassemble() == true)
            {
                dragItemTarget = item;
                item.Disassemble();
                isDragging = true;
            }
        }
    }
    private void OnCancelDrag(InputAction.CallbackContext context)
    {
        if (isDragging == false) return;
        isDragging = false;
        dragItemTarget.Assemble(currentMouseWorldPosition);
        dragItemTarget = null;
    }
    private void OnHoldPerform(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(currentMouseToRay);
        if (hit.collider != null && hit.collider.TryGetComponent<Screw>(out Screw screw))
        {
            if(screw.screwStatus == ScrewStatus.Unscrewed)
            {
                screw.StartScrewPerform(false);
            }
            else if(screw.screwStatus == ScrewStatus.Screwed)
            {
                screw.StartScrewPerform(true);
            }
        }
    }
    private void OnCancelHold(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(currentMouseToRay);
        if (hit.collider != null && hit.collider.TryGetComponent<Screw>(out Screw screw))
        {
            if(screw.screwStatus == ScrewStatus.ScrewTransition)
            {
                screw.PauseScrew();
            }
        }
    }
    private void InputInitialize(InputState state)
    {
        playerInput.Enable();
        if(state == InputState.NonFocus)
        {
            playerInput.Player.DragComponent.canceled += OnEndClicking;
        }
        else if(state == InputState.Focus)
        {
            playerInput.Player.DragComponent.performed += OnDragPerform;
            playerInput.Player.DragComponent.canceled += OnCancelDrag;
            playerInput.Player.Unscrew.performed += OnHoldPerform;
            playerInput.Player.Unscrew.canceled += OnCancelHold;
        }
    }
    private void DisableInput(InputState state)
    {
        if (state == InputState.NonFocus)
        {
            playerInput.Player.DragComponent.canceled -= OnEndClicking;
        }
        else if (state == InputState.Focus)
        {
            playerInput.Player.DragComponent.performed -= OnDragPerform;
            playerInput.Player.DragComponent.canceled -= OnCancelDrag;
            playerInput.Player.Unscrew.performed -= OnHoldPerform;
            playerInput.Player.Unscrew.canceled -= OnCancelHold;
        }
        playerInput.Disable();
    }
    public void ChangeInputState(InputState newState)
    {
        DisableInput(currentInputstate);
        currentInputstate = newState;
        InputInitialize(currentInputstate);
    }
}
