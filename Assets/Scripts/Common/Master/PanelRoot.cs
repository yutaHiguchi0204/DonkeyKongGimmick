using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelRoot : IPanel
{
    protected const float MOVE_TIME = 0.5f;

    [SerializeField]
    protected Canvas _canvas;

    // 2DUI
    [SerializeField]
    protected Image _item2D;

	public override void Initialize()
	{
		this.enabled = false;
	}

	public virtual void GetItem(Item item)
    {
        // スクリーン座標変換
        Vector3 screenPos = Camera.main.WorldToScreenPoint(item.transform.position);
        screenPos.x -= Screen.width / 2;
        screenPos.y -= Screen.height / 2;
        screenPos.z = 0f;

        // 2Dアイテム生成
        Image item2D = Instantiate(_item2D, screenPos, Quaternion.identity);
        item2D.rectTransform.SetParent(_canvas.transform, false);
        item2D.sprite = item.ItemList.Get()[(int)item.ID - CommonState.ITEM_ID_START[(int)item.SelectTitle]];

        GetAnimation(item2D);
    }

    protected virtual void GetAnimation(Image item2D)
    {
        item2D.rectTransform.DOMove(transform.position, MOVE_TIME)
            .OnComplete(() =>
            {
                Destroy(item2D.gameObject);
            });
    }
}
