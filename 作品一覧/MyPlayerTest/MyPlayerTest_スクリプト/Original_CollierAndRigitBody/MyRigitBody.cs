// ---------------------------------------------------------
// MyRigitBody.cs
//
// 作成日:2023/5/10
// 作成者:岡田悠暉
// 概　要:プレイヤーの挙動制御用スクリプト
//
// ---------------------------------------------------------

#region ▼　詳細

/*
 * 
 *重力を再現するスクリプト
 *参照するクラスはこのスクリプトがついているオブジェクトの子オブジェクト(孫オブジェクトも含む)
 *についているスクリプト"MyCollider"のみ。
 *
 *
 *
 */

#endregion

#region ▼　Using

using System.Collections.Generic;
using UnityEngine;

#endregion 

public class MyRigitBody : MonoBehaviour
{
    #region ▼ 変数

    //重力
    private const float _gravity = 10;
    // レイの長さ
    private const float RaycastLength = 1.5f;
    //めり込んだ時の押し戻す力
    private const float CorrectionForce = 30f;
    //このscriptがついているオブジェクトの原点より"RaycastOriginCorrection"分下から上に向かって発射されます
    private const float RaycastOriginCorrection = 0.05f;
    //地面についた時補正で地面より"PositionYCorrection"分上にずらされます。
    private const float PositionYCorrection = 0.03f;
    private const float DecisionCorrection = 0.05f;
    //地面との接触判定用レイ
    private Vector2 _rayCastOriginPos = default;

    //フラグなど
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
        //子オブジェクトらにアタッチされている自作Triggerスクリプトを取得
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

            //取得したタグから当たったオブジェクトの判別を行う
            if (!_isPenetrationGround) { _isPenetrationGround = trigger.SetPenetrationGroundDecision; }
            if (!_isPenetrationCeiling) { _isPenetrationCeiling = trigger.SetPenetrationCeilingDecision; }
            if (!_isPenetrationRightWall) { _isPenetrationRightWall = trigger.SetPenetrationRightWallDecision; }
            if (!_isPenetrationLeftWall) { _isPenetrationLeftWall = trigger.SetPenetrationLeftWallDecision; }
            if (!_isEnemy) { _isEnemy = trigger.SetEnemyDecision; }
            if (trigger.SetEnemyDecision)
            {
                foreach (GameObject EnemyObject in trigger.SetEnemyGameObject)
                {
                    //リストに同じオブジェクトが入っていなければ追加
                    if (!_gameObjectList.Contains(EnemyObject))
                    {
                        _gameObjectList.Add(EnemyObject);
                    }
                }
            }

            //接触判定のフラグを元に戻す
            trigger.SetPenetrationGroundDecision = false;
            trigger.SetPenetrationCeilingDecision = false;
            trigger.SetPenetrationRightWallDecision = false;
            trigger.SetPenetrationLeftWallDecision = false;
            trigger.SetEnemyDecision = false;
        }
        //地面との接触判定を取る
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

        //接触フラグによる座標移動処理
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
