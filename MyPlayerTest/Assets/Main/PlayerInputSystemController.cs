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

    [SerializeField]
    STO_GameSystem GameSystem = default;
    private List<InputDevice> _connectionGameDevice = new List<InputDevice>();
    [SerializeField]
    private List<GameObject> playerObject = default;

    #endregion

    private void Awake()
    {
        //�f�o�C�X�̐ڑ�/�ؒf���������ꍇ�Ăяo����� 
        InputSystem.onDeviceChange += InputSystemOnDeviceChange;
        //�ڑ����̃f�o�C�X�̊m�F�ƃv���C���[�I�u�W�F�N�g�Ƃ̘A�g����
        ConnectDevice();
    }
    private void Start()
    {
        ConnectDevice();
    }

    //�v���C���[�Ƀf�o�C�X�����蓖�Ă鏈��
    private void ConnectDevice()
    {
        _connectionGameDevice.Clear();
        //�ڑ����̃R���g���[���[��T������
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
        if (playerObject.Count == 0)
        {
            Debug.LogError("�ݒ肳�ꂽ�v���C���[�����݂��Ȃ�");
        }
        //�ڑ����̃f�o�C�X���Ȃ��A���邢�̓v���C���[�����݂��Ȃ��ꍇ�͏������s��Ȃ�
        if (_connectionGameDevice.Count == 0)
        {
            Debug.LogError("�ڑ����̃f�o�C�X���Ȃ�");
        }

        //�ڑ����̃f�o�C�X���Ȃ��A���邢�̓v���C���[�����݂��Ȃ��ꍇ�͏������s��Ȃ�
        if (_connectionGameDevice[0].name != "Keyboard")
        {
            _connectionGameDevice.RemoveAt(_connectionGameDevice.Count - 1);
        }

        int counter = 0;
        //�ڑ����ꂽ�f�o�C�X�̐������L�����N�^�[�Ɋ��蓖�Ă����݂�
        while (counter < _connectionGameDevice.Count)
        {
            playerObject[counter].SetActive(true);
            print(playerObject[counter].name + "��" + _connectionGameDevice[counter]);
            playerObject[counter].GetComponent<NewPlayerController>().SetDeice(_connectionGameDevice[counter]);
            playerObject[counter].GetComponent<NewPlayerController>().SetGravity = GameSystem._gravity;
            counter++;
        }
        //�c��̓o�^���ꂽ�L�����N�^�[��Active��false�ɂ���
        while (counter < playerObject.Count)
        {
            playerObject[counter].GetComponent<HumanCharacterBaseProcess>().SetGravity = GameSystem._gravity;
            playerObject[counter].SetActive(false);
            counter++;
        }
    }

    //������
    private void InputSystemOnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        ReroadInputDevice();
    }
    //�C���X�y�N�^�[��̃{�^���ɂ��ēǂݍ��݉\�@������
    public void ReroadInputDevice()
    {
        print("DeviceReset");
        ConnectDevice();
    }
}
