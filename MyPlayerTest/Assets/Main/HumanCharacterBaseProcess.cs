// ---------------------------------------------------------
// HumanCharacterBaseProcess.cs
//
// 作成日:2023/4/30
// 作成者:岡田悠暉
// 概　要:プレイヤーの一部の処理をまとめたスクリプト
//
// ---------------------------------------------------------

#region ▼　詳細

/* *
* 
* ヒューマン型のキャラクターのベースとなる処理
* 継承先で入力処理と追加で必要な処理を書いて、この中のメソッドを呼び出して使用してください。
*
* ～～ 攻撃モーションの追加方法 ～～
* ①生成したスクリプタブルオブジェクト"STO_NewPlayerParameter"を開く
* ②プレイヤーのパラメータの項目"戦闘"の中のリスト"Attack Parameters"を見つけ、「+」を押して新しいモーションを追加する
* ③追加したモーションにパラメータを設定する(アニメーション名はクリップ名と同じにしてください)
* ④アニメーションに変数を追加
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
public class HumanCharacterBaseProcess : MonoBehaviour
{
    #region ▼ 変数

    // *--- クラス参照 -----------------------------------*

    //スクリプタブルオブジェクト
    [SerializeField, Header("ステータスデータ")]
    protected STO_CharacterParameter CharacterParameter;
    [SerializeField, Header("攻撃モーションデータ")]
    protected STO_AttackMotionParameter AttackMotion;
    protected Animator Animator;
    protected MyRigitBody MyRigitBody;

    // *--------------------------------------------------*


    // *--- キャラクターの基本挙動処理 -------------------*
    [SerializeField]
    /// <summary> 現在のHP </summary>
    protected float _nowHP = default;
    /// <summary> すべての攻撃アニメーションクリップ名を格納する配列 </summary>
    protected string[] _attackParameterNames = default;
    /// <summary> 上下左右移動 </summary>
    protected Vector2 _move = default;
    /// <summary> キャラクターが現在移動している方向 </summary>
    protected int _directionOfMovement = default;
    /// <summary> キャラクターに新たに移動させたい方向 </summary>
    protected int _directionOfInput = default;
    /// <summary> 現在の移動速度 </summary>
    protected float _nowSpeed = default;
    /// <summary> ジャンプを開始してから経過した時間 </summary>
    protected float _jumpCountTime = default;
    /// <summary> 現在のジャンプ力 </summary>
    protected float _jumpCountPower = default;
    /// <summary> 現在の連続コンボ数 </summary>
    protected int _comboCounter = default;
    /// <summary> 攻撃終了後コンボが途切れるまでの残り時間 </summary>
    protected float _comboDisconnectionTimer = default;
    protected string _attackNowPlayAnimClipName = default;
    protected int _attackParameterNum = default;
    protected float _stopMotionTime;
    protected float _slowMotionTime;
    protected float _slowMotionLevel;
    //攻撃力の倍率
    protected float _damageMagnification = 1;
    //敵のリスト取得
    protected List<GameObject> _enemyList = default;
    //ブリンク状態の時間制限
    protected float _blinkLimitTime = default;
    //ブリンク後のクールタイム
    protected float _blinkReCastTime = default;
    //オイル使用状態の時間制限
    protected float _OILLimitTime = default;
    //オイル使用後のクールタイム
    protected float _OILReCastTime = default;


    protected float _gravityValue = default;

    // *--------------------------------------------------*


    // *--- フラグ ---------------------------------------*

    // プレイヤーの全処理停止用
    protected bool SystemStop = false;
    //当たり判定取得
    protected bool _isGround = default;
    protected bool _isCeiling = default;
    protected bool _isWallDecision = default;
    protected bool _isHitEnemy = default;
    //入力の取得用
    protected bool _isInputMove = default;
    protected bool _isInputUpButton = default;
    protected bool _isInputDownButton = default;
    protected bool _isInputRightButton = default;
    protected bool _isInputLeftButton = default;
    protected bool _isInputJump = default;
    protected bool _isInputAttack = default;
    protected bool _isInputBlink = default;
    protected bool _isInputOIL = default;
    //特殊な挙動処理を行うため他の処理を制限する オイル処理とブリンクで使用
    protected bool _isBlink = default;
    protected bool _isOIL = default;
    //攻撃の当たり判定クールタイム中
    protected bool _isStopHitDecisionCoolTime = default;
    //ジャンプ処理の初回のみ行いたい処理の際に使う
    protected bool _isJumped = default;
    //ジャンプ中に移動入力が行われていないか
    protected bool _isJumpInertia = default;

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
        _stopMotionTime = AttackMotion._attackParameters[attackParameterNum]._hitStopTime;
        _slowMotionTime = AttackMotion._attackParameters[attackParameterNum]._hitSlowTime;
        _slowMotionLevel = AttackMotion._attackParameters[attackParameterNum]._hitSlowLevel;

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
        
        if (DamageValue < CharacterParameter._minDamageValue)
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
        _nowHP = CharacterParameter._maxHP <= RecoveryValue ? CharacterParameter._maxHP : _nowHP + RecoveryValue;
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
        _nowHP = CharacterParameter._maxHP;
        _comboDisconnectionTimer = CharacterParameter._comboDisconnectionTimeLimits;


        for (int i = 0; i < AttackMotion._attackParameters.Count; i++)
        {
            _attackParameterNames[i] = AttackMotion._attackParameters[i]._AnimationClipName;
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
        _jumpCountTime =  CharacterParameter._jumpMaxTime;
        _isStopHitDecision = true;
        _attackParameterNames = new string[AttackMotion._attackParameters.Count];
    }

    /// <summary>
    /// 変数の初期化処理
    /// </summary>
    protected void ResetVariable()
    {
    }

    public float SetGravity { set { _gravityValue = value; } }


    /// <summary>
    /// Triggerを除くアニメーションのパラメータの設定処理
    /// </summary>
    protected void AnimationUpdate()
    {
        Animator.SetFloat("MoveSpeed", _nowSpeed / CharacterParameter._defaultMaxSpeed);
        Animator.SetBool("IsGround", _isGround);
        Animator.SetFloat("MoveDirection", _directionOfMovement);
    }

    #endregion

}
