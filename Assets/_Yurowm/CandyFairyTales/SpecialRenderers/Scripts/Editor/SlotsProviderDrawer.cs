using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YMatchThree.Core;
using Yurowm.Utilities;

[CustomPropertyDrawer(typeof(SlotRenderer.SlotsProvider))]

public class SlotsProviderDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        position = EditorGUI.PrefixLabel(position, label);
        
        area a = new area(int2.one, new int2(10, 10));

        var target = property.serializedObject.targetObject as SlotRenderer;
        
        Undo.RecordObject(target, null);
        
        // foreach (var point in a.GetPoints()) {
        //     var r = new Rect(
        //         position.x + position.width * (point.X - 1) / 10,
        //         position.y + position.height * (10 - point.Y) / 10,
        //         position.width / 10,
        //         position.height / 10);
        //     
        //     var value = target.slots.slots.Contains(point);
        //     if (GUI.Toggle(r, value, "") != value) {
        //         value = !value;
        //         if (value)
        //             target.slots.slots.Add(point);
        //         else
        //             target.slots.slots.Remove(point);
        //         target.OnValidate();
        //     }
        // }
        
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 150;
    }
}
