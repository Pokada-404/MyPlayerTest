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

public class NewPlayerController : NewPlayerBaseProcess
{
    #region ▼ メイン処理
    // **********************************************************************
    #region 説明

    /*
     *メインの処理
     *関数の処理のまとめです。
     */

    #endregion

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

            // *--- 移動処理 ---*
            Move();
            transform.position += new Vector3(_nowSpeed * _directionOfMovement, 0, 0) * Time.deltaTime;

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
    private bool MyRigitBodyUse()
    {
        //処理を走らせる
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
    /// InputSystemのActionを使うための設定処理
    /// </summary>
    private void SetActionEvent()
    {
        // Actionスクリプトのインスタンス生成
        //PlayerInput = new PlayerInput();
        // Input Actionを機能させるため有効化
        PlayerInput.Enable();
        // Actionイベント登録
        PlayerInput.Action.Move_Press.performed += OnMovePress;
        PlayerInput.Action.Move_Release.performed += OnMoveRelease;
        PlayerInput.Action.Jump_Press.performed += OnJumpPress;
        PlayerInput.Action.Jump_Release.performed += OnJumpRelease;
        PlayerInput.Action.Attack.performed += OnAttack;
        PlayerInput.Action.Blink.performed += OnBlink;
        PlayerInput.Action.InsertOil.performed += OnInsertOil;
    }
    /// <summary>
    /// プレイヤーを操作するデバイスの上書き
    /// </summary>
    /// <param name="connectDevice"></param>
    public void SetDeice(InputDevice connectDevice)
    {
        //PlayerInput?.Dispose();
        PlayerInput = new PlayerInput
        {
            devices = new[] { connectDevice }
        };
        //PlayerInput.Enable();
    }

    // 設定のリセット処理
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので必ずDisposeする必要がある
        PlayerInput?.Dispose();
    }

    //左右移動ボタン押す
    private void OnMovePress(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _move = context.ReadValue<Vector2>();
        _isInputMove = true;
    }

    //左右移動ボタン離す
    private void OnMoveRelease(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _move = Vector2.zero;
        _isInputMove = false;
    }

    //ジャンプボタン押す
    private void OnJumpPress(InputAction.CallbackContext context)
    {
        //入力が制限されているか
        if (_isMoveLimit) { return; }
        if (!_isGround) { return; }
        //すでに空中にいれば処理を行わない
        _isInputJump = true;
    }

    //ジャンプボタン離す
    private void OnJumpRelease(InputAction.CallbackContext context)
    {
        //ジャンプ処理を止める
        _isInputJump = false;
    }

    //攻撃入力
    private void OnAttack(InputAction.CallbackContext context)
    {
        _isInputAttack = true;
    }

    //ブリンク入力
    private void OnBlink(InputAction.CallbackContext context)
    {
        if (_isStopInputBlink) { return; }
        StartCoroutine(BlinkMove());
    }

    //オイル入力
    private void OnInsertOil(InputAction.CallbackContext context)
    {
        _isInputOIL = true;
    }

    // **********************************************************************
    #endregion


    #region ▼ プレイヤー挙動
    //--------------------------------------------------//
    #region 説明

    /*
     * プレイヤー挙動
     * 
     */

    #endregion

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        //制限処理
        if (_isMoveLimit || !_isInputMove || _isStopOtherThanProsses)
        {
            //減速処理のみ行う
            _nowSpeed = _nowSpeed > 0 ? SpeedDown(_nowSpeed, _deceleration) : _nowSpeed;
            //速度を０にする
            _nowSpeed = _isStopOtherThanProsses ? 0 : _nowSpeed;
            return;
        }

        //2Dゲームの為y軸の入力は無効
        _move.y = 0;

        //入力のあった方向を取得し、移動方向と同じなら速度計算のみ行う
        _directionOfInput = _move.x > 0 ? Input_Right : Input_Left;
        if (_directionOfMovement == _directionOfInput)
        {
            _nowSpeed = SpeedUp(_nowSpeed, _acceleration, _maxSpeed);
            return;
        }

        //方向転換を行う 移動速度が０でない場合は減速を行い、０になれば向きを切り替える
        if (_nowSpeed > 0)
        {
            //初回のみ処理
            if (!_isInertiaPlosses)
            {
                _isInertiaPlosses = true;
                //Animator.SetTrigger("Turn");
            }
            _nowSpeed = SpeedDown(_nowSpeed, _switchDirDeceleration);

            return;
        }

        //直前まで減速処理を行っていた場合はターンアニメーション不要
        if (!_isInertiaPlosses)
        {
            //Animator.SetTrigger("Turn");
        }
        _isInertiaPlosses = false;

        //向き情報の切り替えと方向転換
        _directionOfMovement = _directionOfInput;
        StartCoroutine(Turn());
    }
    //方向転換を行う
    private IEnumerator Turn()
    {
        // 秒数(waitTime)秒後に秒数(updateSecond)秒あたり角度(updateAngle)度、回数(updateCount)回だけ回転させる
        // ターンアニメーションの再生を行う為 指定秒数待つ 
        //yield return new WaitForSeconds(TurnWaitTime);  実装見合わせ

        //方向転換を行う
        for (int i = 0; i < updateCount; i++)
        {
            transform.Rotate(0f, updateAngle, 0f);
            yield return new WaitForSeconds(updateSecond);
        }
    }


    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        //制限
        if (_isStopOtherThanProsses || _isMoveLimit) { return; }

        if (_isInputJump)
        {
            //入力のあった時点でまだ地面についていなければ処理を行わない
            if (!_isJumped && _isJumping) { return; }

            //初回のみ処理
            if (!_isJumped)
            {
                Animator.SetBool("Jump", true);
                _isJumped = true;
                _jumpCountPower = _jumpPower;
                _jumpCountTime = _jumpMaxTime;
            }
            //時間経過で強制的にジャンプ終了
            _jumpCountTime -= Time.deltaTime;
            if (_jumpCountTime < 0)
            {
                _isInputJump = false;
                return;
            }

            //上昇時の減速処理と上昇処理
            _jumpCountPower -= _jumpUpPowerDeceleration;
            transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
        }
        else
        {
            //直前までジャンプ処理が行われていたか
            if (_isJumped)
            {
                Animator.SetBool("Jump", false);
                _isJumped = false;
                _jumpCountTime = _jumpMaxTime;
            }
            //落下に転じる際の減速処理
            if (!_isGround && _jumpCountPower > 0)
            {
                _jumpCountPower -= _jumpLimitPowerDeceleration;
                transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
            }
        }
    }


    /// <summary>
    /// 攻撃処理
    /// </summary>
    // 攻撃のメイン処理
    private void Attack()
    {
        //制限
        if (_isStopOtherThanProsses) { _isInputAttack = false; return; }

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
            _comboDisconnectionTimer = PlayerParameter._comboDisconnectionTimeLimits;
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
                _comboDisconnectionTimer = PlayerParameter._comboDisconnectionTimeLimits;
            }
        }
    }
    //攻撃が敵に当たったときの処理
    private IEnumerator HitDecision()
    {
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
            NewPlayerBaseProcess enemyProsses = enemyObject.GetComponent<NewPlayerBaseProcess>();
            if (enemyProsses == null) { continue; }

            //ダメージを与える処理とアニメーションの再生速度を操作して行うヒットストップとスロー処理を行う
            enemyProsses.TakeDamage(PlayerAttackMotion._attackParameters[_attackParameterNum]._damage);
            enemyProsses.StartCoroutine(AnimationSpeedChageProcessing(_attackParameterNum));

            //ダメージが単体にしか入らない場合は最初に当たった敵にのみ処理を行い、残りは処理を飛ばす
            if (!PlayerAttackMotion._attackParameters[_attackParameterNum]._attackTarget_Plural) { break; }
        }
        //自分自身にもアニメーションの再生速度を操作して行うヒットストップとスロー処理
        StartCoroutine( AnimationSpeedChageProcessing(_attackParameterNum));


        //クールタイムを取得、０の場合は１回当たり判定を取ったらそのアニメーションの再生が終了するまで当たり判定を停止する
        float decisionCoolTime = PlayerAttackMotion._attackParameters[_attackParameterNum]._decisionCoolTime;
        if (decisionCoolTime == 0)
        {
            //アニメーションクリップの長さから現在までのアニメーションの再生経過時間を引き、残りの時間当たり判定を停止する
            float animClipLength = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            decisionCoolTime = animClipLength - animClipLength * Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        //クールタイムの開始
        yield return new WaitForSeconds(decisionCoolTime + PlayerAttackMotion._attackParameters[_attackParameterNum]._hitStopTime);
        _isStopHitDecisionCoolTime = false;
    }


    /// <summary>
    /// ブリンク処理
    /// </summary>
    private void Blink()
    {
        if (!_isStopInputBlink) { return; }

        //移動系処理
        print("a");
    }
    private IEnumerator BlinkMove()
    {
        _isStopOtherThanProsses = true;
        _isStopInputBlink = true;

        yield return new WaitForSeconds(_blinkLimitTime);

        _isStopInputBlink = false;
        _isStopOtherThanProsses = false;
    }


    /// <summary>
    /// オイル処理
    /// </summary>
    // オイルのメイン処理
    private void InsertOil()
    {
        if (!_isStopInputBlink) { return; }
        _isStopInputBlink = false;
        print("OIL");
    }


    //--------------------------------------------------//
    #endregion

}
