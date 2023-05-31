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


    #region �� MyRigitBody�ɂ�锻��ƓG���X�g�擾����
    // **********************************************************************
    #region ����

    /*
     * MyRigitBody�ɂ�锻��擾����
     * return��true�Ȃ烁�C���̏�����return(�����������I��)����
     */

    #endregion
    /// <summary>
    /// �d�͂ƐڐG����̎擾
    /// </summary>
    /// <returns>�V���ǂɐڐG���Ă����ꍇtrue��Ԃ�</returns>
    protected bool MyRigitBodyUse()
    {
        //�����𑖂点�� �U�����͏����𑖂点�����Ȃ�
        MyRigitBody.RigitBodyUpdate();
        //����̎擾
        _isGround = MyRigitBody.SetGroundDecision;
        _isHitEnemy = MyRigitBody.SetEnemyDecision;
        _isWallDecision = MyRigitBody.SetWallDecision;
        _isCeiling = MyRigitBody.SetCeilingDecision;
        _enemyList = MyRigitBody.SetEnemyObjectList;
        //�������~�߂�
        if (_isCeiling) { _jumpCountPower = 0; return true; }
        if (_isWallDecision) { return true; }
        return false;
    }

    #endregion
}
