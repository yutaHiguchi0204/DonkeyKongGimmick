using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RouletteBarrel : MonoBehaviour
{
    [SerializeField]
    private Item _item;
    public Item BarrelItem
    {
        get
        {
            return _item;
        }
    }

    // バレルID
    private int _id = 0;

    // アイテムの種類
    private List<ItemName.ItemNameList> _itemIDList = new List<ItemName.ItemNameList>();

    // アイテムの決定
    private ReactiveProperty<ItemName.ItemNameList> _pickItemID = new ReactiveProperty<ItemName.ItemNameList>(ItemName.ItemNameList.None);
    public IReactiveProperty<ItemName.ItemNameList> PickItemID
    {
        get
        {
            return _pickItemID;
        }
    }

    // 失敗時エフェクト
    [SerializeField]
    private ParticleSystem _failedEffect;

    private IDisposable _roulettePlayable;

    public void Initialize(int id, List<ItemName.ItemNameList> itemIDList, float realInterval)
    {
        _id = id;
        _itemIDList = itemIDList;
        _item.SetImage();

        // ルーレット作動
        _roulettePlayable = Observable.Interval(TimeSpan.FromSeconds(realInterval))
            .Subscribe(time =>
            {
                _item.ID = _itemIDList[((int)time + _id) % _itemIDList.Count];
                _item.SetImage();
            });
    }

    // 失敗時
    public IEnumerator Failed()
    {
        _roulettePlayable.Dispose();

        ParticleSystem particle = Instantiate(_failedEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);

        yield return new WaitWhile(() => particle.IsAlive(true));
        Destroy(particle.gameObject);
    }

    // バレルクリック時
    public void OnClick()
    {
        // 回転停止
        _roulettePlayable.Dispose();
        _pickItemID.Value = _item.ID;
    }
}
