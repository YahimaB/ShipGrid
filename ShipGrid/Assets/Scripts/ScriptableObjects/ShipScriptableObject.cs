using UnityEngine;
using Unity.Mathematics;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/ShipScriptableObject", order = 1)]
public class ShipScriptableObject : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int Width => gridSize.x;
    public int Height => gridSize.y;
    public bool this[int i, int j] => grid[i, j];

    [SerializeField]
    private int2 gridSize;
    [SerializeField]
    private BoolGrid grid;

    private bool wasModified = false;

    private void OnValidate()
    {
        wasModified = true;

        if (grid == null || grid.x != Width || grid.y != Height)
            OnSizeChanged();
    }

    private async void OnSizeChanged()
    {
        wasModified = false;
        await Task.Delay(5000);

        if (!wasModified && gridSize.x != 0 && gridSize.y != 0)
            grid = new BoolGrid(grid, gridSize.x, gridSize.y);
    }
}