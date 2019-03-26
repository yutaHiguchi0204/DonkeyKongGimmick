﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private static readonly string[] ITEM_LIST_PATH = 
    {
        "Common/CommonItemList",
        "SDK1/SDK1ItemList",
        "SDK2/SDK2ItemList"
    };

    [SerializeField]
    protected int _id = 0;
    public int ID
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    public enum SelectItemList
    {
        Common,
        SDK1,
        SDK2
    }
    [SerializeField]
    protected SelectItemList _selectItemList = SelectItemList.Common;
    public SelectItemList CurrentSelectItemList
    {
        get
        {
            return _selectItemList;
        }
    }

    // アイテムリスト
    private ItemList _itemList;

    [SerializeField]
    protected SpriteRenderer _itemImage;
    public SpriteRenderer ItemImage
    {
        get
        {
            return _itemImage;
        }
    }

    // 初期化
    public virtual void Initialize()
    {
        // IDが設定されていなかったらスタート値を設定
        if (_id == 0)
        {
            _id = CommonState.ITEM_ID_START[(int)_selectItemList];
        }

        SetImage();
    }

    // 画像設定
    public virtual void SetImage()
    {
        _itemList = Resources.Load(ITEM_LIST_PATH[(int)_selectItemList], typeof(ItemList)) as ItemList;
        _itemImage.sprite = _itemList.Get()[_id - CommonState.ITEM_ID_START[(int)_selectItemList]];
    }
}
