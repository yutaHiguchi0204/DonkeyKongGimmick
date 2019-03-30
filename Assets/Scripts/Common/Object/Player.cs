using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Player : MonoBehaviour
{
    private const float MOVE_DISTANCE = 0.5f;
    private const float JUMP_DISTANCE = 3f;

    private bool _isGround = false;
    public bool IsGround
    {
        get
        {
            return _isGround;
        }
    }

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private CapsuleCollider _capsuleCollider;

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // アニメーション初期化
                //_animator.SetBool("walk", false);
                _animator.SetBool("jump", false);

                // 着地判定
                CheckLanding();
            });
    }

    // 移動処理
    public void Move(string moveKey)
    {
        Quaternion quaternion = transform.localRotation;
        if (moveKey == CommonState.MOVE_LEFT_KEY)
        {
            _rigidbody.velocity = new Vector3(MOVE_DISTANCE, 0f, 0f);
            quaternion.y = -90f;
        }
        else if (moveKey == CommonState.MOVE_RIGHT_KEY)
        {
            _rigidbody.velocity += new Vector3(MOVE_DISTANCE, 0f, 0f);
            quaternion.y = 90f;
        }
        transform.localRotation = quaternion;
        _animator.SetBool("walk", true);
    }

    // ジャンプ処理
    public void Jump()
    {
        _rigidbody.AddForce(new Vector3(0f, JUMP_DISTANCE, 0f));
        _animator.SetBool("jump", true);
    }

    // 着地判定
    private void CheckLanding()
    {
        RaycastHit isHitGround;
        if (Physics.SphereCast(transform.position, _capsuleCollider.radius, Vector3.down, out isHitGround, _capsuleCollider.height / 2 - _capsuleCollider.radius, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            _isGround = true;
        }
        else
        {
            _isGround = false;
        }
        Debug.Log(_isGround);
    }
}
