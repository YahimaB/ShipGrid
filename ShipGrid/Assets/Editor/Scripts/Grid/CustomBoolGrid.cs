using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(BoolGrid))]
public class CustomBoolGrid : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);

        Rect newPosition = position;
        newPosition.height = 20;
        newPosition.width = 20;

        int x = property.FindPropertyRelative("x").intValue;
        int y = property.FindPropertyRelative("y").intValue;
        SerializedProperty rows = property.FindPropertyRelative("rows");

        for (int i = 0; i < y; i++)
        {
            newPosition.y += 20;

            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");
            if (row.arraySize != x)
                row.arraySize = x;

            for (int j = 0; j < x; j++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                newPosition.x += newPosition.width;
            }

            newPosition.x = position.x;
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int y = property.FindPropertyRelative("y").intValue;
        return 20 * (y + 1);
    }
}
