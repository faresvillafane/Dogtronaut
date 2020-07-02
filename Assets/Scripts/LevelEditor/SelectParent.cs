using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SelectParent : EditorWindow
{
    [MenuItem("Edit/Select parent &c")]
    static void SelectParentOfObject()
    {
        Object[] parents = new Object[Selection.objects.Length];
        for(int i = 0; i < Selection.objects.Length; i++)
        {
            parents[i] = ((GameObject)Selection.objects[i]).GetComponentInParent<Tile>();
        }
        Selection.objects = parents;

    }
}
#endif