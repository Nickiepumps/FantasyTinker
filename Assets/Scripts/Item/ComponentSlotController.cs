using UnityEngine;

public class ComponentSlotController : MonoBehaviour
{
    public Component componentRequirement;
    private ComponentType componentType => componentRequirement.componentType;
    private ItemController_New itemController;
    [SerializeField] private Transform attachTransform;

    [SerializeField] private Screw[] screwArr;
    [SerializeField] private float screwDuration;

    [SerializeField] private ComponentSlotController[] aboveComponentArr;
    [SerializeField] private ComponentSlotController belowComponent;

    [SerializeField] private ItemComponent_New currentAttachedComponent;
    public bool isAssembled = true;
    private Vector3 attachPosition => attachTransform.position;
    private int screwAmount => screwArr.Length;

    public void Initialize(ItemController_New itemController)
    {
        this.itemController = itemController;
        foreach (Screw screw in screwArr)
        {
            screw.Initialize(screwDuration, this);
            screw.AddScrewListener(UpdateFullyAssembleStatus);
        }
    }
    public bool IsReadyToDisassemble()
    {
        for (int i = 0; i < screwArr.Length; i++)
        {
            if (screwArr[i].screwStatus == ScrewStatus.Screwed)
            {
                return false;
            }
        }
        for (int i = 0; i < aboveComponentArr.Length; i++)
        {
            if (aboveComponentArr[i].isAssembled == true || IsComponentScrewed(aboveComponentArr[i].screwArr) == true)
            {
                return false;
            }
        }
        return true;
    }
    private bool IsComponentScrewed(Screw[] componentScrewArr)
    {
        for (int i = 0; i < componentScrewArr.Length; i++)
        {
            if (componentScrewArr[i].screwStatus == ScrewStatus.Screwed)
            {
                return true;
            }
        }
        return false;
    }
    private void UpdateFullyAssembleStatus(Screw screw = null)
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
    public void AddToScrewContainer(Screw unscrewedTarget)
    {
        itemController.AddUnscrewedScrew(unscrewedTarget);
    }
    public void GetScrew(Screw unscrewedTarget)
    {
        itemController.GetScrew(unscrewedTarget);
    }
    public void AttachComponent(ItemComponent_New component)
    {
        if (componentRequirement != component.componentScriptableObject) return;

        component.transform.position = attachPosition;
        component.transform.parent = attachTransform;
        currentAttachedComponent = component;
        if(screwAmount == 0)
        {
            isAssembled = true;
        }
    }
    public void DetachComponent()
    {
        if (belowComponent.isAssembled == false) return;

        currentAttachedComponent = null;
        isAssembled = false;
    }
    public bool IsComponentAttachedToSlot()
    {
        if (currentAttachedComponent != null)
        {
            return true;
        }
        return false;
    }
}
