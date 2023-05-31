// ---------------------------------------------------------
// STO_CharacterParameter.cs
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

[CreateAssetMenu(menuName = "Parameter/CharacterParameter", fileName = "NewCharacterParameter")]
public class STO_CharacterParameter : ScriptableObject
{
    [Header("- 詳細設定 -\nステータス")]
    [Label("最大HP")]
    public float _maxHP = default;

    [Header("油さし")]
    #region ▼ 仕様説明
    /*
     *油さし
     *ボタンを押すと油さしができる
     *油さしはHPを一定割合使用することで能力が一時的に強化されるというもの
     *強化される能力は以下の通り
     *・空中移動回数の増加
     *・移動速度上昇
     *・ブリンク時移動可能範囲の拡大
     *・攻撃速度上昇
     *・
     *オイルゲージは一定割合減少していき、0になると通常の能力値にもどる。
     */
    #endregion
    [Label("油さしに使うHP割合(％)"), Range(0, 100)]
    public float _insertOilUseHpRatio = default;
    [Label("オイルゲージの１秒毎の減少割合(％)"), Range(0, 100)]
    public float _oilDecreaceRatio = default;

    //移動関連
    [Header("- 移動関連 -\n通常時地上")]
    [Label("移動速度(m/s)")]
    public float _defaultMaxSpeed = default;
    [Label("移動加速度(m/s)")]
    public float _defaultAcceleration = default;
    [Label("移動減速度(m/s)")]
    public float _defaultDeceleration = default;
    [Label("方向転換減速度(m/s)")]
    public float _defaultSwitchDirDeceleration = default;

    //[Header("通常時空中")]
    //[Label("移動速度(m/s)")]
    //public float _defaultAirMaxSpeed = default;
    //[Label("移動加速度(m/s)")]
    //public float _defaultAirAcceleration = default;
    //[Label("移動減速度(m/s)")]
    //public float _defaultAirDeceleration = default;
    //[Label("方向転換減速度(m/s)")]
    //public float _defaultAirSwitchDirDeceleration = default;
    //[Label("移動回数上限")]
    //public int _defaultAirWalkUsageLimit = default;

    //[Header("オイル使用時地上")]
    //[Label("移動速度(m/s)")]
    //public float _insertOilMaxSpeed = default;
    //[Label("移動加速度(m/s)")]
    //public float _insertOilAcceleration = default;
    //[Label("移動減速度(m/s)")]
    //public float _insertOilDeceleration = default;
    //[Label("方向転換減速度(m/s)")]
    //public float _insertSwitchDirDeceleration = default;

    //[Header("オイル使用時空中")]
    //[Label("移動速度(m/s)")]
    //public float _insertOilAirMaxSpeed = default;
    //[Label("移動加速度(m/s)")]
    //public float _insertOilAirAcceleration = default;
    //[Label("移動減速度(m/s)")]
    //public float _insertOilAirDeceleration = default;
    //[Label("方向転換減速度(m/s)")]
    //public float _insertOilAirSwitchDirDeceleration = default;
    //[Label("移動回数上限")]
    //public int _insertOilAirWalkUsageLimit = default;

    [Header("ジャンプ ")]
    #region ▼ 仕様説明
    /*
     *ジャンプ
     *ボタンを押すとジャンプができる
     *ボタンの押されている長さによって高さが変わる(スパマリのジャンプをイメージしてもらうといい)
     *短めの入力と長めの入力のジャンプの高さの違いなどを設定している
     */
    #endregion
    [Label("ジャンプ力(m/s)")]
    public float _jumpPower;
    [Label("ジャンプ上昇時の減速度(m/s)")]
    public float _jumpUpPowerDeceleration;
    [Label("高度制限による減速度(m/s)")]
    public float _jumpLimitPowerDeceleration;
    [Label("最高高度到達までの時間(s)")]
    public float _jumpMaxTime;
     
    [Header("戦闘 ")]
    #region ▼ 仕様説明
    /*
     *戦闘
     *攻撃力はコンボ中やオイル使用中以外は一定
     */
    #endregion
    [Label("ブリンク使用後のクールタイム")]
    public float _blinkReCastTime = default;
    [Label("オイルドロップ数"), Range(1, 5)]
    public int _dropOilAmount = 1; //default値
    [Label("オイルドロップ確率"), Range(0, 100)]
    public int _dropOilProbability = default;
    [Label("被ダメージ - 強攻撃となるダメージの値の最低値")]
    public float _minDamageValue = default;
    [Label("被ダメージ - 強攻撃を受けた時のノックバック距離(m)")]
    public float _knockBackRange = default;

    [Header("コンボ ")]
    #region ▼ 仕様説明
    /*
     *コンボ
     *攻撃入力後さらに攻撃を入力するとコンボができる
     *コンボは入力連打で際限なく続く
     *コンボ中は攻撃力がと攻撃を繰り出す速度が上がる
     */
    #endregion
    [Label("コンボによる攻撃力上昇 有効")]
    public bool _isAttackPowerUP = default;
    [Label("攻撃力上昇率(攻撃が当たるたび上昇)")]
    public float _comboRateOfUp = default;
    [Label("コンボ中攻撃力")]
    public float _comboAttackPower = default;
    [Label("コンボ継続時間(s)")]
    public float _comboDisconnectionTimeLimits;

    [Header("ブリンク ")]
    #region ▼ 仕様説明
    /*
     *ブリンク
     */
    #endregion
    [Label("ブリンク 効果時間")]
    public float _blinkLimitTime= default;

}
