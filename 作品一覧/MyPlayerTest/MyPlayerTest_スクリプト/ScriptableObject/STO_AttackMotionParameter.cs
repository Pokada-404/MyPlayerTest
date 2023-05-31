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

[CreateAssetMenu(menuName = "Parameter/AttackMotionParameter", fileName = "NewAttackMotionParameter")]
public class STO_AttackMotionParameter : ScriptableObject
{
    [Header("- 詳細設定 -")]
    public List<AttackMotionParameter> _attackParameters;
    [System.Serializable]
    public class AttackMotionParameter
    {
        [Label("アニメーションクリップ名")]
        public string _AnimationClipName = default;
        [Label("モーション内容"), Multiline(2)]
        public string _motionContent = default;
        [Label("攻撃力")]
        public float _damage = default;
        [Label("複数体にダメージを与える")]
        public bool _attackTarget_Plural = default;
        [Label("攻撃判定のクールタイム ※1"), Range(0, 1)]
        public float _decisionCoolTime = default;
        [Header("                   ※1 値を小さくすれば連続でダメージを与えられる、０にすればモーションが終わるまで再び当たらない")]
        [Label("ヒットストップ (Frame数)")]
        public float _hitStopTime = default;
        [Label("ヒットスロー (Frame数)")]
        public float _hitSlowTime = default;
        [Label("ヒットスローレベル"), Range(0, 1)]
        public float _hitSlowLevel = default;
    }
}
