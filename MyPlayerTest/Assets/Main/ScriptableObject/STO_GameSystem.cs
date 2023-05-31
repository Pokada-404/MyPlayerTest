// ---------------------------------------------------------
// STO_AttackMotionParameter.cs
//
// 作成日:2023/4/30
// 作成者:岡田悠暉
// 概　要:プレイヤーのステータスを持つスクリプタブルオブジェクト
//
// ---------------------------------------------------------

#region ▼　詳細

/* *
* 
* 
* */

#endregion

#region ▼　Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

#endregion

[CreateAssetMenu(menuName = "Parameter/GameSystemParameter", fileName = "NewGameSystemParameter")]
public class STO_GameSystem : ScriptableObject
{
        [Label("重力")]
        public float _gravity = default;
}
