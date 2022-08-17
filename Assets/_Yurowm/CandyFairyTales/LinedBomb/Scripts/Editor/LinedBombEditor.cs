using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YMatchThree.Core;
using Yurowm.ObjectEditors;
using Yurowm.Utilities;

namespace YMatchThree.Editor {
    public class LinedBombEditor : ObjectEditor<LinedBomb> {
        public override void OnGUI(LinedBomb bomb, object context = null) {
            bomb.waveOffset = EditorGUILayout.IntSlider("Wave Offset", bomb.waveOffset, 0, 2);
            bomb.side = (Side) EditorGUILayout.EnumFlagsField("Sides", bomb.side);
            
            Edit("Explosion", bomb.explosion);
        }
    }
    public class ILineBreakerEditor : ObjectEditor<ILineBreaker> {
        public override void OnGUI(ILineBreaker breaker, object context = null) {
            if (breaker is SlotContent slotContent)
                slotContent.breakLines = EditorGUILayout.Toggle("Break Lines", slotContent.breakLines);
        }
    }
}