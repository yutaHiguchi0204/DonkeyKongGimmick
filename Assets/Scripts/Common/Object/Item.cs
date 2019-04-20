using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class Item : IObject
{
    protected static readonly string[] ITEM_LIST_PATH =
    {
        "Common/CommonItemList",
        "SDK1/SDK1ItemList",
        "SDK2/SDK2ItemList",
        "SDK3/SDK3ItemList"
    };

    // 回転速度
    protected const float ROTATE_TIME = 1f;

    // 点滅までの時間
    public const float DISAPPEAR_START_TIME = 2f;

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

    [SerializeField]
    protected CommonState.GameTitle _selectTitle = CommonState.GameTitle.Common;
    public CommonState.GameTitle SelectTitle
    {
        get
        {
            return _selectTitle;
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
	public bool IsRotation
	{
		get
		{
			return _isRotation;
		}
		set
		{
			_isRotation = value;
		}
	}

    [SerializeField]
    protected bool _isDisappearing = false;
	public bool IsDisappearing
	{
		get
		{
			return _isDisappearing;
		}
		set
		{
			_isDisappearing = value;
		}
	}

    private Tween _rotateTween;

	[SerializeField]
	private AwaitTriggerExtensions _awaitExt;

    // 取得時の移動先
    [SerializeField]
    private PanelRoot _onClickAfterParent;

    public override async void Initialize()
    {
		CheckTitle();
        _itemList = Resources.Load(ITEM_LIST_PATH[(int)_selectTitle], typeof(ItemList)) as ItemList;

		// IDが設定してあれば初期化
		if (_id != (int)ItemName.ItemNameList.None)
		{
			SetImage();
		}

		// 回転
		if (_isRotation)
        {
            _rotateTween = _itemImage.transform.DOLocalRotate(new Vector3(0f, 360f, 0f), ROTATE_TIME, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

		// 時間経過で消滅
        if (_isDisappearing)
        {
			await _awaitExt.CancelableAsync(WaitForDisappearStart(async () =>
			{
				await _awaitExt.CancelableAsync(Blink(() =>
				{
					Destroy(this.gameObject);
				}));
			}));
        }
    }
	
	public void Initialize(ItemName.ItemNameList id, bool isRotation = false, bool isDisappearing = false)
	{
		_id = id;
		_isRotation = isRotation;
		_isDisappearing = isDisappearing;
		Initialize();
	}

    // 画像設定
    public virtual void SetImage()
    {
        // IDがNoneなら設定しない
        if (_id == (int)ItemName.ItemNameList.None)
        {
            return;
        }

		CheckTitle();
        _itemImage.sprite = _itemList.Get()[(int)_id - CommonState.ITEM_ID_START[(int)_selectTitle]];
    }

	// ボーナス用設定
	public virtual void SetBonus(ItemName.ItemNameList id, PanelRoot panel, bool isRotation = true, bool isDisappearing = true)
	{
		ID = id;
		_isRotation = isRotation;
		_isDisappearing = isDisappearing;

		_onClickAfterParent = panel;

		Initialize();
		SetImage();
	}

    // 点滅
    public IEnumerator Blink(Action action = null)
    {
        for (int i = 0; i < CommonState.BLINK_NUM; i++)
        {
            _itemImage.enabled = false;
            yield return new WaitForSeconds(CommonState.BLINK_TIME / 2);
            _itemImage.enabled = true;
            yield return new WaitForSeconds(CommonState.BLINK_TIME / 2);
			action?.Invoke();
        }
    }

    public virtual void OnClick()
    {
        // 回転を止める
        _rotateTween.Kill();
        transform.localRotation = Quaternion.identity;

        // アイテム取得
        _onClickAfterParent.GetItem(this);

        Destroy(this.gameObject);
    }

    // 待機処理
    protected IEnumerator WaitForDisappearStart(Action action = null)
    {
        yield return new WaitForSeconds(DISAPPEAR_START_TIME);
		action?.Invoke();
    }

	// タイトルチェック
	protected CommonState.GameTitle CheckTitle()
	{
		if (_selectTitle == CommonState.GameTitle.Common && (int)_id >= CommonState.ITEM_ID_START[(int)CommonState.GameTitle.SDK1])
		{
			_selectTitle = (CommonState.GameTitle)((int)_id / CommonState.ITEM_ID_START[(int)CommonState.GameTitle.SDK1]);
		}

		return _selectTitle;
	}
}
