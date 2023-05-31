// ---------------------------------------------------------
// NewPlayerController.cs
//
// 作成日:2023/4/30
// 作成者:岡田悠暉
// 概　要:プレイヤーの挙動制御用スクリプト
//
// ---------------------------------------------------------

#region ▼　詳細

/*
* 
* 
* 
* アニメーションに関して注意点
* このスクリプトはアニメーターとアニメーションクリップに大きく依存しています。
* アニメーションを編集する場合は以下の内容に注意してください
* [ AttackLayer ]
*   ・アイドル(Attack_Idle)の長さがコンボが途切れるまでの猶予時間になっています。
*     フラグ"_isComboCounterReset"はアニメーション先頭をfalseにし、終端より1フレーム前でtrueにしてください 。
*     フラグ"_isAttackProsses"と"_isStopInputAttack"はアニメーション先頭と終端でfalseに設定してください。
*   ・攻撃アニメーションのフラグ"_isAttackProsses"がtrueになっている間は攻撃入力を受け付けません。
*     falseになったタイミングからアニメーションの終端までが攻撃入力の受付時間になります。
*   ・フラグ"_isStopInputAttack"はアニメーションの最後まで再生が終了したことを通知するフラグです。
*     アニメーションの先頭がtrueで終端をfalseにしてください。
*/

#endregion

#region ▼　Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class Test_MyRigitBodyUse : MonoBehaviour
{
    [SerializeField]
    private bool _isGround = default;
    [SerializeField]
    private bool _isEnemy = default;
    private MyRigitBody MyRigitBody;

    private void OnEnable()
    {
        MyRigitBody = this.GetComponent<MyRigitBody>();
    }

    /// <summary>
    /// メイン処理
    /// </summary>
    private void Update()
    {
        // *--- 重力の適用と当たり判定取得処理 ---*
        _isGround = MyRigitBody.SetGroundDecision;
        _isEnemy = MyRigitBody.SetEnemyDecision;
    } 
}
