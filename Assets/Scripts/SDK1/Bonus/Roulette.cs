using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class Roulette : BonusFactory
{
    // 配置座標
    [SerializeField]
    private float _posXMin = -3f;
    [SerializeField]
    private float _posXMax = 3f;
    [SerializeField]
    private float _posY = 3f;

    // バレルの数
    [SerializeField]
    private int _barrelNum = 4;

    // バレル
    private List<RouletteBarrel> _rouletteBarrel = new List<RouletteBarrel>();

    // ルーレットバレルのプレハブ
    [SerializeField]
    private RouletteBarrel _rouletteBarrelPrefab;

    // アイテムの種類
    [SerializeField]
    private List<ItemName.ItemNameList> _itemIDList = new List<ItemName.ItemNameList>();

    // 選択アイテムのIDリスト
    private ReactiveCollection<int> _pickItemIDList = new ReactiveCollection<int>();

    // 爆発エフェクト
    [SerializeField]
    private ParticleSystem _explosion;

    // 確定アイテム
    [SerializeField]
    private Item _correctItem;

    // 回転インターバル
    [SerializeField]
    private float _realInterval = 0.5f;

    // 点滅回数
    private int _blinkNum = 3;

    // 点滅時間
    private float _blinkTime = 0.3f;

    // 成功時にバレルが集まる時間
    [SerializeField]
    private float _gatherTime = 0.15f;

    private void Start()
    {
        // 配置
        for (int i = 0; i < _barrelNum; i++)
        {
            Vector3 defaultPos = transform.position;
            RouletteBarrel barrel = Instantiate(_rouletteBarrelPrefab, new Vector3(defaultPos.x + _posXMin + (_posXMax - _posXMin) * i / (_barrelNum - 1), _posY, defaultPos.z), Quaternion.identity);
            barrel.Initialize(i, _itemIDList, _realInterval);
            barrel.transform.SetParent(this.transform, false);
            _rouletteBarrel.Add(barrel);
        }

        // 監視
        Bind();
    }

    private void Bind()
    { 
        for (int i = 0; i < _barrelNum; i++)
        {
            _rouletteBarrel[i].PickItemID
                .Where(id => id > 0)
                .Subscribe(id =>
                {
                    _pickItemIDList.Add(id);
                });
        }

        // ルーレット判定
        _pickItemIDList
            .ObserveAdd()
            .Subscribe(_ =>
            {
                // 最初の値を基準とする
                int correctID = _pickItemIDList[0];

                // 全て一致しているか
                if (_pickItemIDList.All(id => id == correctID))
                {
                    // 選択バレル数チェック
                    if (_pickItemIDList.Count >= _barrelNum)
                    {
                        // 成功
                        _isCorrected.Value = true;
                        _isFinished.Value = true;
                        Corrected();
                    }
                    return;
                }

                // 失敗
                _isCorrected.Value = false;
                _isFinished.Value = true;
                Failed();
            });
    }

    // 成功
    private void Corrected()
    {
        // 集まる位置
        Vector3 gatherPos = transform.position;
        gatherPos.y = _posY;

        Sequence seq = DOTween.Sequence();
        seq.OnStart(() =>
        {
            for (int i = 0; i < _barrelNum; i++)
            {
                StartCoroutine(_rouletteBarrel[i].Blink(_blinkNum, _blinkTime));
            }
        })
        .AppendInterval(_blinkTime * _blinkNum)
        .AppendCallback(() =>
        {
            for (int i = 0; i < _barrelNum; i++)
            {
                _rouletteBarrel[i].transform.DOMove(gatherPos, _gatherTime);
            }
        })
        .AppendInterval(_gatherTime)
        .AppendCallback(() =>
        {
            for (int i = 0; i < _barrelNum; i++)
            {
                Destroy(_rouletteBarrel[i].gameObject);
            }
            Instantiate(_explosion, gatherPos, Quaternion.identity);
            Item correctItem = Instantiate(_correctItem, gatherPos, _correctItem.transform.localRotation);
            correctItem.ID = _pickItemIDList[0];
            correctItem.Initialize();
        });
        seq.Play();
    }

    // 失敗
    private void Failed()
    {
        for (int i = 0; i < _barrelNum; i++)
        {
            StartCoroutine(_rouletteBarrel[i].Failed());
        }
    }
}
