using UnityEngine;
using UnityEngine.UI;
public enum ComponentType
{
    Unknown,
    EnergyShard,
    Grip,
}
public enum ComponentConditionType
{
    None,
    Normal,
    Broken
}
[CreateAssetMenu(fileName = "NewComponent", menuName = "Add Component")]
public class Component : ScriptableObject
{
    [Header("Component Detail")]
    public string componentName;
    public ComponentType componentType;
    [TextArea] public string componentDescription;
    public Sprite componentImage;

    [Header("Component Properties")]
    public ComponentConditionType componentCondition;
    public int screwRequirementAmount;
}
