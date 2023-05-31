// ---------------------------------------------------------
// NewPlayerBaseProcess.cs
//
// �쐬��:2023/4/30
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̈ꕔ�̏������܂Ƃ߂��X�N���v�g
//
// ---------------------------------------------------------

#region ���@�ڍ�

/* *
* 
* �v���C���[�Ɋւ��鏈��
*
* �`�` �U�����[�V�����̒ǉ����@ �`�`
* �@���������X�N���v�^�u���I�u�W�F�N�g"STO_NewPlayerParameter"���J��
* �A�v���C���[�̃p�����[�^�̍���"�퓬"�̒��̃��X�g"Attack Parameters"�������A�u+�v�������ĐV�������[�V������ǉ�����
* �B�ǉ��������[�V�����Ƀp�����[�^��ݒ肷��
* �C���̃X�N���v�g�̒���Enum"AttackAnimClip"�ɐݒ肵�����[�V�������Œǉ�
* �D�U���A�j���[�V�����̎w��p�̒萔�ɓ������ݒ肵�����[�V�������Œǉ�
* �E�X�N���v�g"NewPlayerController"���J���AHitDecision()�̒���switch����case��ǉ�
* �ȏ�B
* 
* 
* */

#endregion

#region ���@Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

#endregion
public class NewPlayerBaseProcess : MonoBehaviour
{
    #region �� �ϐ�
    //�c��J�X
    /// <summary>
    /// ���G���t���O
    /// </summary>
    protected bool _isInvincible = default;
    /// <summary>
    /// ���S�t���O
    /// </summary>
    protected bool _isDead = default;


    // *--- �N���X�Q�� -----------------------------------*

    [SerializeField, Header("�v���C���[�X�e�[�^�X�f�[�^(�X�N���v�^�u���I�u�W�F�N�g)")]
    protected STO_CharacterParameter PlayerParameter;
    [SerializeField, Header("�v���C���[�U�����[�V�����f�[�^(�X�N���v�^�u���I�u�W�F�N�g)")]
    protected STO_AttackMotionParameter PlayerAttackMotion;
    protected Animator Animator;
    protected PlayerInput PlayerInput;
    protected MyRigitBody MyRigitBody;

    // *--------------------------------------------------*


    // *--- �v���C���[���������֘A -----------------------*

    //�ړ�
    protected Vector2 _move = default;
    protected int _directionOfMovement = default;
    protected int _directionOfInput = default;
    protected float _nowSpeed = default;
    //�W�����v
    protected float _jumpCountTime = default;
    protected float _jumpCountPower = default;
    //�U��
    protected int _comboCounter = default;
    protected float _comboDisconnectionTimer = default;
    protected string _attackNowPlayAnimClipName = default;
    protected int _attackParameterNum = default;
    protected float _stopMotionTime;
    protected float _slowMotionTime;
    protected float _slowMotionLevel;
    //�G�̃��X�g�擾
    protected List<GameObject> _enemyList = default;

    // *--------------------------------------------------*


    // *--- �t���O ---------------------------------------*

    //�����蔻��擾
    protected bool _isGround = default;
    protected bool _isCeiling = default;
    protected bool _isWallDecision = default;
    protected bool _isHitEnemy = default;
    //���͂̎擾�p
    protected bool _isInputMove = default;
    protected bool _isInputJump = default;
    protected bool _isInputAttack = default;
    protected bool _isInputOIL = default;
    // �v���C���[�̑S������~�p (�R���[�`���ɂ�鎞�ԍ������͏���)
    protected bool SystemStop = false;
    //����ȋ����������s�����ߑ��̏����𐧌����� �I�C�������ƃu�����N�Ŏg�p
    protected bool _isStopOtherThanProsses = default;
    //�U���̓����蔻��N�[���^�C����
    protected bool _isStopHitDecisionCoolTime = default;
    //�u�����N�̓��͐���
    protected bool _isStopInputBlink = default;
    //�^�������̏�������̂ݓ���p
    protected bool _isInertiaPlosses = default;
    //�W�����v�ς��ǂ����̔���p
    protected bool _isJumped = default;

    // *--------------------------------------------------*


    // *--- AnimationClip���瑀�� ------------------------*

    /// <summary>
    /// �U���A�j���[�V��������A�C�h���ւ̑J�ڌ��m�p �U���I����ɂ��̃t���O��true�ɂȂ�
    /// </summary>
    [SerializeField,HideInInspector]
    protected bool _isAttackToIdle = default;
    /// <summary>
    /// �W�����v���̏��������p �W�����v���ɂ��̃t���O��true�ɂȂ�
    /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isJumping = default;
    /// <summary>
    /// �ړ������̐����p �U�����ɂ��̃t���O��true�ɂȂ�
    /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isMoveLimit = default;
    /// <summary>
    /// �U�����͂̒�~�p true�̊ԍU�����͂��擾���Ȃ�
    /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isStopAttackInput = default;
    /// <summary>
    /// �U���̓����蔻�菈���̐����p true�̊ԓ����蔻����擾���Ȃ�
    /// /// </summary>
    [SerializeField, HideInInspector]
    protected bool _isStopHitDecision = default;

    // *--------------------------------------------------*


    // *--- �X�N���v�^�u���I�u�W�F�N�g����擾����ϐ� ---*

    [SerializeField]//�f�o�b�O��Ɨp�ɃV���A���C�Y
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
    protected string[] _attackParameterNames = new string[10];   //���ׂĂ̍U���A�j���[�V�����N���b�v�����i�[����z�� �A�j���[�V�����N���b�v��������Ώ�������Ă�����

    // *--------------------------------------------------*


    // *--- �萔 -----------------------------------------*

    //�ړ������Ŏg�p��������ݒ�p
    protected const int Input_Right = 1;
    protected const int Input_Left = -1;
    //�A�j���[�V�����̃A�C�h���A�j���[�V�����w��p
    protected const string IdleAnimation = "Idle";
    protected const string MoveAnimation = "Move";
    //�A�j���[�V������Layer�w��p
    protected const int TakeDamageLayer = 1;
    protected const float MaxWeight = 1;
    // �b��(waitTime)�b��ɕb��(updateSecond)�b������p�x(updateAngle)�x�A��(updateCount)�񂾂���]������
    protected const int updateAngle = 15;
    protected const int updateCount = 12;
    protected const float updateSecond = 0.01f;
    //�����]�����Ƀ^�[���A�j���[�V�����̍Đ����s���� �w��b���҂��Ă���]�����s�����߂Ɏg�p
    protected const float TurnWaitTime = 0; //���݃^�[���A�j���[�V�����͓���Ă��Ȃ��ׂO�ɂ��Ă���B�폜�\��

    // *--------------------------------------------------*

    #endregion

    //�������e�ʃ��]�b�g�܂Ƃ�

    #region �� �X�g�b�v�E�X���[����
    #region ����
    /*
     * 
     * �_���[�W��^����A�󂯂��Ƃ��̓����̃X�g�b�v��X���[���[�V����
     * �Q�[���S�̂̃^�C���X�P�[���𓮂����Ď�������B
     * �������@�͈ȉ��̕��@���������Ă���B
     * �@�������K�v�ł���ΐ�p�����̃��\�b�h���Ăяo���B���\�b�h�ł͗^����ꂽ���Ԃ����X�g�b�v�E�X���[�������s���B�����̓X�g�b�v�̌�ɃX���[���s����B
     * �@�����������ʂȂ�������������C������A�A
     */
    #endregion

    /// <summary>
    /// �����̃X�g�b�v��X���[���[�V��������
    /// </summary>
    /// <param name="attackParameterNum">�U���̃p�����[�^���w���ϐ�</param>
    public IEnumerator AnimationSpeedChageProcessing(int attackParameterNum)
    {
        _stopMotionTime = PlayerAttackMotion._attackParameters[attackParameterNum]._hitStopTime;
        _slowMotionTime = PlayerAttackMotion._attackParameters[attackParameterNum]._hitSlowTime;
        _slowMotionLevel = PlayerAttackMotion._attackParameters[attackParameterNum]._hitSlowLevel;

        if (_stopMotionTime != 0)
        {
            SystemStop = true;
            Animator.speed = 0;

            //�w��t���[�����������s��
            yield return new WaitForSeconds(_stopMotionTime);
        }
        if (_slowMotionTime != 0)
        {
            SystemStop = true;
            Animator.speed = _slowMotionLevel;

            //�w��t���[�����������s��
            yield return new WaitForSeconds(_slowMotionTime);
        }
        Animator.speed = 1;
        SystemStop = false;
    }

    #endregion

    #region �� �v���C���[�����֘A

    /// <summary>
    /// �����x�� �l�𑝉�����
    /// </summary>
    /// <param name="moveSpeed">���݂̈ړ����x</param>
    /// <param name="acceleration">�����x</param>
    /// <param name="maxSpeed">�ő呬�x</param>
    /// <returns></returns>
    protected private float SpeedUp(float moveSpeed, float acceleration, float maxSpeed)
    {
        // �����x���l�����đ��x���㏸������
        // �ő呬�x�𒴂��Ȃ��悤�ɐ�������
        return Mathf.Clamp(moveSpeed + acceleration * Time.deltaTime, 0.0f, maxSpeed);
    }

    /// <summary>
    /// �����x�� �l�����Z����
    /// </summary>
    /// <param name="moveSpeed">���݂̈ړ����x</param>
    /// <param name="deceleration">�����x</param>
    /// <returns></returns>
    protected private float SpeedDown(float moveSpeed, float deceleration)
    {
        // �����x���l�����đ��x������������
        // ���x���O�ȉ��ɂȂ�Ȃ��悤�ɐ�������
        return (moveSpeed -= deceleration * Time.deltaTime) >= 0.05f ? moveSpeed : 0;
    }

    /// <summary>
    /// �_���[�W�̎󂯂鏈���B�U��������Q�Ƃ����B�_���[�W�̒l�ɂ���Đ�p�A�j���[�V�����̍Đ����s���B
    /// </summary>
    /// <param name="DamageValue"></param>
    public void TakeDamage(float DamageValue)
    {
        _nowHP -= DamageValue;
        
        if (DamageValue < _minDamageValue)
        {
            //��U���̔�_���[�W�̃A�j���[�V�������Đ�
            Animator.SetTrigger("TakenDamage_Light");
        }
        else
        {
            SystemStop = true;

            //���U���̔�_���[�W�̃A�j���[�V�������Đ�
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

    #region �� �ϐ��ݒ�
    #region ����
    /*
     * 
     * �ϐ��̐ݒ�Ə��������s�����]�b�g
     * 
     */
    #endregion

    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g����p�����[�^���擾���鏈��
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
    /// �O���[�o���ϐ��̐ݒ菈��
    /// </summary>
    protected void SetValueGlobalVariable()
    {
        //�R���|�[�l���g�擾
        Animator = this.GetComponent<Animator>();
        MyRigitBody = this.GetComponent<MyRigitBody>();
        if (Animator == null)
        {
            Debug.LogError("Animation ���A�^�b�`����Ă��܂���");
            SystemStop = true;
        }
        if (MyRigitBody == null)
        {
            Debug.LogError("MyRigitBody ���A�^�b�`����Ă��܂���");
            SystemStop = true;
        }

        //�t�B�[���h�ϐ��̐ݒ�
        _directionOfMovement = Input_Right;
        _jumpCountTime = _jumpMaxTime;
        _isStopHitDecision = true;
    }

    /// <summary>
    /// �ϐ��̏���������
    /// </summary>
    protected void ResetVariable()
    {
    }

    /// <summary>
    /// Trigger�������A�j���[�V�����̃p�����[�^�̐ݒ菈��
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
