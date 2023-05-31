// ---------------------------------------------------------
// STO_CharacterParameter.cs
//
// �쐬��:2023/4/30
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̃X�e�[�^�X�����X�N���v�^�u���I�u�W�F�N�g 
//
// ---------------------------------------------------------

#region ���@�ڍ�

/* *
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

[CreateAssetMenu(menuName = "Parameter/CharacterParameter", fileName = "NewCharacterParameter")]
public class STO_CharacterParameter : ScriptableObject
{
    [Header("- �ڍאݒ� -\n�X�e�[�^�X")]
    [Label("�ő�HP")]
    public float _maxHP = default;

    [Header("������")]
    #region �� �d�l����
    /*
     *������
     *�{�^���������Ɩ��������ł���
     *��������HP����芄���g�p���邱�ƂŔ\�͂��ꎞ�I�ɋ��������Ƃ�������
     *���������\�͈͂ȉ��̒ʂ�
     *�E�󒆈ړ��񐔂̑���
     *�E�ړ����x�㏸
     *�E�u�����N���ړ��\�͈͂̊g��
     *�E�U�����x�㏸
     *�E
     *�I�C���Q�[�W�͈�芄���������Ă����A0�ɂȂ�ƒʏ�̔\�͒l�ɂ��ǂ�B
     */
    #endregion
    [Label("�������Ɏg��HP����(��)"), Range(0, 100)]
    public float _insertOilUseHpRatio = default;
    [Label("�I�C���Q�[�W�̂P�b���̌�������(��)"), Range(0, 100)]
    public float _oilDecreaceRatio = default;

    //�ړ��֘A
    [Header("- �ړ��֘A -\n�ʏ펞�n��")]
    [Label("�ړ����x(m/s)")]
    public float _defaultMaxSpeed = default;
    [Label("�ړ������x(m/s)")]
    public float _defaultAcceleration = default;
    [Label("�ړ������x(m/s)")]
    public float _defaultDeceleration = default;
    [Label("�����]�������x(m/s)")]
    public float _defaultSwitchDirDeceleration = default;

    //[Header("�ʏ펞��")]
    //[Label("�ړ����x(m/s)")]
    //public float _defaultAirMaxSpeed = default;
    //[Label("�ړ������x(m/s)")]
    //public float _defaultAirAcceleration = default;
    //[Label("�ړ������x(m/s)")]
    //public float _defaultAirDeceleration = default;
    //[Label("�����]�������x(m/s)")]
    //public float _defaultAirSwitchDirDeceleration = default;
    //[Label("�ړ��񐔏��")]
    //public int _defaultAirWalkUsageLimit = default;

    //[Header("�I�C���g�p���n��")]
    //[Label("�ړ����x(m/s)")]
    //public float _insertOilMaxSpeed = default;
    //[Label("�ړ������x(m/s)")]
    //public float _insertOilAcceleration = default;
    //[Label("�ړ������x(m/s)")]
    //public float _insertOilDeceleration = default;
    //[Label("�����]�������x(m/s)")]
    //public float _insertSwitchDirDeceleration = default;

    //[Header("�I�C���g�p����")]
    //[Label("�ړ����x(m/s)")]
    //public float _insertOilAirMaxSpeed = default;
    //[Label("�ړ������x(m/s)")]
    //public float _insertOilAirAcceleration = default;
    //[Label("�ړ������x(m/s)")]
    //public float _insertOilAirDeceleration = default;
    //[Label("�����]�������x(m/s)")]
    //public float _insertOilAirSwitchDirDeceleration = default;
    //[Label("�ړ��񐔏��")]
    //public int _insertOilAirWalkUsageLimit = default;

    [Header("�W�����v ")]
    #region �� �d�l����
    /*
     *�W�����v
     *�{�^���������ƃW�����v���ł���
     *�{�^���̉�����Ă��钷���ɂ���č������ς��(�X�p�}���̃W�����v���C���[�W���Ă��炤�Ƃ���)
     *�Z�߂̓��͂ƒ��߂̓��͂̃W�����v�̍����̈Ⴂ�Ȃǂ�ݒ肵�Ă���
     */
    #endregion
    [Label("�W�����v��(m/s)")]
    public float _jumpPower;
    [Label("�W�����v�㏸���̌����x(m/s)")]
    public float _jumpUpPowerDeceleration;
    [Label("���x�����ɂ�錸���x(m/s)")]
    public float _jumpLimitPowerDeceleration;
    [Label("�ō����x���B�܂ł̎���(s)")]
    public float _jumpMaxTime;
     
    [Header("�퓬 ")]
    #region �� �d�l����
    /*
     *�퓬
     *�U���͂̓R���{����I�C���g�p���ȊO�͈��
     */
    #endregion
    [Label("�u�����N�g�p��̃N�[���^�C��")]
    public float _blinkReCastTime = default;
    [Label("�I�C���h���b�v��"), Range(1, 5)]
    public int _dropOilAmount = 1; //default�l
    [Label("�I�C���h���b�v�m��"), Range(0, 100)]
    public int _dropOilProbability = default;
    [Label("��_���[�W - ���U���ƂȂ�_���[�W�̒l�̍Œ�l")]
    public float _minDamageValue = default;
    [Label("��_���[�W - ���U�����󂯂����̃m�b�N�o�b�N����(m)")]
    public float _knockBackRange = default;

    [Header("�R���{ ")]
    #region �� �d�l����
    /*
     *�R���{
     *�U�����͌コ��ɍU������͂���ƃR���{���ł���
     *�R���{�͓��͘A�łōی��Ȃ�����
     *�R���{���͍U���͂��ƍU�����J��o�����x���オ��
     */
    #endregion
    [Label("�R���{�ɂ��U���͏㏸ �L��")]
    public bool _isAttackPowerUP = default;
    [Label("�U���͏㏸��(�U���������邽�я㏸)")]
    public float _comboRateOfUp = default;
    [Label("�R���{���U����")]
    public float _comboAttackPower = default;
    [Label("�R���{�p������(s)")]
    public float _comboDisconnectionTimeLimits;

    [Header("�u�����N ")]
    #region �� �d�l����
    /*
     *�u�����N
     */
    #endregion
    [Label("�u�����N ���ʎ���")]
    public float _blinkLimitTime= default;

}
