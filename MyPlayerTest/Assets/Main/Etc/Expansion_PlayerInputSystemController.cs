//using UnityEditor;
//using UnityEngine;

////�{�^����\��������R���|�[�l���g(Class)
//[CustomEditor(typeof(PlayerInputSystemController))]
//public class Expansion_PlayerInputSystemController : Editor
//{

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        //�{�^����\��������R���|�[�l���g
//        PlayerInputSystemController playerInpSysCont = target as PlayerInputSystemController;

//        GUILayout.Space(20);
//        EditorGUILayout.BeginHorizontal();
//        {
//            EditorGUILayout.Space();
//            EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
//            if (GUILayout.Button("Update Device Connection",   //�{�^���̏�ɕ\�����镶��
//                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.Height(30)))
//            { 
//                // �p�u���b�N���\�b�h�����s
//                playerInpSysCont.ReroadInputDevice(); 
//            }
//            EditorGUI.EndDisabledGroup();
//            EditorGUILayout.Space();
//        }
//        EditorGUILayout.EndHorizontal();
//        GUILayout.Space(20);
//    }
//}