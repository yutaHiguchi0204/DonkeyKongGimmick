using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class KongPanelRoot : PanelRoot
{
    private const int PANEL_NUM = 4;
    private const float ACTIVE_TIME = 3f;

    // 配置座標
    [SerializeField]
    private float _posXMin = -60f;
    [SerializeField]
    private float _posXMax = 60f;

    private float _hidePosY = 320f;
    private float _openPosY = 240f;

    // 配置スケール
    [SerializeField]
    private float _afterScale = 32f;

    // パネルアイテム2D
    private Image[] _panelItem = new Image[PANEL_NUM];

    // アイテムID
    private ItemName.ItemNameList _id;

    // パネルの先頭ID
    private ItemName.ItemNameList _startID = ItemName.ItemNameList.PanelK;

    // 1up用パネル
    [SerializeField]
    private PlayerLife _lifePanel;

    public override void GetItem(Item item)
    {
        _id = item.ID;

        base.GetItem(item);
    }

    protected override void GetAnimation(Image item2D)
    {
        _panelItem[_id - _startID] = item2D;

        // FIXME: 位置設定がうまくいっていないためゴリ押し調整
        Vector3 panelPos = Vector3.zero;
        panelPos.x = Screen.width / 2 + _posXMin + (_posXMax - _posXMin) * (_id - _startID) / (PANEL_NUM - 1);
        panelPos.y = Screen.height / 2 + _openPosY - _item2D.rectTransform.sizeDelta.y - 14.7f;
        panelPos.z = 0f;

        Sequence seq = DOTween.Sequence();
        seq.OnStart(() =>
            {
                this.enabled = true;
            })
            .AppendCallback(() =>
            {
                FloatReactiveProperty afterScale = new FloatReactiveProperty(item2D.rectTransform.sizeDelta.x);

                DOTween.To(() => afterScale.Value,
                    (s) => afterScale.Value = s,
                    _afterScale,
                    MOVE_TIME);

                afterScale.Subscribe(scale =>
                {
                    item2D.rectTransform.sizeDelta = new Vector2(scale, scale);
                });
            })
            .Join(item2D.rectTransform.DOMove(panelPos, MOVE_TIME))
            .Append(transform.DOLocalMoveY(_openPosY - _hidePosY, MOVE_TIME).SetRelative())
            .AppendCallback(() =>
            {
                item2D.rectTransform.localPosition = Vector3.right * item2D.rectTransform.localPosition.x;
                item2D.rectTransform.SetParent(this.transform, false);
            })
            .OnComplete(() =>
            {
                if (_panelItem.All(hasPanel => hasPanel != null))
                {
                    for (int i = 0; i < PANEL_NUM; i++)
                    {
                        StartCoroutine(PlusLifeAnimation(_panelItem[i], i));
                    }
                }
                else
                {
                    transform.DOLocalMoveY(_hidePosY - _openPosY, MOVE_TIME)
                        .SetRelative()
                        .SetDelay(ACTIVE_TIME)
                        .OnComplete(() =>
                        {
                            this.enabled = false;
                        });
                }
            });
        seq.Play();
    }

    // 1up
    private IEnumerator PlusLifeAnimation(Image item2D, int id)
    {
        // 点滅
        for (int blink_i = 0; blink_i < CommonState.BLINK_NUM; blink_i++)
        {
            item2D.enabled = false;
            yield return new WaitForSeconds(CommonState.BLINK_TIME / 2);
            item2D.enabled = true;
            yield return new WaitForSeconds(CommonState.BLINK_TIME / 2);
        }

        // アニメーション
        item2D.rectTransform.DOMove(_lifePanel.transform.position, MOVE_TIME)
            .SetDelay(id * MOVE_TIME)
            .OnComplete(() =>
            {
                item2D.enabled = false;

                // 1up
                if (id + _startID == ItemName.ItemNameList.PanelG)
                {
                    _lifePanel.AddLife();
                    Destroy(this.gameObject);
                }
            });
    }
}
