using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class BonusFactory : MonoBehaviour
{
    // 終了
    protected ReactiveProperty<bool> _isFinished = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsFinished
    {
        get
        {
            return _isFinished;
        }
    }

    // 結果
    protected ReactiveProperty<bool> _isCorrected = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsCorrected
    {
        get
        {
            return _isCorrected;
        }
    }
}
