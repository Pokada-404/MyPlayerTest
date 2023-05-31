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

    private List<InputDevice> _connectionGameDevice = new List<InputDevice>();
    [SerializeField]
    private List<GameObject> _activePlayer;

    #endregion

    private void Awake()
    {
        //接続中のデバイスの確認とプレイヤーオブジェクトとの連携処理
        ConnectDevice();
        //新たに接続/切断があった場合呼び出される 
        //InputSystem.onDeviceChange += InputSystemOnDeviceChange; 未実装
    }

    //インスペクター上のボタンにより再読み込み可能
    public void ReroadInputDevice()
    {
        _connectionGameDevice.Clear();
        ConnectDevice();
    }



    //プレイヤーにデバイスを割り当てる処理
    private void ConnectDevice()
    {
        //プレイヤー操作スクリプトがアタッチされているゲームオブジェクトを探す処理
        foreach (GameObject playerObject in GameObject.FindObjectsOfType<GameObject>())
        {
            if (playerObject.GetComponent<NewPlayerController>() != null && !_activePlayer.Contains(playerObject) && playerObject.activeSelf)
            {
                _activePlayer.Add(playerObject);
            }
        }

        //プレイヤー操作スクリプトがアタッチされているゲームオブジェクトを探す処理
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
        if (_activePlayer.Count == 0 || _connectionGameDevice.Count == 0)
        {
            Debug.LogError("接続中のデバイスがない、あるいはプレイヤーが存在しない");
        }

        int counter = 0;
        //ゲームシーン上に存在するプレイヤーの数だけデバイスの割り当てを試みる
        while (counter < _activePlayer.Count)
        {
            _activePlayer[counter].GetComponent<NewPlayerController>().SetDeice(_connectionGameDevice[counter]);
            counter++;
        }
    }

    //未実装
    //private void InputSystemOnDeviceChange(InputDevice device, InputDeviceChange change)
    //{
    //    _connectionGameDevice.Clear();
    //    _activePlayer.Clear();
    //    ConnectDevice();
    //}
}
