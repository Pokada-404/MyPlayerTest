// ---------------------------------------------------------
// MyCollider.cs
//
// 作成日:2023/5/10 
// 作成者:岡田悠暉
// 概　要:プレイヤーの挙動制御用スクリプト
//
// ---------------------------------------------------------

#region ▼　詳細

/* 
 * ！- 注意 -！
 * スクリプト"MyRigitBody"はこのオブジェクトの一番上のオブジェクトに着いている必要があります。
 *
 * スクリプト"MyRigitBody"からのみ呼び出されます。
 */

#endregion

#region ▼　Using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class MyColliderTrigger : MonoBehaviour
{

    #region ▼ 変数

    // BoxCastのサイズ値とオフセット値
    private Vector2 _boxCastSize = default;
    private Vector2 _boxCastOffset = default;

    [Header("敵判定設定")]
    [SerializeField, Label("敵判定タグ名")] //このタグを持つオブジェクトを敵とみなします
    private List<string> _enemyTag = default;
    [Header("判定無効設定")]
    [SerializeField, Label("ステージの接触判定無効")]
    private bool _isStopDecision_Stage = default;
    [SerializeField, Label("敵の接触判定無効")]
    private bool _isStopDecision_Enemy = default;
    //判定フラグ
    private bool _isPenetrationGround = default;
    private bool _isPenetrationCeiling = default;
    private bool _isPenetrationRightWall = default;
    private bool _isPenetrationLeftWall = default;
    private bool _isEnemy = default;
    private List<GameObject> gameObjectList = new List<GameObject>();

    //上下左右どこにレイが当たったかの判定用
    private const float boundary = 45f;

    #endregion


    private void Start()
    {
        //2Dボックスコライダーを取得し必要な値を取得
        BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
        _boxCastSize = _boxCollider.size;
        _boxCastOffset = _boxCollider.offset;
    }

    // BoxCastを飛ばすメソッド
    public void ColliderUpdate()
    {
        //リストの
        gameObjectList.Clear();
        Vector2 boxCastOrigin = (Vector2)this.transform.position + _boxCastOffset;
        // BoxCastを2種飛ばす
        RaycastHit2D[] hit = Physics2D.BoxCastAll(boxCastOrigin, _boxCastSize, 0f, transform.up, 0f);

        //レイが当たった順にタグの確認、タグが指定のものであればそれに合った処理を行う
        for (int j = 0; j < hit.Length; j++)
        {
            if (hit[j].collider.tag == this.gameObject.tag) { continue; }

            //タグから判定取得
            if (hit[j].collider.tag == "Stage")
            {
                if (_isStopDecision_Stage) { break; }
                Vector3 contactNormal = hit[j].normal;

                // 法線ベクトルをワールド空間に変換し角度を計算
                float angle = Vector3.Angle(Vector3.up, hit[j].transform.TransformDirection(hit[j].normal));

                if (angle <= boundary)
                {
                    //地面
                    _isPenetrationGround = true;
                    break;
                }
                if (angle <= boundary * 3)
                {
                    //壁
                    if (boxCastOrigin.x < hit[j].point.x)
                    {
                        //右壁
                        _isPenetrationRightWall = true;
                    }
                    else
                    {
                        //左壁
                        _isPenetrationLeftWall = true;
                    }
                }
                if (angle > boundary * 3)
                {
                    //天井
                    _isPenetrationCeiling = true;
                }
            }
            for (int i = 0; i < _enemyTag.Count; i++)
            {
                if (hit[j].collider.tag == _enemyTag[i])
                {
                    if (_isStopDecision_Enemy) { break; }
                    //リストに敵キャラクターのオブジェクトを追加
                    gameObjectList.Add(GetTopParent(hit[j].transform.gameObject));
                    _isEnemy = true;
                }
            }
        }

    }

    private GameObject GetTopParent(GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        while (parent != null)
        {
            gameObject = parent.gameObject;
            parent = gameObject.transform.parent;
        }
        return gameObject;
    }


    public bool SetPenetrationGroundDecision
    {
        get { return _isPenetrationGround; }
        set { _isPenetrationGround = value; }
    }
    public bool SetPenetrationCeilingDecision
    {
        get { return _isPenetrationCeiling; }
        set { _isPenetrationCeiling = value; }
    }
    public bool SetPenetrationRightWallDecision
    {
        get { return _isPenetrationRightWall; }
        set { _isPenetrationRightWall = value; }
    }
    public bool SetPenetrationLeftWallDecision
    {
        get { return _isPenetrationLeftWall; }
        set { _isPenetrationLeftWall = value; }
    }

    public bool SetEnemyDecision
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }

    public List<GameObject> SetEnemyGameObject
    {
        get { return gameObjectList; }
    }
}
