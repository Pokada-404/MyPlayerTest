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


    #region ¥ MyRigitBody‚É‚æ‚é”»’è‚Æ“GƒŠƒXƒgæ“¾ˆ—
    // **********************************************************************
    #region à–¾

    /*
     * MyRigitBody‚É‚æ‚é”»’èæ“¾ˆ—
     * return‚ªtrue‚È‚çƒƒCƒ“‚Ìˆ—‚àreturn(ˆ—‚ğ‚¹‚¸I—¹)‚·‚é
     */

    #endregion
    /// <summary>
    /// d—Í‚ÆÚG”»’è‚Ìæ“¾
    /// </summary>
    /// <returns>“Vˆä‚â•Ç‚ÉÚG‚µ‚Ä‚¢‚½ê‡true‚ğ•Ô‚·</returns>
    protected bool MyRigitBodyUse()
    {
        //ˆ—‚ğ‘–‚ç‚¹‚é UŒ‚’†‚Íˆ—‚ğ‘–‚ç‚¹‚½‚­‚È‚¢
        MyRigitBody.RigitBodyUpdate();
        //”»’è‚Ìæ“¾
        _isGround = MyRigitBody.SetGroundDecision;
        _isHitEnemy = MyRigitBody.SetEnemyDecision;
        _isWallDecision = MyRigitBody.SetWallDecision;
        _isCeiling = MyRigitBody.SetCeilingDecision;
        _enemyList = MyRigitBody.SetEnemyObjectList;
        //ˆ—‚ğ~‚ß‚é
        if (_isCeiling) { _jumpCountPower = 0; return true; }
        if (_isWallDecision) { return true; }
        return false;
    }

    #endregion
}
