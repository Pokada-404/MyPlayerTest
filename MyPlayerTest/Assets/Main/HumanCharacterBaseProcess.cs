// ---------------------------------------------------------
// HumanCharacterBaseProcess.cs
//
// �쐬��:2023/4/30
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̈ꕔ�̏������܂Ƃ߂��X�N���v�g
//
// ---------------------------------------------------------

#region ���@�ڍ�

/* *
* 
* �q���[�}���^�̃L�����N�^�[�̃x�[�X�ƂȂ鏈��
* �p����œ��͏����ƒǉ��ŕK�v�ȏ����������āA���̒��̃��\�b�h���Ăяo���Ďg�p���Ă��������B
*
* �`�` �U�����[�V�����̒ǉ����@ �`�`
* �@���������X�N���v�^�u���I�u�W�F�N�g"STO_NewPlayerParameter"���J��
* �A�v���C���[�̃p�����[�^�̍���"�퓬"�̒��̃��X�g"Attack Parameters"�������A�u+�v�������ĐV�������[�V������ǉ�����
* �B�ǉ��������[�V�����Ƀp�����[�^��ݒ肷��(�A�j���[�V�������̓N���b�v���Ɠ����ɂ��Ă�������)
* �C�A�j���[�V�����ɕϐ���ǉ�
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
public class HumanCharacterBaseProcess : MonoBehaviour
{
    #region �� �ϐ�

    // *--- �N���X�Q�� -----------------------------------*

    //�X�N���v�^�u���I�u�W�F�N�g
    [SerializeField, Header("�X�e�[�^�X�f�[�^")]
    protected STO_CharacterParameter CharacterParameter;
    [SerializeField, Header("�U�����[�V�����f�[�^")]
    protected STO_AttackMotionParameter AttackMotion;
    protected Animator Animator;
    protected MyRigitBody MyRigitBody;

    // *--------------------------------------------------*


    // *--- �L�����N�^�[�̊�{�������� -------------------*
    [SerializeField]
    /// <summary> ���݂�HP </summary>
    protected float _nowHP = default;
    /// <summary> ���ׂĂ̍U���A�j���[�V�����N���b�v�����i�[����z�� </summary>
    protected string[] _attackParameterNames = default;
    /// <summary> �㉺���E�ړ� </summary>
    protected Vector2 _move = default;
    /// <summary> �L�����N�^�[�����݈ړ����Ă������ </summary>
    protected int _directionOfMovement = default;
    /// <summary> �L�����N�^�[�ɐV���Ɉړ������������� </summary>
    protected int _directionOfInput = default;
    /// <summary> ���݂̈ړ����x </summary>
    protected float _nowSpeed = default;
    /// <summary> �W�����v���J�n���Ă���o�߂������� </summary>
    protected float _jumpCountTime = default;
    /// <summary> ���݂̃W�����v�� </summary>
    protected float _jumpCountPower = default;
    /// <summary> ���݂̘A���R���{�� </summary>
    protected int _comboCounter = default;
    /// <summary> �U���I����R���{���r�؂��܂ł̎c�莞�� </summary>
    protected float _comboDisconnectionTimer = default;
    protected string _attackNowPlayAnimClipName = default;
    protected int _attackParameterNum = default;
    protected float _stopMotionTime;
    protected float _slowMotionTime;
    protected float _slowMotionLevel;
    //�U���͂̔{��
    protected float _damageMagnification = 1;
    //�G�̃��X�g�擾
    protected List<GameObject> _enemyList = default;
    //�u�����N��Ԃ̎��Ԑ���
    protected float _blinkLimitTime = default;
    //�u�����N��̃N�[���^�C��
    protected float _blinkReCastTime = default;
    //�I�C���g�p��Ԃ̎��Ԑ���
    protected float _OILLimitTime = default;
    //�I�C���g�p��̃N�[���^�C��
    protected float _OILReCastTime = default;


    protected float _gravityValue = default;

    // *--------------------------------------------------*


    // *--- �t���O ---------------------------------------*

    // �v���C���[�̑S������~�p
    protected bool SystemStop = false;
    //�����蔻��擾
    protected bool _isGround = default;
    protected bool _isCeiling = default;
    protected bool _isWallDecision = default;
    protected bool _isHitEnemy = default;
    //���͂̎擾�p
    protected bool _isInputMove = default;
    protected bool _isInputUpButton = default;
    protected bool _isInputDownButton = default;
    protected bool _isInputRightButton = default;
    protected bool _isInputLeftButton = default;
    protected bool _isInputJump = default;
    protected bool _isInputAttack = default;
    protected bool _isInputBlink = default;
    protected bool _isInputOIL = default;
    //����ȋ����������s�����ߑ��̏����𐧌����� �I�C�������ƃu�����N�Ŏg�p
    protected bool _isBlink = default;
    protected bool _isOIL = default;
    //�U���̓����蔻��N�[���^�C����
    protected bool _isStopHitDecisionCoolTime = default;
    //�W�����v�����̏���̂ݍs�����������̍ۂɎg��
    protected bool _isJumped = default;
    //�W�����v���Ɉړ����͂��s���Ă��Ȃ���
    protected bool _isJumpInertia = default;

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
        _stopMotionTime = AttackMotion._attackParameters[attackParameterNum]._hitStopTime;
        _slowMotionTime = AttackMotion._attackParameters[attackParameterNum]._hitSlowTime;
        _slowMotionLevel = AttackMotion._attackParameters[attackParameterNum]._hitSlowLevel;

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
        
        if (DamageValue < CharacterParameter._minDamageValue)
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
        _nowHP = CharacterParameter._maxHP <= RecoveryValue ? CharacterParameter._maxHP : _nowHP + RecoveryValue;
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
        _nowHP = CharacterParameter._maxHP;
        _comboDisconnectionTimer = CharacterParameter._comboDisconnectionTimeLimits;


        for (int i = 0; i < AttackMotion._attackParameters.Count; i++)
        {
            _attackParameterNames[i] = AttackMotion._attackParameters[i]._AnimationClipName;
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
        _jumpCountTime =  CharacterParameter._jumpMaxTime;
        _isStopHitDecision = true;
        _attackParameterNames = new string[AttackMotion._attackParameters.Count];
    }

    /// <summary>
    /// �ϐ��̏���������
    /// </summary>
    protected void ResetVariable()
    {
    }

    public float SetGravity { set { _gravityValue = value; } }


    /// <summary>
    /// Trigger�������A�j���[�V�����̃p�����[�^�̐ݒ菈��
    /// </summary>
    protected void AnimationUpdate()
    {
        Animator.SetFloat("MoveSpeed", _nowSpeed / CharacterParameter._defaultMaxSpeed);
        Animator.SetBool("IsGround", _isGround);
        Animator.SetFloat("MoveDirection", _directionOfMovement);
    }

    #endregion

}
