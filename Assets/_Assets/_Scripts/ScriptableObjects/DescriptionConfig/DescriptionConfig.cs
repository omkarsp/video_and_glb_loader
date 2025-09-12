using UnityEngine;

[CreateAssetMenu(fileName = "DescriptionConfig", menuName = "Description/DescriptionConfig")]
public class DescriptionConfig : ScriptableObject
{
    public string title = "item";
    public Transform location;
    public string description;
    public Sprite subjectImage;
}
