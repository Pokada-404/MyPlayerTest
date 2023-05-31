// ---------------------------------------------------------
// STO_AttackMotionParameter.cs
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

[CreateAssetMenu(menuName = "Parameter/AttackMotionParameter", fileName = "NewAttackMotionParameter")]
public class STO_AttackMotionParameter : ScriptableObject
{
    [Header("- �ڍאݒ� -")]
    public List<AttackMotionParameter> _attackParameters;
    [System.Serializable]
    public class AttackMotionParameter
    {
        [Label("�A�j���[�V�����N���b�v��")]
        public string _AnimationClipName = default;
        [Label("���[�V�������e"), Multiline(2)]
        public string _motionContent = default;
        [Label("�U����")]
        public float _damage = default;
        [Label("�����̂Ƀ_���[�W��^����")]
        public bool _attackTarget_Plural = default;
        [Label("�U������̃N�[���^�C�� ��1"), Range(0, 1)]
        public float _decisionCoolTime = default;
        [Header("                   ��1 �l������������ΘA���Ń_���[�W��^������A�O�ɂ���΃��[�V�������I���܂ōĂѓ�����Ȃ�")]
        [Label("�q�b�g�X�g�b�v (Frame��)")]
        public float _hitStopTime = default;
        [Label("�q�b�g�X���[ (Frame��)")]
        public float _hitSlowTime = default;
        [Label("�q�b�g�X���[���x��"), Range(0, 1)]
        public float _hitSlowLevel = default;
    }
}
