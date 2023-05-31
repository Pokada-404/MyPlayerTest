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
* ���̃X�N���v�g�̓A�j���[�^�[�ƃA�j���[�V�����N���b�v�ɑ傫���ˑ����Ă��܂��B
* �A�j���[�V������ҏW����ꍇ�͈ȉ��̓��e�ɒ��ӂ��Ă�������
* [ AttackLayer ]
*   �E�A�C�h��(Attack_Idle)�̒������R���{���r�؂��܂ł̗P�\���ԂɂȂ��Ă��܂��B
*     �t���O"_isComboCounterReset"�̓A�j���[�V�����擪��false�ɂ��A�I�[���1�t���[���O��true�ɂ��Ă������� �B
*     �t���O"_isAttackProsses"��"_isStopInputAttack"�̓A�j���[�V�����擪�ƏI�[��false�ɐݒ肵�Ă��������B
*   �E�U���A�j���[�V�����̃t���O"_isAttackProsses"��true�ɂȂ��Ă���Ԃ͍U�����͂��󂯕t���܂���B
*     false�ɂȂ����^�C�~���O����A�j���[�V�����̏I�[�܂ł��U�����͂̎�t���ԂɂȂ�܂��B
*   �E�t���O"_isStopInputAttack"�̓A�j���[�V�����̍Ō�܂ōĐ����I���������Ƃ�ʒm����t���O�ł��B
*     �A�j���[�V�����̐擪��true�ŏI�[��false�ɂ��Ă��������B
*/

#endregion

#region ���@Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class Test_MyRigitBodyUse : MonoBehaviour
{
    [SerializeField]
    private bool _isGround = default;
    [SerializeField]
    private bool _isEnemy = default;
    private MyRigitBody MyRigitBody;

    private void OnEnable()
    {
        MyRigitBody = this.GetComponent<MyRigitBody>();
    }

    /// <summary>
    /// ���C������
    /// </summary>
    private void Update()
    {
        // *--- �d�͂̓K�p�Ɠ����蔻��擾���� ---*
        _isGround = MyRigitBody.SetGroundDecision;
        _isEnemy = MyRigitBody.SetEnemyDecision;
    } 
}
