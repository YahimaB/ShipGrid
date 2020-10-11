using UnityEngine;
using Unity.Mathematics;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Module", menuName = "ScriptableObjects/ModuleScriptableObject", order = 1)]
public class ModuleScriptableObject : ScriptableObject
{
    public string Name;
    public int2 Size;
    public Sprite Icon;
}
