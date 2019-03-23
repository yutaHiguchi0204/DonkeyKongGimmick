using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private static readonly string[] ITEM_LIST_PATH = 
    {
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
        SDK1,
        SDK2
    }
    [SerializeField]
    protected SelectItemList _selectItemList = SelectItemList.SDK1;
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
    protected MeshRenderer _itemImage;
    public MeshRenderer ItemImage
    {
        get
        {
            return _itemImage;
        }
    }

    // 初期化
    public virtual void Initialize()
    {
        SetImage();
    }

    // 画像設定
    public virtual void SetImage()
    {
        _itemList = Resources.Load(ITEM_LIST_PATH[(int)_selectItemList], typeof(ItemList)) as ItemList;
        _itemImage.material = _itemList.Get()[_id - CommonState.ITEM_ID_START[(int)_selectItemList]];
    }
}
