using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private void Start()
    {
        //Observable.EveryUpdate()
        //    .Select(_ => Input.inputString)
        //    .Where(key => Input.anyKey)
        //    .Subscribe(key =>
        //    {
        //        if (key == CommonState.MOVE_LEFT_KEY || key == CommonState.MOVE_RIGHT_KEY)
        //        {
        //            _player.Move(key);
        //        }
        //    });

        //Observable.EveryUpdate()
            //.Select(_ => Input.inputString)
            //.Where(key => Input.anyKeyDown)
            //.Subscribe(key =>
            //{
            //    if (key == CommonState.JUMP_KEY)
            //    {
            //        _player.Jump();
            //    }
            //});
    }
}
