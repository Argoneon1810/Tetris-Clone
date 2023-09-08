using System.Collections;
using UnityEngine;

public class DebugMsgSender : MonoBehaviour
{
    public bool toggleBlock, fillLine, fillLineButBlock, eraseBlock;
    public CellGrid grid;
    public Vector2Int target;

    private void Update()
    {
        if(toggleBlock.Trigger())
            if (grid.IsActive(target))
                grid.DisableBlock(target);
            else
                grid.EnableBlock(target, Color.white);
        if(fillLine.Trigger())
            for(int x = 0; x < grid.Dimen.x; ++x)
                grid.EnableBlock(new Vector2Int(x, target.y), Color.white);
        if (fillLineButBlock.Trigger())
            for (int x = 0; x < grid.Dimen.x; ++x)
                if (x == target.x)
                    grid.DisableBlock(new Vector2Int(x, target.y));
                else
                    grid.EnableBlock(new Vector2Int(x, target.y), Color.white);
    }
}