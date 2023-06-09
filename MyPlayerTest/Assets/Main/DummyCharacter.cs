using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacter : HumanCharacterBaseProcess
{
    // Start is called before the first frame update
    void Start()
    {
        SetValueGlobalVariable();
        SetValueFromScriptableObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (MyRigitBodyUse()) { return; }
        AnimationUpdate();
    }


    #region ▼ MyRigitBodyによる判定と敵リスト取得処理
    // **********************************************************************
    #region 説明

    /*
     * MyRigitBodyによる判定取得処理
     * returnがtrueならメインの処理もreturn(処理をせず終了)する
     */

    #endregion
    /// <summary>
    /// 重力と接触判定の取得
    /// </summary>
    /// <returns>天井や壁に接触していた場合trueを返す</returns>
    protected bool MyRigitBodyUse()
    {
        //処理を走らせる 攻撃中は処理を走らせたくない
        MyRigitBody.RigitBodyUpdate();
        //判定の取得
        _isGround = MyRigitBody.SetGroundDecision;
        _isHitEnemy = MyRigitBody.SetEnemyDecision;
        _isWallDecision = MyRigitBody.SetWallDecision;
        _isCeiling = MyRigitBody.SetCeilingDecision;
        _enemyList = MyRigitBody.SetEnemyObjectList;
        //処理を止める
        if (_isCeiling) { _jumpCountPower = 0; return true; }
        if (_isWallDecision) { return true; }
        return false;
    }

    #endregion
}
