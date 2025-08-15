using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

            if (buttonAttribute != null)
            {
                string label = string.IsNullOrEmpty(buttonAttribute.Label) ? method.Name : buttonAttribute.Label;

                if (GUILayout.Button(label))
                {
                    method.Invoke(target, null);
                }
            }
        }
    }
}
