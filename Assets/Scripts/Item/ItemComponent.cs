using PrimeTween;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField] private Collider2D componentCollider;
    public Transform focusPosition;
    public Transform parentComponent;
    public ItemComponent mainComponent;
    public ItemController itemController;

    [SerializeField] private Screw[] screwArr;
    private int screwAmount => screwArr.Length;
    [SerializeField] private Transform componentPos;
    [SerializeField] private float screwDuration;

    [SerializeField] private Component componentScriptableObject;
    [SerializeField] private ItemComponent[] aboveComponentArr;
    [SerializeField] private ItemComponent belowComponent;

    public bool isAssembled = true;

    private Vector2 currentPlacedPosition;
    public void Initialize()
    {
        if(screwAmount != 0)
        {
            foreach(Screw screw in screwArr)
            {
                screw.Initialize(1f, this);
                screw.AddScrewListener(UpdateFullyAssembleStatus);
            }
        }
    }
    public void StartFocusRotate()
    {
        Tween.LocalRotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 90f), 1f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, new Vector3(focusPosition.position.x, 0f, -5f), 1f, Easing.Standard(Ease.OutCubic));
    }
    public void EndFocusRotate()
    {
        Tween.LocalRotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 0.3f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, new Vector3(focusPosition.position.x, 0f, -5f), currentPlacedPosition, 1f, Easing.Standard(Ease.OutCubic));
        Tween.Scale(transform, Vector3.one, 0.3f, Easing.Standard(Ease.OutCubic));
    }
    public void Assemble(Vector3 mousePos)
    {
        componentCollider.enabled = true;

        if (componentPos == null) return;

        float length = Vector2.Distance(mousePos, componentPos.position);
        if (length <= 1f && mainComponent.enabled == true)
        {
            if (IsReadyToAssemble() == true)
            {
                if(screwAmount == 0)
                {
                    isAssembled = true;
                }
                AttachToMainComponent();
                transform.position = componentPos.position;
                OnEndDragging();
            }
        }
        else
        {
            OnEndDragging();
            currentPlacedPosition = transform.position;
        }
    }
    public void Disassemble()
    {
        isAssembled = false;
        itemController.assemblyStatus = false;
        OnStartDragging();
        componentCollider.enabled = false;
        DetachFromMainComponent();
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
    public bool IsReadyToDisassemble()
    {
        for(int i = 0; i < screwArr.Length; i++)
        {
            if (screwArr[i].screwStatus == ScrewStatus.Screwed)
            {
                return false;
            }
        }
        for (int i = 0; i < aboveComponentArr.Length; i++)
        {
            if (aboveComponentArr[i].isAssembled == true || IsComponentScrewed(aboveComponentArr[i].screwArr) == true || aboveComponentArr[i].transform.parent == aboveComponentArr[i].parentComponent)
            {
                return false;
            }
        }
        return true;
    }
    private bool IsComponentScrewed(Screw[] componentScrewArr)
    {
        for(int i = 0; i < componentScrewArr.Length; i++)
        {
            if (componentScrewArr[i].screwStatus == ScrewStatus.Screwed)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsReadyToAssemble()
    {
        if(belowComponent.isAssembled == true)
        {
            return true;
        }
        return false;
    }
    private void UpdateFullyAssembleStatus(Screw unscrewedTarget = null)
    {
        for (int i = 0; i < screwArr.Length; i++)
        {
            if (screwArr[i].screwStatus == ScrewStatus.Unscrewed)
            {
                isAssembled = false;
                return;
            }
        }
        isAssembled = true;
    }
    private void DetachFromMainComponent()
    {
        this.enabled = false;
        transform.parent = itemController.transform;
    }
    private void AttachToMainComponent()
    {
        this.enabled = true;
        transform.parent = parentComponent.transform;
        Tween.LocalRotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 0.3f, Easing.Standard(Ease.OutCubic));
        Tween.Position(transform, componentPos.position, 0.3f, Easing.Standard(Ease.OutCubic));
    }
    public bool IsAttachedToMainComponent()
    {
        if(transform.parent == parentComponent.transform)
        {
            return true;
        }
        return false;
    }
}
