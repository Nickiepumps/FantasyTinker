using PrimeTween;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField] private Screw[] screwArr;
    private int screwAmount => screwArr.Length;
    [SerializeField] private Transform componentPos;
    [SerializeField] private float screwDuration;

    [SerializeField] private ItemComponent[] aboveComponentArr;
    [SerializeField] private ItemComponent belowComponent;

    public bool isAssembled = true;
    public void Initialize()
    {
        if(screwAmount != 0)
        {
            foreach(Screw screw in screwArr)
            {
                screw.Initialize(1f);
            }
        }
    }
    public void StartFocusRotate()
    {
        Tween.Rotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 90f), 0.3f, Easing.Standard(Ease.OutCubic));
    }
    public void EndFocusRotate()
    {
        Tween.Rotation(transform, transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 0.3f, Easing.Standard(Ease.OutCubic));
    }
    public void Assemble(Vector3 mousePos)
    {
        float length = Vector3.Distance(mousePos, componentPos.position);
        if (length <= 1f)
        {
            if (screwAmount != 0 && IsFullyAssemble() == true)
            {
                isAssembled = true;
                transform.position = componentPos.position;
                OnEndDragging();
            }
            if (screwAmount == 0)
            {
                isAssembled = true;
                transform.position = componentPos.position;
                OnEndDragging();
                Debug.Log($"{gameObject.name} has been Assembled");
            }
        }
    }
    public void Disassemble()
    {
        isAssembled = false;
        OnStartDragging();
        Debug.Log($"{gameObject.name} start disassemble");
    }
    private void OnStartDragging()
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
            if (aboveComponentArr[i].isAssembled == true)
            {
                return false;
            }
        }

        return true;
    }
    private bool IsFullyAssemble()
    {
        for (int i = 0; i < screwArr.Length; i++)
        {
            if (screwArr[i].screwStatus == ScrewStatus.Unscrewed)
            {
                return false;
            }
        }
        return true;
    }
}
