[System.Serializable]
public class BoolGrid
{
    public int x;
    public int y;
    public BoolRow[] rows;

    public bool this[int i, int j]
    {
        get => rows[i].row[j];
        set => rows[i].row[j] = value;
    }

    public BoolGrid(int width, int height)
    {
        x = width;
        y = height;
        rows = new BoolRow[y];
        for (int i = 0; i < y; i++)
            rows[i] = new BoolRow(x);
    }

    public BoolGrid(BoolGrid source, int newWidth, int newHeght) : this(newWidth, newHeght)
    {
        if (source == null)
            return;

        int xOffset = (newWidth - source.x) / 2;
        int yOffset = (newHeght - source.y) / 2;

        for (int i = yOffset; i < source.y; i++)
        {
            if (i < 0 || i >= newHeght) continue;

            for (int j = xOffset; j < source.x; j++)
            {
                if (j < 0 || j >= newWidth) continue;

                this[i, j] = source[i, j];
            }
        }
    }

    [System.Serializable]
    public struct BoolRow
    {
        public bool[] row;

        public BoolRow(int width)
        {
            row = new bool[width];
        }
    }
}
