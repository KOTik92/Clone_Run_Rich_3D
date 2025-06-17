using System.IO;
using UnityEditor;
using UnityEngine;

namespace ButchersGames
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        private SerializedProperty _editorMode, _lvlList, _playerController, _textCurrentLevel;
        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = target as LevelManager;
        }

        private void OnEnable()
        {
            _editorMode = serializedObject.FindProperty("editorMode");
            _lvlList = serializedObject.FindProperty("levels");
            _playerController = serializedObject.FindProperty("playerController");
            _textCurrentLevel = serializedObject.FindProperty("textCurrentLevel");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            _editorMode.boolValue = GUILayout.Toggle(_editorMode.boolValue, new GUIContent("Editor Mode"), GUILayout.Width(100), GUILayout.Height(20));
            if (_editorMode.boolValue)
            {
                DrawSelectedLevel();
            }

            EditorGUILayout.PropertyField(_lvlList);
            EditorGUILayout.PropertyField(_playerController);
            EditorGUILayout.PropertyField(_textCurrentLevel);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (GUILayout.Button("Delete Saves", GUILayout.Width(200), GUILayout.Height(20)))
            {
                PlayerPrefs.DeleteAll();
                File.Delete(Path.Combine(Application.persistentDataPath, "GameData.json"));
                if (File.Exists(Path.Combine(Application.dataPath, @"YandexGame\WorkingData\Editor\SavesEditorYG.json"))) File.Delete(Path.Combine(Application.dataPath, @"YandexGame\WorkingData\Editor\SavesEditorYG.json"));
            }
        }

        private void DrawSelectedLevel()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            int index = EditorGUILayout.IntField("Current Level", _levelManager.CurrentLevelIndex + 1);
            if (EditorGUI.EndChangeCheck())
            {
                _levelManager.SelectLevel(index - 1);
                serializedObject.ApplyModifiedProperties();
            }

            if (GUILayout.Button("<<", GUILayout.Width(30), GUILayout.Height(20)))
            {
                _levelManager.PrevLevel();
            }
            if (GUILayout.Button(">>", GUILayout.Width(30), GUILayout.Height(20)))
            {
                _levelManager.NextLevel();
            }

            EditorGUILayout.EndHorizontal();
        }

    }
}