using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrewInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 currentMouseWorldPosition => Camera.main.ScreenToWorldPoint(playerInput.Player.MousePosition.ReadValue<Vector2>());

    [SerializeField] private float screwDuration = 1f;
    private float currentScrewDuration;
    private bool isScrew = false;
    private bool isCompleted = false;
    private Tween screwRotationTween;
    private Tween screwScaleTween;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Unscrew.performed += OnStartScrewAction;
        playerInput.Player.Unscrew.canceled += OnCancelScrewAction;
    }
    private void OnDisable()
    {
        playerInput.Player.Unscrew.performed -= OnStartScrewAction;
        playerInput.Player.Unscrew.canceled -= OnCancelScrewAction;
        playerInput.Player.Disable();
    }
    private void OnStartScrewAction(InputAction.CallbackContext context)
    {
        Collider2D hit = Physics2D.OverlapPoint(currentMouseWorldPosition);
        if (hit != null && hit.gameObject == gameObject && isCompleted == false)
        {
            isScrew = true;
            screwRotationTween = Tween.EulerAngles(transform, new Vector3(0f, 0f, 360f), Vector3.zero, 0.8f, Easing.Standard(Ease.Linear), -1);
            screwScaleTween = Tween.Scale(transform, new Vector3(1.3f, 1.3f, 1.3f), screwDuration, Easing.Standard(Ease.Linear));
            screwRotationTween.isPaused = false;
        }
    }
    private void OnCompletedScrewAction()
    {
        Debug.Log("Finished Screw");
        isCompleted = true;
        gameObject.SetActive(false);
    }
    private void OnCancelScrewAction(InputAction.CallbackContext context)
    {
        if (isScrew == true && isCompleted == false)
        {
            Debug.Log("End Hold");
            screwRotationTween.isPaused = true;
            isScrew = false;
        }
    }
    private void Update()
    {
        if(isCompleted == false && isScrew == true)
        {
            currentScrewDuration += Time.deltaTime;
            if (currentScrewDuration >= screwDuration)
            {
                OnCompletedScrewAction();
            }
        }
    }
}
