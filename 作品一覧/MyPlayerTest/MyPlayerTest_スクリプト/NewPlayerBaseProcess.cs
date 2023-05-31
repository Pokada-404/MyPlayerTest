// ---------------------------------------------------------
// NewPlayerBaseProcess.cs
//
// 作成日:2023/4/30
// 作成者:岡田悠暉
// 概　要:プレイヤーの一部の処理をまとめたスクリプト
//
// ---------------------------------------------------------

#region ▼　詳細

/* *
* 
* プレイヤーに関する処理
*
* ～～ 攻撃モーションの追加方法 ～～
* ①生成したスクリプタブルオブジェクト"STO_NewPlayerParameter"を開く
* ②プレイヤーのパラメータの項目"戦闘"の中のリスト"Attack Parameters"を見つけ、「+」を押して新しいモーションを追加する
* ③追加したモーションにパラメータを設定する
* ④このスクリプトの中のEnum"AttackAnimClip"に設定したモーション名で追加
* ⑤攻撃アニメーションの指定用の定数に同じく設定したモーション名で追加
* ⑥スクリプト"NewPlayerController"を開き、HitDecision()の中のswitch文にcaseを追加
* 以上。
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
public class NewPlayerBaseProcess : MonoBehaviour
{
    #region ▼ 変数
    //残りカス
    /// <summary>
    /// 無敵中フラグ
    /// </summary>
    protected bool _isInvincible = default;
    /// <summary>
    /// 死亡フラグ
    /// </summary>
    protected bool _isDead = default;


    // *--- クラス参照 -----------------------------------*

    [SerializeField, Header("プレイヤーステータスデータ(スクリプタブルオブジェクト)")]
    protected STO_CharacterParameter PlayerParameter;
    [SerializeField, Header("プレイヤー攻撃モーションデータ(スクリプタブルオブジェクト)")]
    protected STO_AttackMotionParameter PlayerAttackMotion;
    protected Animator Animator;
    protected PlayerInput PlayerInput;
    protected MyRigitBody MyRigitBody;

    // *--------------------------------------------------*


    // *--- プレイヤー挙動処理関連 -----------------------*

    //移動
    protected Vector2 _move = default;
    protected int _directionOfMovement = default;
    protected int _directionOfInput = default;
    protected float _nowSpeed = default;
    //ジャンプ
    protected float _jumpCountTime = default;
    protected float _jumpCountPower = default;
    //攻撃
    protected int _comboCounter = default;
    protected float _comboDisconnectionTimer = default;
    protected string _attackNowPlayAnimClipName = default;
    protected int _attackParameterNum = default;
    protected float _stopMotionTime;
    protected float _slowMotionTime;
    protected float _slowMotionLevel;
    //敵のリスト取得
    protected List<GameObject> _enemyList = default;

    // *--------------------------------------------------*


    // *--- フラグ ---------------------------------------*

    //当たり判定取得
    protected bool _isGround = default;
    protected bool _isCeiling = default;
    protected bool _isWallDecision = default;
    protected bool _isHitEnemy = default;
    //入力の取得用
    protected bool _isInputMove = default;
    protected bool _isInputJump = default;
    protected bool _isInputAttack = default;
    protected bool _isInputOIL = default;
    // プレイヤーの全処理停止用 (コルーチンによる時間差処理は除く)
    protected bool SystemStop = false;
    //特殊な挙動処理を行うため他の処理を制限する オイル処理とブリンクで使用
    protected bool _isStopOtherThanProsses = default;
    //攻撃の当たり判定クールタイム中
    protected bool _isStopHitDecisionCoolTime = default;
    //ブリンクの入力制限
    protected bool _isStopInputBlink = default;
    //疑似慣性の処理初回のみ動作用
    protected bool _isInertiaPlosses = default;
    //ジャンプ済かどうかの判定用
    protected bool _isJumped = default;

    // *--------------------------------------------------*


    // *--- AnimationClipから操作 ------------------------*

    /// <summary>
    /// 攻撃アニメーションからアイドルへの遷移検知用 攻撃終了後にこのフラグはtrueになる
    /// </summary>
    [SerializeField,HideInInspector]
    protected bool _isAttackToIdle = default;
    /// <summary>
    /// ジャンプ中の処理制限用 ジャンプ中にこのフラグはtrueになる
    /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isJumping = default;
    /// <summary>
    /// 移動処理の制限用 攻撃中にこのフラグはtrueになる
    /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isMoveLimit = default;
    /// <summary>
    /// 攻撃入力の停止用 trueの間攻撃入力を取得しない
    /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isStopAttackInput = default;
    /// <summary>
    /// 攻撃の当たり判定処理の制限用 trueの間当たり判定を取得しない
    /// /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isStopHitDecision = default;

    // *--------------------------------------------------*


    // *--- スクリプタブルオブジェクトから取得する変数 ---*

    [SerializeField]//デバッグ作業用にシリアライズ
    protected float _nowHP = default;
    protected float _maxHP = default;
    protected float _maxSpeed = default;
    protected float _acceleration = default;
    protected float _deceleration = default;
    protected float _jumpCountTimeLimit = default;
    protected float _switchDirDeceleration = default;
    protected float _jumpPower = default;
    protected float _jumpUpPowerDeceleration = default;
    protected float _jumpLimitPowerDeceleration = default;
    protected float _jumpMaxTime = default;
    protected float _blinkLimitTime = default;
    protected float _minDamageValue = default;
    protected string[] _attackParameterNames = new string[10];   //すべての攻撃アニメーションクリップ名を格納する配列 アニメーションクリップが増えれば上限あげてもいい

    // *--------------------------------------------------*


    // *--- 定数 -----------------------------------------*

    //移動処理で使用する向き設定用
    protected const int Input_Right = 1;
    protected const int Input_Left = -1;
    //アニメーションのアイドルアニメーション指定用
    protected const string IdleAnimation = "Idle";
    protected const string MoveAnimation = "Move";
    //アニメーションのLayer指定用
    protected const int TakeDamageLayer = 1;
    protected const float MaxWeight = 1;
    // 秒数(waitTime)秒後に秒数(updateSecond)秒あたり角度(updateAngle)度、回数(updateCount)回だけ回転させる
    protected const int updateAngle = 15;
    protected const int updateCount = 12;
    protected const float updateSecond = 0.01f;
    //方向転換時にターンアニメーションの再生を行う為 指定秒数待ってから転換を行うために使用
    protected const float TurnWaitTime = 0; //現在ターンアニメーションは入れていない為０にしている。削除予定

    // *--------------------------------------------------*

    #endregion

    //処理内容別メゾットまとめ

    #region ▼ ストップ・スロー処理
    #region 説明
    /*
     * 
     * ダメージを与える、受けたときの動きのストップやスローモーション
     * ゲーム全体のタイムスケールを動かして実装する。
     * 実装方法は以下の方法を検討している。
     * 　処理が必要であれば専用処理のメソッドを呼び出す。メソッドでは与えられた時間だけストップ・スロー処理を行う。処理はストップの後にスローが行われる。
     * 　もう少し無駄なく処理が書ける気がする、、
     */
    #endregion

    /// <summary>
    /// 動きのストップやスローモーション処理
    /// </summary>
    /// <param name="attackParameterNum">攻撃のパラメータを指す変数</param>
    public IEnumerator AnimationSpeedChageProcessing(int attackParameterNum)
    {
        _stopMotionTime = PlayerAttackMotion._attackParameters[attackParameterNum]._hitStopTime;
        _slowMotionTime = PlayerAttackMotion._attackParameters[attackParameterNum]._hitSlowTime;
        _slowMotionLevel = PlayerAttackMotion._attackParameters[attackParameterNum]._hitSlowLevel;

        if (_stopMotionTime != 0)
        {
            SystemStop = true;
            Animator.speed = 0;

            //指定フレーム分処理を行う
            yield return new WaitForSeconds(_stopMotionTime);
        }
        if (_slowMotionTime != 0)
        {
            SystemStop = true;
            Animator.speed = _slowMotionLevel;

            //指定フレーム分処理を行う
            yield return new WaitForSeconds(_slowMotionTime);
        }
        Animator.speed = 1;
        SystemStop = false;
    }

    #endregion

    #region ▼ プレイヤー挙動関連

    /// <summary>
    /// 加速度分 値を増加する
    /// </summary>
    /// <param name="moveSpeed">現在の移動速度</param>
    /// <param name="acceleration">加速度</param>
    /// <param name="maxSpeed">最大速度</param>
    /// <returns></returns>
    protected private float SpeedUp(float moveSpeed, float acceleration, float maxSpeed)
    {
        // 加速度を考慮して速度を上昇させる
        // 最大速度を超えないように制限する
        return Mathf.Clamp(moveSpeed + acceleration * Time.deltaTime, 0.0f, maxSpeed);
    }

    /// <summary>
    /// 減速度分 値を減算する
    /// </summary>
    /// <param name="moveSpeed">現在の移動速度</param>
    /// <param name="deceleration">減速度</param>
    /// <returns></returns>
    protected private float SpeedDown(float moveSpeed, float deceleration)
    {
        // 減速度を考慮して速度を減少させる
        // 速度が０以下にならないように制限する
        return (moveSpeed -= deceleration * Time.deltaTime) >= 0.05f ? moveSpeed : 0;
    }

    /// <summary>
    /// ダメージの受ける処理。攻撃側から参照される。ダメージの値によって専用アニメーションの再生を行う。
    /// </summary>
    /// <param name="DamageValue"></param>
    public void TakeDamage(float DamageValue)
    {
        _nowHP -= DamageValue;
        
        if (DamageValue < _minDamageValue)
        {
            //弱攻撃の被ダメージのアニメーションを再生
            Animator.SetTrigger("TakenDamage_Light");
        }
        else
        {
            SystemStop = true;

            //強攻撃の被ダメージのアニメーションを再生
            Animator.SetTrigger("TakenDamage_Heavy");
            StartCoroutine(TakeDamageLayerWeightChange());
        }
        StartCoroutine(TakeDamageLayerWeightChange());
    }
    protected IEnumerator TakeDamageLayerWeightChange()
    {
        Animator.SetLayerWeight(TakeDamageLayer, MaxWeight);
        yield return new WaitForSeconds(0.2f);
        float newLayerWeight = TakeDamageLayer;
        while (newLayerWeight > 0)
        {
            newLayerWeight -= 0.05f;
            Animator.SetLayerWeight(TakeDamageLayer, newLayerWeight);
            yield return new WaitForSeconds(0.01f);
        }
        Animator.SetLayerWeight(TakeDamageLayer, 0);
        SystemStop = false;
    }

    public void RecoveryHealth(float RecoveryValue)
    {
        _nowHP = _maxHP <= RecoveryValue ? _maxHP : _nowHP + RecoveryValue;
    }


    #endregion

    #region ▼ 変数設定
    #region 説明
    /*
     * 
     * 変数の設定と初期化を行うメゾット
     * 
     */
    #endregion

    /// <summary>
    /// スクリプタブルオブジェクトからパラメータを取得する処理
    /// </summary>
    protected void SetValueFromScriptableObject()
    {
        _jumpCountTimeLimit = PlayerParameter._jumpMaxTime;
        _nowHP = PlayerParameter._maxHP;
        _maxHP = PlayerParameter._maxHP;
        _maxSpeed = PlayerParameter._defaultMaxSpeed;
        _acceleration = PlayerParameter._defaultAcceleration;
        _deceleration = PlayerParameter._defaultDeceleration;
        _switchDirDeceleration = PlayerParameter._defaultSwitchDirDeceleration;
        _jumpPower = PlayerParameter._jumpPower;
        _jumpLimitPowerDeceleration = PlayerParameter._jumpLimitPowerDeceleration;
        _jumpMaxTime = PlayerParameter._jumpMaxTime;
        _jumpUpPowerDeceleration = PlayerParameter._jumpUpPowerDeceleration;
        _comboDisconnectionTimer = PlayerParameter._comboDisconnectionTimeLimits;
        _blinkLimitTime = PlayerParameter._blinkLimitTime;
        _minDamageValue = PlayerParameter._minDamageValue;

        for (int i = 0; i < PlayerAttackMotion._attackParameters.Count; i++)
        {
            _attackParameterNames[i] = PlayerAttackMotion._attackParameters[i]._AnimationClipName;
        }
    }

    /// <summary>
    /// グローバル変数の設定処理
    /// </summary>
    protected void SetValueGlobalVariable()
    {
        //コンポーネント取得
        Animator = this.GetComponent<Animator>();
        MyRigitBody = this.GetComponent<MyRigitBody>();
        if (Animator == null)
        {
            Debug.LogError("Animation がアタッチされていません");
            SystemStop = true;
        }
        if (MyRigitBody == null)
        {
            Debug.LogError("MyRigitBody がアタッチされていません");
            SystemStop = true;
        }

        //フィールド変数の設定
        _directionOfMovement = Input_Right;
        _jumpCountTime = _jumpMaxTime;
        _isStopHitDecision = true;
    }

    /// <summary>
    /// 変数の初期化処理
    /// </summary>
    protected void ResetVariable()
    {
    }

    /// <summary>
    /// Triggerを除くアニメーションのパラメータの設定処理
    /// </summary>
    protected void AnimationUpdate()
    {
        Animator.SetFloat("MoveSpeed", _nowSpeed / _maxSpeed);
        Animator.SetFloat("MoveSpeed", _nowSpeed / _maxSpeed);
        Animator.SetBool("IsGround", _isGround);
        Animator.SetFloat("MoveDirection", _directionOfMovement);
    }

    #endregion

}
