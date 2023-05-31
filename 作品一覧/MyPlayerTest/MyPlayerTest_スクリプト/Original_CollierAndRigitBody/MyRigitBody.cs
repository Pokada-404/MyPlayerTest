// ---------------------------------------------------------
// MyRigitBody.cs
//
// �쐬��:2023/5/10
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̋�������p�X�N���v�g
//
// ---------------------------------------------------------

#region ���@�ڍ�

/*
 * 
 *�d�͂��Č�����X�N���v�g
 *�Q�Ƃ���N���X�͂��̃X�N���v�g�����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g(���I�u�W�F�N�g���܂�)
 *�ɂ��Ă���X�N���v�g"MyCollider"�̂݁B
 *
 *
 *
 */

#endregion

#region ���@Using

using System.Collections.Generic;
using UnityEngine;

#endregion 

public class MyRigitBody : MonoBehaviour
{
    #region �� �ϐ�

    //�d��
    private const float _gravity = 10;
    // ���C�̒���
    private const float RaycastLength = 1.5f;
    //�߂荞�񂾎��̉����߂���
    private const float CorrectionForce = 30f;
    //����script�����Ă���I�u�W�F�N�g�̌��_���"RaycastOriginCorrection"���������Ɍ������Ĕ��˂���܂�
    private const float RaycastOriginCorrection = 0.05f;
    //�n�ʂɂ������␳�Œn�ʂ��"PositionYCorrection"����ɂ��炳��܂��B
    private const float PositionYCorrection = 0.03f;
    private const float DecisionCorrection = 0.05f;
    //�n�ʂƂ̐ڐG����p���C
    private Vector2 _rayCastOriginPos = default;

    //�t���O�Ȃ�
    MyColliderTrigger[] _myColliderTrigger;
    private bool _isStopGravity = default;
    private bool _isPenetrationGround = default;
    private bool _isPenetrationCeiling = default;
    private bool _isPenetrationRightWall = default;
    private bool _isPenetrationLeftWall = default;
    private bool _isEnemy = default;
    private List<GameObject> _gameObjectList = new List<GameObject>();

    #endregion

    private void Awake()
    {
        //�q�I�u�W�F�N�g��ɃA�^�b�`����Ă��鎩��Trigger�X�N���v�g���擾
        _myColliderTrigger = GetComponentsInChildren<MyColliderTrigger>();
    }

    public void RigitBodyUpdate()
    {
        _isStopGravity = false;
        _isPenetrationGround = false;
        _isPenetrationCeiling = false;
        _isPenetrationRightWall = false;
        _isPenetrationLeftWall = false;
        _isEnemy = false;
        _gameObjectList.Clear();

        foreach (MyColliderTrigger trigger in _myColliderTrigger)
        {
            trigger.ColliderUpdate();

            //�擾�����^�O���瓖�������I�u�W�F�N�g�̔��ʂ��s��
            if (!_isPenetrationGround) { _isPenetrationGround = trigger.SetPenetrationGroundDecision; }
            if (!_isPenetrationCeiling) { _isPenetrationCeiling = trigger.SetPenetrationCeilingDecision; }
            if (!_isPenetrationRightWall) { _isPenetrationRightWall = trigger.SetPenetrationRightWallDecision; }
            if (!_isPenetrationLeftWall) { _isPenetrationLeftWall = trigger.SetPenetrationLeftWallDecision; }
            if (!_isEnemy) { _isEnemy = trigger.SetEnemyDecision; }
            if (trigger.SetEnemyDecision)
            {
                foreach (GameObject EnemyObject in trigger.SetEnemyGameObject)
                {
                    //���X�g�ɓ����I�u�W�F�N�g�������Ă��Ȃ���Βǉ�
                    if (!_gameObjectList.Contains(EnemyObject))
                    {
                        _gameObjectList.Add(EnemyObject);
                    }
                }
            }

            //�ڐG����̃t���O�����ɖ߂�
            trigger.SetPenetrationGroundDecision = false;
            trigger.SetPenetrationCeilingDecision = false;
            trigger.SetPenetrationRightWallDecision = false;
            trigger.SetPenetrationLeftWallDecision = false;
            trigger.SetEnemyDecision = false;
        }
        //�n�ʂƂ̐ڐG��������
        _rayCastOriginPos = new Vector2(transform.position.x, transform.position.y - RaycastOriginCorrection);
        RaycastHit2D[] hit = Physics2D.RaycastAll(_rayCastOriginPos, Vector3.up, RaycastLength);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.tag == "Stage")
            {
                _isStopGravity = true;
                transform.position = new Vector3(transform.position.x,
                                                 hit[i].transform.position.y + hit[i].collider.offset.y + hit[i].collider.bounds.size.y / 2 + PositionYCorrection,
                                                 transform.position.z);
                break;
            }
        }

        //�ڐG�t���O�ɂ����W�ړ�����
        if (!_isStopGravity)
        {
            transform.position += Vector3.down * _gravity * Time.deltaTime;
        }
        if (_isPenetrationGround)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += DecisionCorrection;
            transform.position = newPosition;
        }
        if (_isPenetrationCeiling)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= DecisionCorrection;
            transform.position = newPosition;
        }
        if (_isPenetrationRightWall)
        {
            Vector3 newPosition = transform.position;
            newPosition.x -= DecisionCorrection;
            transform.position = newPosition;
        }
        if (_isPenetrationLeftWall)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += DecisionCorrection;
            transform.position = newPosition;
        }
    }

    public bool SetGroundDecision { get { return _isStopGravity; } }
    public bool SetCeilingDecision { get { return _isPenetrationCeiling; } }
    public bool SetWallDecision { get { return (_isPenetrationRightWall || _isPenetrationLeftWall) ? true : false; } }
    public bool SetEnemyDecision { get { return _isEnemy; } }
    public List<GameObject>  SetEnemyObjectList { get { return _gameObjectList; } }

}
