// ---------------------------------------------------------
// PlayerInputSystemController.cs
//
// �쐬��:2023/5/23
// �쐬��:���c�I��
// �T�@�v:�v���C���[�̓���Device�̊Ǘ��X�N���v�g
//
// ---------------------------------------------------------

#region ���@�ڍ�

/*
* 
* 
*/

#endregion

#region ���@Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion


public class PlayerInputSystemController : MonoBehaviour
{
    #region �� �ϐ�

    private List<InputDevice> _connectionGameDevice = new List<InputDevice>();
    [SerializeField]
    private List<GameObject> _activePlayer;

    #endregion

    private void Awake()
    {
        //�ڑ����̃f�o�C�X�̊m�F�ƃv���C���[�I�u�W�F�N�g�Ƃ̘A�g����
        ConnectDevice();
        //�V���ɐڑ�/�ؒf���������ꍇ�Ăяo����� 
        //InputSystem.onDeviceChange += InputSystemOnDeviceChange; ������
    }

    //�C���X�y�N�^�[��̃{�^���ɂ��ēǂݍ��݉\
    public void ReroadInputDevice()
    {
        _connectionGameDevice.Clear();
        ConnectDevice();
    }



    //�v���C���[�Ƀf�o�C�X�����蓖�Ă鏈��
    private void ConnectDevice()
    {
        //�v���C���[����X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g��T������
        foreach (GameObject playerObject in GameObject.FindObjectsOfType<GameObject>())
        {
            if (playerObject.GetComponent<NewPlayerController>() != null && !_activePlayer.Contains(playerObject) && playerObject.activeSelf)
            {
                _activePlayer.Add(playerObject);
            }
        }

        //�v���C���[����X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g��T������
        foreach (InputDevice newDevice in InputSystem.devices)
        {
            //�Q�[���p�b�h�ǉ�
            if (newDevice.name != "Keyboard" && newDevice.name != "Mouse")
            {
                _connectionGameDevice.Add(newDevice);
            }
        }
        foreach (InputDevice newDevice in InputSystem.devices)
        {
            //�L�[�{�[�h�̂ݒǉ�
            if (newDevice.name == "Keyboard")
            {
                _connectionGameDevice.Add(newDevice);
            }
        }

        
        //�ڑ����̃f�o�C�X���Ȃ��A���邢�̓v���C���[�����݂��Ȃ��ꍇ�͏������s��Ȃ�
        if (_activePlayer.Count == 0 || _connectionGameDevice.Count == 0)
        {
            Debug.LogError("�ڑ����̃f�o�C�X���Ȃ��A���邢�̓v���C���[�����݂��Ȃ�");
        }

        int counter = 0;
        //�Q�[���V�[����ɑ��݂���v���C���[�̐������f�o�C�X�̊��蓖�Ă����݂�
        while (counter < _activePlayer.Count)
        {
            _activePlayer[counter].GetComponent<NewPlayerController>().SetDeice(_connectionGameDevice[counter]);
            counter++;
        }
    }

    //������
    //private void InputSystemOnDeviceChange(InputDevice device, InputDeviceChange change)
    //{
    //    _connectionGameDevice.Clear();
    //    _activePlayer.Clear();
    //    ConnectDevice();
    //}
}
