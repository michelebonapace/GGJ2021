using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonPropertyDrawer : PropertyDrawer
{
    private MethodInfo eventMethodInfo = null;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        ButtonAttribute inspectorButtonAttribute = (ButtonAttribute)attribute;

        float buttonLength = 50 + inspectorButtonAttribute.MethodName.Length * 6;
        Rect buttonRect = new Rect(position.x + (position.width - buttonLength) * 0.5f, position.y, buttonLength, position.height);

        if (GUI.Button(buttonRect, inspectorButtonAttribute.MethodName))
        {
            System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
            string eventName = inspectorButtonAttribute.MethodName;

            if (eventMethodInfo == null)
            {
                eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            if (eventMethodInfo != null)
            {
                eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
            }
            else
            {
                Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
            }
        }
    }
}
