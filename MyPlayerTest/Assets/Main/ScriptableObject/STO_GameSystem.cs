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

[CreateAssetMenu(menuName = "Parameter/GameSystemParameter", fileName = "NewGameSystemParameter")]
public class STO_GameSystem : ScriptableObject
{
        [Label("�d��")]
        public float _gravity = default;
}
