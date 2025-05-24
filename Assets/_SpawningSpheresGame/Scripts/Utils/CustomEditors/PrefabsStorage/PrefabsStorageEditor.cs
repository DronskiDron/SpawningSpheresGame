#if UNITY_EDITOR
using SpawningSpheresGame.Game.Settings.DataStorage;
using UnityEditor;
using UnityEngine;

namespace SpawningSpheresGame.Utils.CustomEditors
{
    [CustomEditor(typeof(PrefabsStorage))]
    public class PrefabsStorageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Рисуем стандартный инспектор с правильными отступами
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            SerializedProperty prefabsList = serializedObject.FindProperty("_prefabsList");
            if (prefabsList != null)
            {
                EditorGUILayout.PropertyField(prefabsList, new GUIContent("Prefabs List"), true);
            }

            serializedObject.ApplyModifiedProperties();

            // Добавляем кнопку обновления
            GUILayout.Space(10);
            GUI.backgroundColor = new Color(0.2f, 0.8f, 0.3f);
            if (GUILayout.Button("UPDATE DATA TYPES", GUILayout.Height(30)))
            {
                UpdateAllDataTypes();
            }
            GUI.backgroundColor = Color.white;
        }


        private void UpdateAllDataTypes()
        {
            var storage = (PrefabsStorage)target;
            SerializedProperty prefabsList = serializedObject.FindProperty("_prefabsList");

            if (prefabsList == null)
            {
                Debug.LogError("Couldn't find _prefabsList");
                return;
            }

            Undo.RecordObject(target, "Update Data Types");

            for (int i = 0; i < prefabsList.arraySize; i++)
            {
                SerializedProperty prefabDataProp = prefabsList.GetArrayElementAtIndex(i);
                if (prefabDataProp == null) continue;

                SerializedProperty customDataProp = prefabDataProp.FindPropertyRelative("CustomData");
                if (customDataProp == null) continue;

                for (int j = 0; j < customDataProp.arraySize; j++)
                {
                    SerializedProperty entryProp = customDataProp.GetArrayElementAtIndex(j);
                    if (entryProp == null) continue;

                    SerializedProperty typeProp = entryProp.FindPropertyRelative("Type");
                    if (typeProp == null) continue;

                    PrefabEntityType type = (PrefabEntityType)typeProp.enumValueIndex;
                    if (type == PrefabEntityType.None) continue;

                    SerializedProperty dataProp = entryProp.FindPropertyRelative("Data");
                    if (dataProp == null) continue;

                    // Создаем новый объект нужного типа
                    BasePrefabData newData = CreateDataForType(type);

                    // Устанавливаем значение
                    dataProp.managedReferenceValue = newData;
                }
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            Debug.Log("Data types have been updated successfully");
        }


        private BasePrefabData CreateDataForType(PrefabEntityType type)
        {
            switch (type)
            {
                case PrefabEntityType.Player:
                    return new PlayerData { Type = type };
                case PrefabEntityType.MoveCamera:
                    return new MoveCameraData { Type = type };
                case PrefabEntityType.ZoomCamera:
                    return new ZoomCameraData { Type = type };
                default:
                    return null;
            }
        }
    }
}
#endif