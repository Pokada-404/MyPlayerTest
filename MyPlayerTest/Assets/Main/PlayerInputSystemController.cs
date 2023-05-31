// ---------------------------------------------------------
// PlayerInputSystemController.cs
//
// 作成日:2023/5/23
// 作成者:岡田悠暉
// 概　要:プレイヤーの入力Deviceの管理スクリプト
//
// ---------------------------------------------------------

#region ▼　詳細

/*
* 
* 
*/

#endregion

#region ▼　Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion


public class PlayerInputSystemController : MonoBehaviour
{
    #region ▼ 変数

    [SerializeField]
    STO_GameSystem GameSystem = default;
    private List<InputDevice> _connectionGameDevice = new List<InputDevice>();
    [SerializeField]
    private List<GameObject> playerObject = default;

    #endregion

    private void Awake()
    {
        //デバイスの接続/切断があった場合呼び出される 
        InputSystem.onDeviceChange += InputSystemOnDeviceChange;
        //接続中のデバイスの確認とプレイヤーオブジェクトとの連携処理
        ConnectDevice();
    }
    private void Start()
    {
        ConnectDevice();
    }

    //プレイヤーにデバイスを割り当てる処理
    private void ConnectDevice()
    {
        _connectionGameDevice.Clear();
        //接続中のコントローラーを探す処理
        foreach (InputDevice newDevice in InputSystem.devices)
        {
            //ゲームパッド追加
            if (newDevice.name != "Keyboard" && newDevice.name != "Mouse")
            {
                _connectionGameDevice.Add(newDevice);
            }
        }
        foreach (InputDevice newDevice in InputSystem.devices)
        {
            //キーボードのみ追加
            if (newDevice.name == "Keyboard")
            {
                _connectionGameDevice.Add(newDevice);
            }
        }


        //接続中のデバイスがない、あるいはプレイヤーが存在しない場合は処理を行わない
        if (playerObject.Count == 0)
        {
            Debug.LogError("設定されたプレイヤーが存在しない");
        }
        //接続中のデバイスがない、あるいはプレイヤーが存在しない場合は処理を行わない
        if (_connectionGameDevice.Count == 0)
        {
            Debug.LogError("接続中のデバイスがない");
        }

        //接続中のデバイスがない、あるいはプレイヤーが存在しない場合は処理を行わない
        if (_connectionGameDevice[0].name != "Keyboard")
        {
            _connectionGameDevice.RemoveAt(_connectionGameDevice.Count - 1);
        }

        int counter = 0;
        //接続されたデバイスの数だけキャラクターに割り当てを試みる
        while (counter < _connectionGameDevice.Count)
        {
            playerObject[counter].SetActive(true);
            print(playerObject[counter].name + "に" + _connectionGameDevice[counter]);
            playerObject[counter].GetComponent<NewPlayerController>().SetDeice(_connectionGameDevice[counter]);
            playerObject[counter].GetComponent<NewPlayerController>().SetGravity = GameSystem._gravity;
            counter++;
        }
        //残りの登録されたキャラクターはActiveをfalseにする
        while (counter < playerObject.Count)
        {
            playerObject[counter].GetComponent<HumanCharacterBaseProcess>().SetGravity = GameSystem._gravity;
            playerObject[counter].SetActive(false);
            counter++;
        }
    }

    //未実装
    private void InputSystemOnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        ReroadInputDevice();
    }
    //インスペクター上のボタンにより再読み込み可能　未実装
    public void ReroadInputDevice()
    {
        print("DeviceReset");
        ConnectDevice();
    }
}
