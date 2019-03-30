using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Item : MonoBehaviour
{
    private static readonly string[] ITEM_LIST_PATH = 
    {
        "Common/CommonItemList",
        "SDK1/SDK1ItemList",
        "SDK2/SDK2ItemList"
    };

    // 回転速度
    private const float ROTATE_TIME = 1f;

    [SerializeField]
    protected ItemName.ItemNameList _id = ItemName.ItemNameList.None;
    public ItemName.ItemNameList ID
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
    public ItemList ItemList
    {
        get
        {
            return _itemList;
        }
    }

    [SerializeField]
    protected SpriteRenderer _itemImage;
    public SpriteRenderer ItemImage
    {
        get
        {
            return _itemImage;
        }
    }

    // 回転させるかどうか
    [SerializeField]
    private bool _isRotation = false;

    private Tween _rotateTween;

    // 取得時の移動先
    [SerializeField]
    private PanelRoot _onClickAfterParent;

    private void Start()
    {
        _itemList = Resources.Load(ITEM_LIST_PATH[(int)_selectItemList], typeof(ItemList)) as ItemList;

        if (_isRotation)
        {
            _rotateTween = _itemImage.transform.DOLocalRotate(new Vector3(0f, 360f, 0f), ROTATE_TIME, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        // IDが設定してあれば初期化
        if (_id != (int)ItemName.ItemNameList.None)
        {
            Initialize();
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
        // IDがNoneなら設定しない
        if (_id == (int)ItemName.ItemNameList.None)
        {
            return;
        }

        _itemImage.sprite = _itemList.Get()[(int)_id - CommonState.ITEM_ID_START[(int)_selectItemList]];
    }

    // アイテム取得

    public virtual void OnClick()
    {
        // 回転を止める
        _rotateTween.Kill();
        transform.localRotation = Quaternion.identity;

        // アイテム取得
        _onClickAfterParent.GetItem(this);

        Destroy(this.gameObject);
    }
}
