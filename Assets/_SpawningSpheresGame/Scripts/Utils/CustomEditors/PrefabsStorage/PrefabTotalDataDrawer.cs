#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using SpawningSpheresGame.Game.Settings.DataStorage;
using System.Collections.Generic;

namespace SpawningSpheresGame.Utils.CustomEditors
{
    [CustomPropertyDrawer(typeof(PrefabTotalData))]
    public class PrefabTotalDataDrawer : PropertyDrawer
    {
        // Высота одной строки в редакторе
        private const float LineHeight = 18f;
        // Отступ между элементами
        private const float ElementSpacing = 5f;

        // Словарь для хранения развернутых состояний
        private static Dictionary<string, bool> _expandedStates = new Dictionary<string, bool>();


        // Получаем или устанавливаем состояние развертывания
        private bool GetExpanded(string key, bool defaultValue = false)
        {
            if (!_expandedStates.TryGetValue(key, out bool value))
            {
                _expandedStates[key] = defaultValue;
                return defaultValue;
            }
            return value;
        }


        // Устанавливаем состояние развертывания
        private void SetExpanded(string key, bool value)
        {
            _expandedStates[key] = value;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string path = property.propertyPath;
            bool expanded = GetExpanded(path);

            // Если свернуто, только высота заголовка
            if (!expanded)
                return LineHeight;

            // Базовая высота для основных полей
            float height = LineHeight; // Заголовок
            height += LineHeight; // ID
            height += LineHeight; // Prefab

            // Высота для TransformPresets
            string transformsKey = path + ".Transforms";
            bool transformsExpanded = GetExpanded(transformsKey);

            height += LineHeight; // Заголовок для transforms

            if (transformsExpanded)
            {
                SerializedProperty transformsProp = property.FindPropertyRelative("_transformPresets");
                if (transformsProp != null)
                {
                    height += EditorGUI.GetPropertyHeight(transformsProp, true);
                }
            }

            // Высота для CustomData
            string customDataKey = path + ".CustomData";
            bool customDataExpanded = GetExpanded(customDataKey);

            height += LineHeight; // Заголовок для customData

            if (customDataExpanded)
            {
                SerializedProperty customDataProp = property.FindPropertyRelative("CustomData");
                if (customDataProp != null)
                {
                    height += EditorGUI.GetPropertyHeight(customDataProp, true);
                }
            }

            // Добавляем отступы
            height += ElementSpacing * 4;

            return height;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string path = property.propertyPath;
            bool expanded = GetExpanded(path);

            // Получаем ID для отображения
            SerializedProperty idProp = property.FindPropertyRelative("_id");
            string prefabName = "Unknown";
            if (idProp != null && idProp.enumValueIndex >= 0 && idProp.enumValueIndex < idProp.enumDisplayNames.Length)
            {
                prefabName = idProp.enumDisplayNames[idProp.enumValueIndex];
            }

            // Рисуем заголовок
            Rect headerRect = new Rect(position.x, position.y, position.width, LineHeight);
            expanded = EditorGUI.Foldout(headerRect, expanded, $"Prefab: {prefabName}", true);
            SetExpanded(path, expanded);

            if (expanded)
            {
                float yPos = headerRect.yMax + ElementSpacing;

                // Отступ для вложенных элементов
                float indent = 15f;

                // ID поле
                Rect idRect = new Rect(position.x + indent, yPos, position.width - indent, LineHeight);
                EditorGUI.PropertyField(idRect, idProp);
                yPos = idRect.yMax + ElementSpacing;

                // Prefab поле
                Rect prefabRect = new Rect(position.x + indent, yPos, position.width - indent, LineHeight);
                EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("_prefab"));
                yPos = prefabRect.yMax + ElementSpacing;

                // TransformPresets
                string transformsKey = path + ".Transforms";
                bool transformsExpanded = GetExpanded(transformsKey);

                Rect transformsHeaderRect = new Rect(position.x + indent, yPos, position.width - indent, LineHeight);
                transformsExpanded = EditorGUI.Foldout(transformsHeaderRect, transformsExpanded, "Transform Presets", true);
                SetExpanded(transformsKey, transformsExpanded);
                yPos = transformsHeaderRect.yMax;

                if (transformsExpanded)
                {
                    SerializedProperty transformsProp = property.FindPropertyRelative("_transformPresets");
                    if (transformsProp != null)
                    {
                        float transformsHeight = EditorGUI.GetPropertyHeight(transformsProp, true);
                        Rect transformsRect = new Rect(position.x + indent * 2, yPos, position.width - indent * 2, transformsHeight);
                        EditorGUI.PropertyField(transformsRect, transformsProp, GUIContent.none, true);
                        yPos = transformsRect.yMax + ElementSpacing;
                    }
                }
                else
                {
                    yPos += ElementSpacing;
                }

                // CustomData
                string customDataKey = path + ".CustomData";
                bool customDataExpanded = GetExpanded(customDataKey);

                Rect customDataHeaderRect = new Rect(position.x + indent, yPos, position.width - indent, LineHeight);
                customDataExpanded = EditorGUI.Foldout(customDataHeaderRect, customDataExpanded, "Custom Data", true);
                SetExpanded(customDataKey, customDataExpanded);
                yPos = customDataHeaderRect.yMax;

                if (customDataExpanded)
                {
                    SerializedProperty customDataProp = property.FindPropertyRelative("CustomData");
                    if (customDataProp != null)
                    {
                        float customDataHeight = EditorGUI.GetPropertyHeight(customDataProp, true);
                        Rect customDataRect = new Rect(position.x + indent * 2, yPos, position.width - indent * 2, customDataHeight);
                        EditorGUI.PropertyField(customDataRect, customDataProp, GUIContent.none, true);
                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif