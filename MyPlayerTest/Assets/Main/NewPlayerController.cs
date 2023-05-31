// ---------------------------------------------------------
// NewPlayerController.cs
//
// 作成日:2023/5/15
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
* このスクリプトはアニメーターとアニメーションクリップに依存しています。
* 
*/

#endregion

#region ▼　Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class NewPlayerController : HumanCharacterBaseProcess
{
    protected PlayerInput PlayerInput;

    private void Start()
    {
        //変数の値設定
        SetActionEvent();
        SetValueGlobalVariable();
        SetValueFromScriptableObject();
    }

    private void Update()
    {
        //z座標の固定
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //HPが０になれば死亡
        if (_nowHP <= 0) { gameObject.SetActive(false); return; }

        if (!SystemStop)
        {
            // *--- 重力の適用と当たり判定取得処理 ---*
            if (MyRigitBodyUse()) { return; }

            // *--- 入力処理 ---*
            GetMoveValue();

            // *--- 移動処理 ---*
            HorizontalMove();

            // *--- ジャンプ処理 ---*
            Jump();

            // *--- 攻撃処理 ---*
            Attack();

            // *--- ブリンク処理 ---*
            Blink();

            // *--- オイル処理 ---*
            InsertOil();

            // *--- 変数の初期化処理 ---*
            ResetVariable();

            // *--- アニメーションのパラメータ設定 ---*
            AnimationUpdate();
        }
    }


    #region ▼ 入力取得処理
    // **********************************************************************
    #region 説明

    /*
     *InputSystemで入力を取得している。
     *メソッドの内容は上から順に
     *・Actionを使うための設定
     *・設定のリセット
     *・入力の検知用メソット群
     *となっている。
     */

    #endregion

    /// <summary>
    /// デバイス設定処理
    /// </summary>
    /// <param name="connectDevice">割り当てるデバイス</param>
    public void SetDeice(InputDevice connectDevice)
    {
        if (PlayerInput != null)
        {
            PlayerInput.Disable();
            PlayerInput?.Dispose();
        }
        PlayerInput = new PlayerInput
        {
            devices = new[] { connectDevice }
        };
        PlayerInput.Enable();
        SetActionEvent();
    }

    /// <summary>
    /// ActionMap設定処理
    /// </summary>
    private void SetActionEvent()
    {
        // Actionイベント登録
        PlayerInput.Action.Move_UP.performed += OnMoveUp;
        PlayerInput.Action.Move_Down.performed += OnMoveDown;
        PlayerInput.Action.Move_Right.performed += OnMoveRight;
        PlayerInput.Action.Move_Left.performed += OnMoveLeft;
        PlayerInput.Action.Jump_Press.performed += OnJumpPress;
        PlayerInput.Action.Jump_Release.performed += OnJumpRelease;
        PlayerInput.Action.Attack.performed += OnAttack;
        PlayerInput.Action.Blink.performed += OnBlink;
        PlayerInput.Action.InsertOil.performed += OnInsertOil;
    }

    // 自身でインスタンス化したActionクラスは以下の処理が必須
    private void OnEnable()
    {
        if (PlayerInput != null) { PlayerInput.Enable(); }
    }
    private void OnDisable()
    {
       // PlayerInput.Disable();
    }
    private void OnDestroy()
    {
        PlayerInput?.Dispose();
    }


    //移動入力
    private void OnMoveUp(InputAction.CallbackContext context)
    {
        if (_blinkLimitTime <= 0) { return; }
        _isInputUpButton = _isInputUpButton ? false : true;
        _move.y = _isInputUpButton ? 1 : 0;
    }
    private void OnMoveDown(InputAction.CallbackContext context)
    {
        if (_blinkLimitTime <= 0) { return; }
        _isInputDownButton = _isInputDownButton ? false : true;
        _move.y = _isInputDownButton ? -1 : 0;
    }
    private void OnMoveRight(InputAction.CallbackContext context)
    {
        _isInputRightButton = _isInputRightButton ? false : true;
        _move.x = _isInputRightButton ? 1 : 0;
        _move.x = _isInputLeftButton ? -1 : _move.x;
    }
    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        _isInputLeftButton = _isInputLeftButton ? false : true;
        _move.x = _isInputLeftButton ? -1 : 0;
        _move.x = _isInputRightButton ? 1 : _move.x;
    }
    private void GetMoveValue()
    {
        _isInputMove = !_isInputUpButton && !_isInputDownButton && !_isInputRightButton && !_isInputLeftButton ? false : true;
        if (!_isInputMove && !_isGround)
        {
            _isJumpInertia = true;
        }
        else
        {
            _isJumpInertia = false;
        }
    }

    //ジャンプ入力
    private void OnJumpPress(InputAction.CallbackContext context)
    {
        if (_isMoveLimit || !_isGround || _isBlink || (!_isJumped && _isJumping)) { return; }
        _isInputJump = true;
    }
    private void OnJumpRelease(InputAction.CallbackContext context)
    {
        _isInputJump = false;
    }

    //攻撃入力
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (_isBlink || _isStopAttackInput) { return; }
        _isInputAttack = true;
    }

    //ブリンク入力
    private void OnBlink(InputAction.CallbackContext context)
    {
        _isInputBlink = true;
    }

    //オイル入力
    private void OnInsertOil(InputAction.CallbackContext context)
    {
        _isInputOIL = true;
    }

    // **********************************************************************
    #endregion


    #region ▼ MyRigitBodyによる判定と敵リスト取得処理
    // **********************************************************************
    #region 説明

    /*
     * MyRigitBodyによる判定取得処理
     * returnがtrueならメインの処理もreturn(処理をせず終了)する
     */

    #endregion
    /// <summary>
    /// 重力と接触判定の取得
    /// </summary>
    /// <returns>天井や壁に接触していた場合trueを返す</returns>
    protected bool MyRigitBodyUse()
    {
        //処理を走らせる 攻撃中は処理を走らせたくない
        MyRigitBody.RigitBodyUpdate();
        //判定の取得
        _isGround = MyRigitBody.SetGroundDecision;
        _isHitEnemy = MyRigitBody.SetEnemyDecision;
        _isWallDecision = MyRigitBody.SetWallDecision;
        _isCeiling = MyRigitBody.SetCeilingDecision;
        _enemyList = MyRigitBody.SetEnemyObjectList;
        //処理を止める
        if (_isCeiling) { _jumpCountPower = 0; return true; }
        if (_isWallDecision) { return true; }
        return false;
    }

    #endregion


    #region ▼ プレイヤー挙動
    //--------------------------------------------------//
    #region 説明

    /*
     * プレイヤー挙動
     * 
     */

    #endregion
    protected void HorizontalMove()
    {
        //制限処理
        if (_isJumpInertia)
        {
            transform.position += new Vector3(_nowSpeed * _directionOfMovement, 0, 0) * Time.deltaTime;
            return;
        }
        if (_isOIL)
        {
            Move(CharacterParameter._defaultDeceleration, CharacterParameter._defaultAcceleration, CharacterParameter._defaultMaxSpeed, CharacterParameter._defaultAirSwitchDirDeceleration, CharacterParameter._defaultSwitchDirDeceleration);
        }
        else if(_isBlink)
        {
            Move(CharacterParameter._defaultDeceleration, CharacterParameter._defaultAcceleration * 2, CharacterParameter._defaultMaxSpeed * 1.5f, CharacterParameter._defaultAirSwitchDirDeceleration, CharacterParameter._defaultSwitchDirDeceleration);
        }
        else
        {
            Move(CharacterParameter._defaultDeceleration, CharacterParameter._defaultAcceleration, CharacterParameter._defaultMaxSpeed, CharacterParameter._defaultAirSwitchDirDeceleration, CharacterParameter._defaultSwitchDirDeceleration);
        }
        transform.position += new Vector3(_nowSpeed * _directionOfMovement, 0, 0) * Time.deltaTime;
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    protected void Move(float Deceleration, float Acceleration, float MaxSpeed, float AirSwitchDirDeceleration, float SwitchDirDeceleration)
    {
        //制限処理
        if (_isMoveLimit || !_isInputMove)
        {
            //減速処理のみ行う
            _nowSpeed = _nowSpeed > 0 ? SpeedDown(_nowSpeed, Deceleration) : _nowSpeed;
            //速度を０にする
            _nowSpeed = _isBlink ? 0 : _nowSpeed;
            return;
        }

        //入力のあった方向を取得し、移動方向と同じなら速度計算のみ行う
        _directionOfInput = _move.x > 0 ? Input_Right : Input_Left;
        if (_directionOfMovement == _directionOfInput)
        {
            _nowSpeed = SpeedUp(_nowSpeed, Acceleration, MaxSpeed);
            return;
        }

        //方向転換を行う
        if (_nowSpeed > 0)
        {
            //空中と地上で異なった慣性をかける
            _nowSpeed = _isJumping ? SpeedDown(_nowSpeed, AirSwitchDirDeceleration) : //空中の慣性
                                     SpeedDown(_nowSpeed, SwitchDirDeceleration); //地上の慣性
            return;
        }

        //向き情報の切り替えと方向転換
        _directionOfMovement = _directionOfInput;
        StartCoroutine(Turn());
    }
    //方向転換を行う
    protected IEnumerator Turn()
    {
        // 秒数(waitTime)秒後に秒数(updateSecond)秒あたり角度(updateAngle)度、回数(updateCount)回だけ回転させる
        for (int i = 0; i < updateCount; i++)
        {
            transform.Rotate(0f, updateAngle, 0f);
            yield return new WaitForSeconds(updateSecond);
        }
    }


    /// <summary>
    /// ジャンプ処理
    /// </summary>
    protected void Jump()
    {
        if (_isInputJump)
        {
            //初回のみ処理
            if (!_isJumped)
            {
                Animator.SetBool("Jump", true);
                _isJumped = true;
                _jumpCountPower = CharacterParameter._jumpPower + _gravityValue;
                _jumpCountTime = CharacterParameter._jumpMaxTime;
            }

            //時間経過で強制的にジャンプ終了
            _jumpCountTime -= Time.deltaTime;
            if (_jumpCountTime < 0)
            {
                _isInputJump = false;
                return;
            }

            //上昇時の減速処理と上昇処理
            _jumpCountPower -= CharacterParameter._jumpUpPowerDeceleration * Time.deltaTime;
            transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
        }
        else
        {
            //直前までジャンプ処理が行われていたか
            if (_isJumped)
            {
                Animator.SetBool("Jump", false);
                _isJumped = false;
                _jumpCountTime = CharacterParameter._jumpMaxTime;
            }
            //落下に転じる際の減速処理
            if (!_isGround && _jumpCountPower > 0)
            {
                _jumpCountPower -= CharacterParameter._jumpLimitPowerDeceleration * Time.deltaTime;
                transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
            }
        }
    }


    /// <summary>
    /// 攻撃処理
    /// </summary>
    // 攻撃のメイン処理
    protected void Attack()
    {
        //攻撃の入力が入り、かつ入力の受付制限がかかっていない
        if (_isInputAttack && !_isStopAttackInput)
        {
            _isInputAttack = false;
            Animator.SetTrigger("Attack");
        }

        //敵に攻撃が当たり、かつ判定受付に制限がかかっていない
        if (_isHitEnemy && !_isStopHitDecision)
        {
            //コンボ継続時間のリセット
            _comboDisconnectionTimer = CharacterParameter._comboDisconnectionTimeLimits;
            //判定のクールタイム中ではない
            if (!_isStopHitDecisionCoolTime)
            {
                StartCoroutine(HitDecision());
            }
        }

        //コンボ中にアイドル状態へ遷移した場合一定時間後にコンボ終了
        if (_comboCounter != 0 && _isAttackToIdle)
        {
            _comboDisconnectionTimer -= Time.deltaTime;
            if (_comboDisconnectionTimer <= 0)
            {
                print("ComboDisConnected");
                _comboCounter = 0;
                _comboDisconnectionTimer = CharacterParameter._comboDisconnectionTimeLimits;
            }
        }
    }
    //攻撃が敵に当たったときの処理
    protected IEnumerator HitDecision()
    {
        if (_isBlink) { yield break; }
        _isStopHitDecisionCoolTime = true;

        //現在再生中のアニメーション名を取得し、スクリプタブルオブジェクトからパラメータを探す。再生中のアニメーションがアイドルなら処理を行わない
        _attackNowPlayAnimClipName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (_attackNowPlayAnimClipName == IdleAnimation || _attackNowPlayAnimClipName == MoveAnimation) { yield break; }
        for (_attackParameterNum = 0; _attackParameterNames[_attackParameterNum] != _attackNowPlayAnimClipName; _attackParameterNum++) { }

        //コンボ数をカウント
        _comboCounter++;
        print("hit : " + _comboCounter);

        //攻撃が当たったすべての敵にダメージを与える処理
        foreach (GameObject enemyObject in _enemyList)
        {
            HumanCharacterBaseProcess enemyProsses = enemyObject.GetComponent<HumanCharacterBaseProcess>();
            if (enemyProsses == null) { continue; }

            //ダメージを与える処理とアニメーションの再生速度を操作して行うヒットストップとスロー処理を行う
            enemyProsses.TakeDamage(AttackMotion._attackParameters[_attackParameterNum]._damage * _damageMagnification);
            enemyProsses.StartCoroutine(AnimationSpeedChageProcessing(_attackParameterNum));

            //ダメージが単体にしか入らない場合は最初に当たった敵にのみ処理を行い、残りは処理を飛ばす
            if (!AttackMotion._attackParameters[_attackParameterNum]._attackTarget_Plural) { break; }
        }
        //自分自身にもアニメーションの再生速度を操作して行うヒットストップとスロー処理
        StartCoroutine(AnimationSpeedChageProcessing(_attackParameterNum));


        //クールタイムを取得、０の場合は１回当たり判定を取ったらそのアニメーションの再生が終了するまで当たり判定を停止する
        float decisionCoolTime = AttackMotion._attackParameters[_attackParameterNum]._decisionCoolTime;
        if (decisionCoolTime == 0)
        {
            //アニメーションクリップの長さから現在までのアニメーションの再生経過時間を引き、残りの時間当たり判定を停止する
            float animClipLength = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            decisionCoolTime = animClipLength - animClipLength * Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        //クールタイムの開始
        yield return new WaitForSeconds(decisionCoolTime + AttackMotion._attackParameters[_attackParameterNum]._hitStopTime);
        _isStopHitDecisionCoolTime = false;
    }


    /// <summary>
    /// ブリンク処理
    /// </summary>
    protected void Blink()
    {
        if (_isInputBlink)
        {   
            _isInputBlink = false;

            if(_blinkReCastTime > 0) { return; }
            
            _isBlink = true;
            _blinkLimitTime = CharacterParameter._blinkMaxTime;
            _blinkReCastTime = CharacterParameter._blinkReCastTime;
            Animator.SetTrigger("BlinkStart");
        }
        if (_blinkLimitTime > 0)
        {
            //ブリンク処理
            _blinkLimitTime -= Time.deltaTime;

            //追加の移動処理
            transform.position += new Vector3(_nowSpeed * _directionOfMovement * CharacterParameter._blinkMoveSpeedmagnification, 0, 0) * Time.deltaTime;

            if (_blinkLimitTime <= 0) 
            { 
                _isBlink = false;
                Animator.SetTrigger("BlinkEnd");
            }
        }
        else if (_blinkReCastTime > 0)
        {
            //ブリンククールタイム
            _blinkReCastTime -= Time.deltaTime;
        }
    }


    /// <summary>
    /// オイル処理
    /// </summary>
    // オイルのメイン処理
    protected void InsertOil()
    {
        if (_isInputOIL)
        {
            _isInputOIL = false;

            print(_OILLimitTime);
            if (_OILReCastTime > 0) { return; }

            _isOIL = true;
            _OILLimitTime = CharacterParameter._OILMaxTime;
            _OILReCastTime = CharacterParameter._OILReCastTime;
            Animator.speed = CharacterParameter._OILAnimationSpeedMagnification;
            _damageMagnification = AttackMotion._damageMagnification_OIL;
        }
        if (_OILLimitTime > 0)
        {
            //ブリンク処理
            _OILLimitTime -= Time.deltaTime;

            print(_move);

            //追加の移動処理
            transform.position += new Vector3(_nowSpeed * _directionOfMovement * CharacterParameter._OILMoveSpeedMagnification, 0, 0) * Time.deltaTime;

            if (_OILLimitTime <= 0)
            {
                _isBlink = false;
            }
        }
        else if (_OILReCastTime > 0)
        {
            //ブリンククールタイム
            _OILReCastTime -= Time.deltaTime;
            Animator.speed = 1;
            _damageMagnification = 1;
        }
    }


    //--------------------------------------------------//
    #endregion

}
