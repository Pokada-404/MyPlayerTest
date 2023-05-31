//using UnityEditor;
//using UnityEngine;

////ボタンを表示させるコンポーネント(Class)
//[CustomEditor(typeof(PlayerInputSystemController))]
//public class Expansion_PlayerInputSystemController : Editor
//{

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        //ボタンを表示させるコンポーネント
//        PlayerInputSystemController playerInpSysCont = target as PlayerInputSystemController;

//        GUILayout.Space(20);
//        EditorGUILayout.BeginHorizontal();
//        {
//            EditorGUILayout.Space();
//            EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
//            if (GUILayout.Button("Update Device Connection",   //ボタンの上に表示する文字
//                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.Height(30)))
//            { 
//                // パブリックメソッドを実行
//                playerInpSysCont.ReroadInputDevice(); 
//            }
//            EditorGUI.EndDisabledGroup();
//            EditorGUILayout.Space();
//        }
//        EditorGUILayout.EndHorizontal();
//        GUILayout.Space(20);
//    }
//}