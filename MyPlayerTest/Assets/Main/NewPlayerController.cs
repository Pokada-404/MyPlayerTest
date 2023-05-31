// ---------------------------------------------------------
// NewPlayerController.cs
//
// �쐬��:2023/5/15
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̋�������p�X�N���v�g
//
// ---------------------------------------------------------

#region ���@�ڍ�

/*
* 
* 
* 
* �A�j���[�V�����Ɋւ��Ē��ӓ_
* ���̃X�N���v�g�̓A�j���[�^�[�ƃA�j���[�V�����N���b�v�Ɉˑ����Ă��܂��B
* 
*/

#endregion

#region ���@Using

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
        //�ϐ��̒l�ݒ�
        SetActionEvent();
        SetValueGlobalVariable();
        SetValueFromScriptableObject();
    }

    private void Update()
    {
        //z���W�̌Œ�
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //HP���O�ɂȂ�Ύ��S
        if (_nowHP <= 0) { gameObject.SetActive(false); return; }

        if (!SystemStop)
        {
            // *--- �d�͂̓K�p�Ɠ����蔻��擾���� ---*
            if (MyRigitBodyUse()) { return; }

            // *--- ���͏��� ---*
            GetMoveValue();

            // *--- �ړ����� ---*
            HorizontalMove();

            // *--- �W�����v���� ---*
            Jump();

            // *--- �U������ ---*
            Attack();

            // *--- �u�����N���� ---*
            Blink();

            // *--- �I�C������ ---*
            InsertOil();

            // *--- �ϐ��̏��������� ---*
            ResetVariable();

            // *--- �A�j���[�V�����̃p�����[�^�ݒ� ---*
            AnimationUpdate();
        }
    }


    #region �� ���͎擾����
    // **********************************************************************
    #region ����

    /*
     *InputSystem�œ��͂��擾���Ă���B
     *���\�b�h�̓��e�͏ォ�珇��
     *�EAction���g�����߂̐ݒ�
     *�E�ݒ�̃��Z�b�g
     *�E���͂̌��m�p���\�b�g�Q
     *�ƂȂ��Ă���B
     */

    #endregion

    /// <summary>
    /// �f�o�C�X�ݒ菈��
    /// </summary>
    /// <param name="connectDevice">���蓖�Ă�f�o�C�X</param>
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
    /// ActionMap�ݒ菈��
    /// </summary>
    private void SetActionEvent()
    {
        // Action�C�x���g�o�^
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

    // ���g�ŃC���X�^���X������Action�N���X�͈ȉ��̏������K�{
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


    //�ړ�����
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

    //�W�����v����
    private void OnJumpPress(InputAction.CallbackContext context)
    {
        if (_isMoveLimit || !_isGround || _isBlink || (!_isJumped && _isJumping)) { return; }
        _isInputJump = true;
    }
    private void OnJumpRelease(InputAction.CallbackContext context)
    {
        _isInputJump = false;
    }

    //�U������
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (_isBlink || _isStopAttackInput) { return; }
        _isInputAttack = true;
    }

    //�u�����N����
    private void OnBlink(InputAction.CallbackContext context)
    {
        _isInputBlink = true;
    }

    //�I�C������
    private void OnInsertOil(InputAction.CallbackContext context)
    {
        _isInputOIL = true;
    }

    // **********************************************************************
    #endregion


    #region �� MyRigitBody�ɂ�锻��ƓG���X�g�擾����
    // **********************************************************************
    #region ����

    /*
     * MyRigitBody�ɂ�锻��擾����
     * return��true�Ȃ烁�C���̏�����return(�����������I��)����
     */

    #endregion
    /// <summary>
    /// �d�͂ƐڐG����̎擾
    /// </summary>
    /// <returns>�V���ǂɐڐG���Ă����ꍇtrue��Ԃ�</returns>
    protected bool MyRigitBodyUse()
    {
        //�����𑖂点�� �U�����͏����𑖂点�����Ȃ�
        MyRigitBody.RigitBodyUpdate();
        //����̎擾
        _isGround = MyRigitBody.SetGroundDecision;
        _isHitEnemy = MyRigitBody.SetEnemyDecision;
        _isWallDecision = MyRigitBody.SetWallDecision;
        _isCeiling = MyRigitBody.SetCeilingDecision;
        _enemyList = MyRigitBody.SetEnemyObjectList;
        //�������~�߂�
        if (_isCeiling) { _jumpCountPower = 0; return true; }
        if (_isWallDecision) { return true; }
        return false;
    }

    #endregion


    #region �� �v���C���[����
    //--------------------------------------------------//
    #region ����

    /*
     * �v���C���[����
     * 
     */

    #endregion
    protected void HorizontalMove()
    {
        //��������
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
    /// �ړ�����
    /// </summary>
    protected void Move(float Deceleration, float Acceleration, float MaxSpeed, float AirSwitchDirDeceleration, float SwitchDirDeceleration)
    {
        //��������
        if (_isMoveLimit || !_isInputMove)
        {
            //���������̂ݍs��
            _nowSpeed = _nowSpeed > 0 ? SpeedDown(_nowSpeed, Deceleration) : _nowSpeed;
            //���x���O�ɂ���
            _nowSpeed = _isBlink ? 0 : _nowSpeed;
            return;
        }

        //���͂̂������������擾���A�ړ������Ɠ����Ȃ瑬�x�v�Z�̂ݍs��
        _directionOfInput = _move.x > 0 ? Input_Right : Input_Left;
        if (_directionOfMovement == _directionOfInput)
        {
            _nowSpeed = SpeedUp(_nowSpeed, Acceleration, MaxSpeed);
            return;
        }

        //�����]�����s��
        if (_nowSpeed > 0)
        {
            //�󒆂ƒn��ňقȂ���������������
            _nowSpeed = _isJumping ? SpeedDown(_nowSpeed, AirSwitchDirDeceleration) : //�󒆂̊���
                                     SpeedDown(_nowSpeed, SwitchDirDeceleration); //�n��̊���
            return;
        }

        //�������̐؂�ւ��ƕ����]��
        _directionOfMovement = _directionOfInput;
        StartCoroutine(Turn());
    }
    //�����]�����s��
    protected IEnumerator Turn()
    {
        // �b��(waitTime)�b��ɕb��(updateSecond)�b������p�x(updateAngle)�x�A��(updateCount)�񂾂���]������
        for (int i = 0; i < updateCount; i++)
        {
            transform.Rotate(0f, updateAngle, 0f);
            yield return new WaitForSeconds(updateSecond);
        }
    }


    /// <summary>
    /// �W�����v����
    /// </summary>
    protected void Jump()
    {
        if (_isInputJump)
        {
            //����̂ݏ���
            if (!_isJumped)
            {
                Animator.SetBool("Jump", true);
                _isJumped = true;
                _jumpCountPower = CharacterParameter._jumpPower + _gravityValue;
                _jumpCountTime = CharacterParameter._jumpMaxTime;
            }

            //���Ԍo�߂ŋ����I�ɃW�����v�I��
            _jumpCountTime -= Time.deltaTime;
            if (_jumpCountTime < 0)
            {
                _isInputJump = false;
                return;
            }

            //�㏸���̌��������Ə㏸����
            _jumpCountPower -= CharacterParameter._jumpUpPowerDeceleration * Time.deltaTime;
            transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
        }
        else
        {
            //���O�܂ŃW�����v�������s���Ă�����
            if (_isJumped)
            {
                Animator.SetBool("Jump", false);
                _isJumped = false;
                _jumpCountTime = CharacterParameter._jumpMaxTime;
            }
            //�����ɓ]����ۂ̌�������
            if (!_isGround && _jumpCountPower > 0)
            {
                _jumpCountPower -= CharacterParameter._jumpLimitPowerDeceleration * Time.deltaTime;
                transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
            }
        }
    }


    /// <summary>
    /// �U������
    /// </summary>
    // �U���̃��C������
    protected void Attack()
    {
        //�U���̓��͂�����A�����͂̎�t�������������Ă��Ȃ�
        if (_isInputAttack && !_isStopAttackInput)
        {
            _isInputAttack = false;
            Animator.SetTrigger("Attack");
        }

        //�G�ɍU����������A�������t�ɐ������������Ă��Ȃ�
        if (_isHitEnemy && !_isStopHitDecision)
        {
            //�R���{�p�����Ԃ̃��Z�b�g
            _comboDisconnectionTimer = CharacterParameter._comboDisconnectionTimeLimits;
            //����̃N�[���^�C�����ł͂Ȃ�
            if (!_isStopHitDecisionCoolTime)
            {
                StartCoroutine(HitDecision());
            }
        }

        //�R���{���ɃA�C�h����Ԃ֑J�ڂ����ꍇ��莞�Ԍ�ɃR���{�I��
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
    //�U�����G�ɓ��������Ƃ��̏���
    protected IEnumerator HitDecision()
    {
        if (_isBlink) { yield break; }
        _isStopHitDecisionCoolTime = true;

        //���ݍĐ����̃A�j���[�V���������擾���A�X�N���v�^�u���I�u�W�F�N�g����p�����[�^��T���B�Đ����̃A�j���[�V�������A�C�h���Ȃ珈�����s��Ȃ�
        _attackNowPlayAnimClipName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (_attackNowPlayAnimClipName == IdleAnimation || _attackNowPlayAnimClipName == MoveAnimation) { yield break; }
        for (_attackParameterNum = 0; _attackParameterNames[_attackParameterNum] != _attackNowPlayAnimClipName; _attackParameterNum++) { }

        //�R���{�����J�E���g
        _comboCounter++;
        print("hit : " + _comboCounter);

        //�U���������������ׂĂ̓G�Ƀ_���[�W��^���鏈��
        foreach (GameObject enemyObject in _enemyList)
        {
            HumanCharacterBaseProcess enemyProsses = enemyObject.GetComponent<HumanCharacterBaseProcess>();
            if (enemyProsses == null) { continue; }

            //�_���[�W��^���鏈���ƃA�j���[�V�����̍Đ����x�𑀍삵�čs���q�b�g�X�g�b�v�ƃX���[�������s��
            enemyProsses.TakeDamage(AttackMotion._attackParameters[_attackParameterNum]._damage * _damageMagnification);
            enemyProsses.StartCoroutine(AnimationSpeedChageProcessing(_attackParameterNum));

            //�_���[�W���P�̂ɂ�������Ȃ��ꍇ�͍ŏ��ɓ��������G�ɂ̂ݏ������s���A�c��͏������΂�
            if (!AttackMotion._attackParameters[_attackParameterNum]._attackTarget_Plural) { break; }
        }
        //�������g�ɂ��A�j���[�V�����̍Đ����x�𑀍삵�čs���q�b�g�X�g�b�v�ƃX���[����
        StartCoroutine(AnimationSpeedChageProcessing(_attackParameterNum));


        //�N�[���^�C�����擾�A�O�̏ꍇ�͂P�񓖂��蔻���������炻�̃A�j���[�V�����̍Đ����I������܂œ����蔻����~����
        float decisionCoolTime = AttackMotion._attackParameters[_attackParameterNum]._decisionCoolTime;
        if (decisionCoolTime == 0)
        {
            //�A�j���[�V�����N���b�v�̒������猻�݂܂ł̃A�j���[�V�����̍Đ��o�ߎ��Ԃ������A�c��̎��ԓ����蔻����~����
            float animClipLength = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            decisionCoolTime = animClipLength - animClipLength * Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        //�N�[���^�C���̊J�n
        yield return new WaitForSeconds(decisionCoolTime + AttackMotion._attackParameters[_attackParameterNum]._hitStopTime);
        _isStopHitDecisionCoolTime = false;
    }


    /// <summary>
    /// �u�����N����
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
            //�u�����N����
            _blinkLimitTime -= Time.deltaTime;

            //�ǉ��̈ړ�����
            transform.position += new Vector3(_nowSpeed * _directionOfMovement * CharacterParameter._blinkMoveSpeedmagnification, 0, 0) * Time.deltaTime;

            if (_blinkLimitTime <= 0) 
            { 
                _isBlink = false;
                Animator.SetTrigger("BlinkEnd");
            }
        }
        else if (_blinkReCastTime > 0)
        {
            //�u�����N�N�[���^�C��
            _blinkReCastTime -= Time.deltaTime;
        }
    }


    /// <summary>
    /// �I�C������
    /// </summary>
    // �I�C���̃��C������
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
            //�u�����N����
            _OILLimitTime -= Time.deltaTime;

            print(_move);

            //�ǉ��̈ړ�����
            transform.position += new Vector3(_nowSpeed * _directionOfMovement * CharacterParameter._OILMoveSpeedMagnification, 0, 0) * Time.deltaTime;

            if (_OILLimitTime <= 0)
            {
                _isBlink = false;
            }
        }
        else if (_OILReCastTime > 0)
        {
            //�u�����N�N�[���^�C��
            _OILReCastTime -= Time.deltaTime;
            Animator.speed = 1;
            _damageMagnification = 1;
        }
    }


    //--------------------------------------------------//
    #endregion

}
