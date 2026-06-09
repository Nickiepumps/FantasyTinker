using System.Collections.Generic;
using UnityEngine;

public class ItemController_New : MonoBehaviour
{
    [SerializeField] private List<ItemComponent_New> itemComponentList;
    [SerializeField] private List<ComponentSlotController> componentSlotList;
    [SerializeField] private ItemComponent_New anchoredComponent;
    [SerializeField] private Transform focusPosition;
    private List<Screw> unscrewedList = new List<Screw>();
    public bool assemblyStatus = true;
    private bool fixingStatus = false;
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        foreach(ItemComponent_New item in itemComponentList)
        {
            item.Initialize(anchoredComponent, this);
        }
        foreach(ComponentSlotController slot in componentSlotList)
        {
            slot.Initialize(this);
        }
    }
    public void AddUnscrewedScrew(Screw unscrewedTarget)
    {
        unscrewedList.Add(unscrewedTarget);
    }
    public void GetScrew(Screw unscrewedTarget)
    {
        if (unscrewedList.Count == 0) return;
        for(int i = 0; i< unscrewedList.Count; i++)
        {
            if (unscrewedList[i] == unscrewedTarget)
            {
                unscrewedList.Remove(unscrewedList[i]);
                return;
            }
        }
    }
}
