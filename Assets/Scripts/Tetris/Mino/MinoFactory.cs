using UnityEngine;

public static class MinoFactory
{
    private static readonly Color Orange = new Color(1, 0x7f / (float)0xff, 0, 1);
    private static readonly Color Yellow = new Color(1, 1, 0, 1);
    private static readonly Color Purple = new Color(0x80 / (float)0xff, 0, 0x80 / (float)0xff, 1);

    public enum MinoType
    {
        O = 0,
        L = 1,
        J = 2,
        S = 3,
        Z = 4,
        I = 5,
        T = 6
    }

    public static IMino Create(MinoType minoType)
    {
        switch (minoType)
        {
            default:
            case MinoType.I:
                return new Mino_I(Color.cyan);
            case MinoType.J:
                return new Mino_J(Color.blue);
            case MinoType.L:
                return new Mino_L(Orange);
            case MinoType.O:
                return new Mino_O(Yellow);
            case MinoType.S:
                return new Mino_S(Color.green);
            case MinoType.T:
                return new Mino_T(Purple);
            case MinoType.Z:
                return new Mino_Z(Color.red);
        }
    }
}