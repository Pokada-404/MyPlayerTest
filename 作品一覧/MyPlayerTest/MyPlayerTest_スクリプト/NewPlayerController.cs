// ---------------------------------------------------------
// NewPlayerController.cs
//
// �쐬��:2023/4/30
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

public class NewPlayerController : NewPlayerBaseProcess
{
    #region �� ���C������
    // **********************************************************************
    #region ����

    /*
     *���C���̏���
     *�֐��̏����̂܂Ƃ߂ł��B
     */

    #endregion

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

            // *--- �ړ����� ---*
            Move();
            transform.position += new Vector3(_nowSpeed * _directionOfMovement, 0, 0) * Time.deltaTime;

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
    private bool MyRigitBodyUse()
    {
        //�����𑖂点��
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
    /// InputSystem��Action���g�����߂̐ݒ菈��
    /// </summary>
    private void SetActionEvent()
    {
        // Action�X�N���v�g�̃C���X�^���X����
        //PlayerInput = new PlayerInput();
        // Input Action���@�\�����邽�ߗL����
        PlayerInput.Enable();
        // Action�C�x���g�o�^
        PlayerInput.Action.Move_Press.performed += OnMovePress;
        PlayerInput.Action.Move_Release.performed += OnMoveRelease;
        PlayerInput.Action.Jump_Press.performed += OnJumpPress;
        PlayerInput.Action.Jump_Release.performed += OnJumpRelease;
        PlayerInput.Action.Attack.performed += OnAttack;
        PlayerInput.Action.Blink.performed += OnBlink;
        PlayerInput.Action.InsertOil.performed += OnInsertOil;
    }
    /// <summary>
    /// �v���C���[�𑀍삷��f�o�C�X�̏㏑��
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

    // �ݒ�̃��Z�b�g����
    private void OnDestroy()
    {
        // ���g�ŃC���X�^���X������Action�N���X��IDisposable���������Ă���̂ŕK��Dispose����K�v������
        PlayerInput?.Dispose();
    }

    //���E�ړ��{�^������
    private void OnMovePress(InputAction.CallbackContext context)
    {
        // Move�A�N�V�����̓��͎擾
        _move = context.ReadValue<Vector2>();
        _isInputMove = true;
    }

    //���E�ړ��{�^������
    private void OnMoveRelease(InputAction.CallbackContext context)
    {
        // Move�A�N�V�����̓��͎擾
        _move = Vector2.zero;
        _isInputMove = false;
    }

    //�W�����v�{�^������
    private void OnJumpPress(InputAction.CallbackContext context)
    {
        //���͂���������Ă��邩
        if (_isMoveLimit) { return; }
        if (!_isGround) { return; }
        //���łɋ󒆂ɂ���Ώ������s��Ȃ�
        _isInputJump = true;
    }

    //�W�����v�{�^������
    private void OnJumpRelease(InputAction.CallbackContext context)
    {
        //�W�����v�������~�߂�
        _isInputJump = false;
    }

    //�U������
    private void OnAttack(InputAction.CallbackContext context)
    {
        _isInputAttack = true;
    }

    //�u�����N����
    private void OnBlink(InputAction.CallbackContext context)
    {
        if (_isStopInputBlink) { return; }
        StartCoroutine(BlinkMove());
    }

    //�I�C������
    private void OnInsertOil(InputAction.CallbackContext context)
    {
        _isInputOIL = true;
    }

    // **********************************************************************
    #endregion


    #region �� �v���C���[����
    //--------------------------------------------------//
    #region ����

    /*
     * �v���C���[����
     * 
     */

    #endregion

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        //��������
        if (_isMoveLimit || !_isInputMove || _isStopOtherThanProsses)
        {
            //���������̂ݍs��
            _nowSpeed = _nowSpeed > 0 ? SpeedDown(_nowSpeed, _deceleration) : _nowSpeed;
            //���x���O�ɂ���
            _nowSpeed = _isStopOtherThanProsses ? 0 : _nowSpeed;
            return;
        }

        //2D�Q�[���̈�y���̓��͖͂���
        _move.y = 0;

        //���͂̂������������擾���A�ړ������Ɠ����Ȃ瑬�x�v�Z�̂ݍs��
        _directionOfInput = _move.x > 0 ? Input_Right : Input_Left;
        if (_directionOfMovement == _directionOfInput)
        {
            _nowSpeed = SpeedUp(_nowSpeed, _acceleration, _maxSpeed);
            return;
        }

        //�����]�����s�� �ړ����x���O�łȂ��ꍇ�͌������s���A�O�ɂȂ�Ό�����؂�ւ���
        if (_nowSpeed > 0)
        {
            //����̂ݏ���
            if (!_isInertiaPlosses)
            {
                _isInertiaPlosses = true;
                //Animator.SetTrigger("Turn");
            }
            _nowSpeed = SpeedDown(_nowSpeed, _switchDirDeceleration);

            return;
        }

        //���O�܂Ō����������s���Ă����ꍇ�̓^�[���A�j���[�V�����s�v
        if (!_isInertiaPlosses)
        {
            //Animator.SetTrigger("Turn");
        }
        _isInertiaPlosses = false;

        //�������̐؂�ւ��ƕ����]��
        _directionOfMovement = _directionOfInput;
        StartCoroutine(Turn());
    }
    //�����]�����s��
    private IEnumerator Turn()
    {
        // �b��(waitTime)�b��ɕb��(updateSecond)�b������p�x(updateAngle)�x�A��(updateCount)�񂾂���]������
        // �^�[���A�j���[�V�����̍Đ����s���� �w��b���҂� 
        //yield return new WaitForSeconds(TurnWaitTime);  ���������킹

        //�����]�����s��
        for (int i = 0; i < updateCount; i++)
        {
            transform.Rotate(0f, updateAngle, 0f);
            yield return new WaitForSeconds(updateSecond);
        }
    }


    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump()
    {
        //����
        if (_isStopOtherThanProsses || _isMoveLimit) { return; }

        if (_isInputJump)
        {
            //���͂̂��������_�ł܂��n�ʂɂ��Ă��Ȃ���Ώ������s��Ȃ�
            if (!_isJumped && _isJumping) { return; }

            //����̂ݏ���
            if (!_isJumped)
            {
                Animator.SetBool("Jump", true);
                _isJumped = true;
                _jumpCountPower = _jumpPower;
                _jumpCountTime = _jumpMaxTime;
            }
            //���Ԍo�߂ŋ����I�ɃW�����v�I��
            _jumpCountTime -= Time.deltaTime;
            if (_jumpCountTime < 0)
            {
                _isInputJump = false;
                return;
            }

            //�㏸���̌��������Ə㏸����
            _jumpCountPower -= _jumpUpPowerDeceleration;
            transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
        }
        else
        {
            //���O�܂ŃW�����v�������s���Ă�����
            if (_isJumped)
            {
                Animator.SetBool("Jump", false);
                _isJumped = false;
                _jumpCountTime = _jumpMaxTime;
            }
            //�����ɓ]����ۂ̌�������
            if (!_isGround && _jumpCountPower > 0)
            {
                _jumpCountPower -= _jumpLimitPowerDeceleration;
                transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
            }
        }
    }


    /// <summary>
    /// �U������
    /// </summary>
    // �U���̃��C������
    private void Attack()
    {
        //����
        if (_isStopOtherThanProsses) { _isInputAttack = false; return; }

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
            _comboDisconnectionTimer = PlayerParameter._comboDisconnectionTimeLimits;
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
                _comboDisconnectionTimer = PlayerParameter._comboDisconnectionTimeLimits;
            }
        }
    }
    //�U�����G�ɓ��������Ƃ��̏���
    private IEnumerator HitDecision()
    {
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
            NewPlayerBaseProcess enemyProsses = enemyObject.GetComponent<NewPlayerBaseProcess>();
            if (enemyProsses == null) { continue; }

            //�_���[�W��^���鏈���ƃA�j���[�V�����̍Đ����x�𑀍삵�čs���q�b�g�X�g�b�v�ƃX���[�������s��
            enemyProsses.TakeDamage(PlayerAttackMotion._attackParameters[_attackParameterNum]._damage);
            enemyProsses.StartCoroutine(AnimationSpeedChageProcessing(_attackParameterNum));

            //�_���[�W���P�̂ɂ�������Ȃ��ꍇ�͍ŏ��ɓ��������G�ɂ̂ݏ������s���A�c��͏������΂�
            if (!PlayerAttackMotion._attackParameters[_attackParameterNum]._attackTarget_Plural) { break; }
        }
        //�������g�ɂ��A�j���[�V�����̍Đ����x�𑀍삵�čs���q�b�g�X�g�b�v�ƃX���[����
        StartCoroutine( AnimationSpeedChageProcessing(_attackParameterNum));


        //�N�[���^�C�����擾�A�O�̏ꍇ�͂P�񓖂��蔻���������炻�̃A�j���[�V�����̍Đ����I������܂œ����蔻����~����
        float decisionCoolTime = PlayerAttackMotion._attackParameters[_attackParameterNum]._decisionCoolTime;
        if (decisionCoolTime == 0)
        {
            //�A�j���[�V�����N���b�v�̒������猻�݂܂ł̃A�j���[�V�����̍Đ��o�ߎ��Ԃ������A�c��̎��ԓ����蔻����~����
            float animClipLength = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            decisionCoolTime = animClipLength - animClipLength * Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        //�N�[���^�C���̊J�n
        yield return new WaitForSeconds(decisionCoolTime + PlayerAttackMotion._attackParameters[_attackParameterNum]._hitStopTime);
        _isStopHitDecisionCoolTime = false;
    }


    /// <summary>
    /// �u�����N����
    /// </summary>
    private void Blink()
    {
        if (!_isStopInputBlink) { return; }

        //�ړ��n����
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
    /// �I�C������
    /// </summary>
    // �I�C���̃��C������
    private void InsertOil()
    {
        if (!_isStopInputBlink) { return; }
        _isStopInputBlink = false;
        print("OIL");
    }


    //--------------------------------------------------//
    #endregion

}
