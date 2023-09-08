using UnityEngine;

public class WallKick_Other
{

    protected static readonly Vector2Int[][][] srsOffsets = {
        new [] {                    //from이 B일 때
            null,                       //to가 B일 때 (invalid)
            new [] {                    //to가 L일 때
                Vector2Int.zero,
                Vector2Int.right,
                new Vector2Int(1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(1, -2)
            },
            null,                       //to가 T일 때 (180도 회전 미지원)
            new [] {                    //to가 R일 때
                Vector2Int.zero,
                Vector2Int.left,
                new Vector2Int(-1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(-1, -2)
            }
        },
        new [] {                    //from이 L일 때
            new [] {                    //to가 B일 때
                Vector2Int.zero,
                Vector2Int.left,
                new Vector2Int(-1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 2)
            },
            null,                       //to가 L일 때 (invalid)
            new [] {                    //to가 T일 때
                Vector2Int.zero,
                Vector2Int.left,
                new Vector2Int(-1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 2)
            },
            null                        //to가 R일 때 (180도 회전 미지원)
        },
        new [] {                    //from이 T일 때
            null,                       //to가 B일 때 (180도 회전 미지원)
            new [] {                    //to가 L일 때
                Vector2Int.zero,
                Vector2Int.right,
                new Vector2Int(1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(1, -2)
            },
            null,                       //to가 T일 때 (invalid)
            new [] {                    //to가 R일 때
                Vector2Int.zero,
                Vector2Int.left,
                new Vector2Int(-1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(-1, -2)
            }
        },
        new [] {                    //from이 R일 때
            new [] {                    //to가 B일 때
                Vector2Int.zero,
                Vector2Int.right,
                new Vector2Int(1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2)
            },
            null,                       //to가 L일 때 (180도 회전 미지원)
            new [] {                    //to가 T일 때
                Vector2Int.zero,
                Vector2Int.right,
                new Vector2Int(1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2)
            },
            null                        //to가 R일 때 (invalid)
        },
    };
}