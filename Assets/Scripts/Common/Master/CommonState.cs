using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonState
{
    // キー関係
    public const string MOVE_LEFT_KEY = "a";
    public const string MOVE_RIGHT_KEY = "d";
    public const string JUMP_KEY = "w";

    // 点滅エフェクト
    public const int BLINK_NUM = 3;
    public const float BLINK_TIME = 0.3f;

    // アイテム関係
    public static readonly int[] ITEM_ID_START =
    {
        1,
        100,
        200
    };
}
