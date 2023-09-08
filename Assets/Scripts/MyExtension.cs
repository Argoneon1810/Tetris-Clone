using System.Collections;
using UnityEngine;

public static class MyExtension
{
    public static bool Trigger(this ref bool toggle)
    {
        if (toggle)
        {
            toggle = false;
            return true;
        }
        return false;
    }
    
    public static IMino.Rotation GetLeft(this IMino.Rotation current)
    {
        switch (current)
        {
            default:
            case IMino.Rotation.B:
                return IMino.Rotation.L;
            case IMino.Rotation.L:
                return IMino.Rotation.T;
            case IMino.Rotation.T:
                return IMino.Rotation.R;
            case IMino.Rotation.R:
                return IMino.Rotation.B;
        }
    }

    public static IMino.Rotation GetRight(this IMino.Rotation current)
    {
        switch (current)
        {
            default:
            case IMino.Rotation.B:
                return IMino.Rotation.R;
            case IMino.Rotation.R:
                return IMino.Rotation.T;
            case IMino.Rotation.T:
                return IMino.Rotation.L;
            case IMino.Rotation.L:
                return IMino.Rotation.B;
        }
    }
}