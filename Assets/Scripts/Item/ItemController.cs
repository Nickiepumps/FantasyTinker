using UnityEngine;
using PrimeTween;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemComponent[] itemComponentArr;
    [SerializeField] private ItemComponent mainComponent;
    [SerializeField] private Transform focusPosition;
    public bool assemblyStatus = true;
    private bool fixingStatus = false;
    private Vector2 currentPosition;
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        currentPosition = mainComponent.gameObject.transform.position;
        foreach (ItemComponent item in itemComponentArr)
        {
            item.mainComponent = mainComponent;
            item.itemController = this;
            item.focusPosition = focusPosition;
            item.Initialize();
        }
    }
    public void StartFocusRotate()
    {
        Tween.Rotation(mainComponent.gameObject.transform, mainComponent.gameObject.transform.localRotation, Quaternion.Euler(0f, 0f, 90f), 1f, Easing.Standard(Ease.OutCubic));
        Tween.Position(mainComponent.gameObject.transform, Vector2.zero, 1f, Easing.Standard(Ease.OutCubic));
        mainComponent.enabled = true;
    }
    public void EndFocusRotate()
    {
        Tween.Rotation(mainComponent.gameObject.transform, mainComponent.gameObject.transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 1f, Easing.Standard(Ease.OutCubic));
        Tween.Position(mainComponent.gameObject.transform, currentPosition, 1f, Easing.Standard(Ease.OutCubic));
        mainComponent.enabled = false;
    }
    public void EnableAllAssembledComponent()
    {
        foreach(ItemComponent item in itemComponentArr)
        {
            if(item.isAssembled == true)
            {
                item.enabled = true;
            }
        }
    }
    public void DisableAllComponent()
    {
        foreach (ItemComponent item in itemComponentArr)
        {
            item.enabled = false;
        }
    }
    public bool IsPartOfThisItem(GameObject clickObject)
    {
        return clickObject.transform.IsChildOf(transform);
    }
    public bool UpdateAssemblyStatus()
    {
        foreach(ItemComponent item in itemComponentArr)
        {
            if(item.isAssembled == false)
            {
                return assemblyStatus = false;
            }
        }
        return assemblyStatus = true;
    }
}
