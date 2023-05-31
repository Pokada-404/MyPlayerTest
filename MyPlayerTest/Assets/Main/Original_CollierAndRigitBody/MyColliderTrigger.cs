// ---------------------------------------------------------
// MyCollider.cs
//
// �쐬��:2023/5/10 
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̋�������p�X�N���v�g
//
// ---------------------------------------------------------

#region ���@�ڍ�

/* 
 * �I- ���� -�I
 * �X�N���v�g"MyRigitBody"�͂��̃I�u�W�F�N�g�̈�ԏ�̃I�u�W�F�N�g�ɒ����Ă���K�v������܂��B
 *
 * �X�N���v�g"MyRigitBody"����̂݌Ăяo����܂��B
 */

#endregion

#region ���@Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class MyColliderTrigger : MonoBehaviour
{

    #region �� �ϐ�

    // BoxCast�̃T�C�Y�l�ƃI�t�Z�b�g�l
    private Vector2 _boxCastSize = default;
    private Vector2 _boxCastOffset = default;

    [Header("�G����ݒ�")]
    [SerializeField, Label("�G����^�O��")] //���̃^�O�����I�u�W�F�N�g��G�Ƃ݂Ȃ��܂�
    private List<string> _enemyTag = default;
    [Header("���薳���ݒ�")]
    [SerializeField, Label("�X�e�[�W�̐ڐG���薳��")]
    private bool _isStopDecision_Stage = default;
    [SerializeField, Label("�G�̐ڐG���薳��")]
    private bool _isStopDecision_Enemy = default;
    //����t���O
    private bool _isPenetrationGround = default;
    private bool _isPenetrationCeiling = default;
    private bool _isPenetrationRightWall = default;
    private bool _isPenetrationLeftWall = default;
    private bool _isEnemy = default;
    private List<GameObject> gameObjectList = new List<GameObject>();

    //�㉺���E�ǂ��Ƀ��C�������������̔���p
    private const float boundary = 45f;

    #endregion


    private void Start()
    {
        //2D�{�b�N�X�R���C�_�[���擾���K�v�Ȓl���擾
        BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
        _boxCastSize = _boxCollider.size;
        _boxCastOffset = _boxCollider.offset;
    }

    // BoxCast���΂����\�b�h
    public void ColliderUpdate()
    {
        //���X�g��
        gameObjectList.Clear();
        Vector2 boxCastOrigin = (Vector2)this.transform.position + _boxCastOffset;
        // BoxCast��2���΂�
        RaycastHit2D[] hit = Physics2D.BoxCastAll(boxCastOrigin, _boxCastSize, 0f, transform.up, 0f);

        //���C�������������Ƀ^�O�̊m�F�A�^�O���w��̂��̂ł���΂���ɍ������������s��
        for (int j = 0; j < hit.Length; j++)
        {
            if (hit[j].collider.tag == this.gameObject.tag) { continue; }

            //�^�O���画��擾
            if (hit[j].collider.tag == "Stage")
            {
                if (_isStopDecision_Stage) { break; }
                Vector3 contactNormal = hit[j].normal;

                // �@���x�N�g�������[���h��Ԃɕϊ����p�x���v�Z
                float angle = Vector3.Angle(Vector3.up, hit[j].transform.TransformDirection(hit[j].normal));

                if (angle <= boundary)
                {
                    //�n��
                    _isPenetrationGround = true;
                    break;
                }
                if (angle <= boundary * 3)
                {
                    //��
                    if (boxCastOrigin.x < hit[j].point.x)
                    {
                        //�E��
                        _isPenetrationRightWall = true;
                    }
                    else
                    {
                        //����
                        _isPenetrationLeftWall = true;
                    }
                }
                if (angle > boundary * 3)
                {
                    //�V��
                    _isPenetrationCeiling = true;
                }
            }
            for (int i = 0; i < _enemyTag.Count; i++)
            {
                if (hit[j].collider.tag == _enemyTag[i])
                {
                    if (_isStopDecision_Enemy) { break; }
                    //���X�g�ɓG�L�����N�^�[�̃I�u�W�F�N�g��ǉ�
                    gameObjectList.Add(GetTopParent(hit[j].transform.gameObject));
                    _isEnemy = true;
                }
            }
        }

    }

    private GameObject GetTopParent(GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        while (parent != null)
        {
            gameObject = parent.gameObject;
            parent = gameObject.transform.parent;
        }
        return gameObject;
    }


    public bool SetPenetrationGroundDecision
    {
        get { return _isPenetrationGround; }
        set { _isPenetrationGround = value; }
    }
    public bool SetPenetrationCeilingDecision
    {
        get { return _isPenetrationCeiling; }
        set { _isPenetrationCeiling = value; }
    }
    public bool SetPenetrationRightWallDecision
    {
        get { return _isPenetrationRightWall; }
        set { _isPenetrationRightWall = value; }
    }
    public bool SetPenetrationLeftWallDecision
    {
        get { return _isPenetrationLeftWall; }
        set { _isPenetrationLeftWall = value; }
    }

    public bool SetEnemyDecision
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }

    public List<GameObject> SetEnemyGameObject
    {
        get { return gameObjectList; }
    }
}
