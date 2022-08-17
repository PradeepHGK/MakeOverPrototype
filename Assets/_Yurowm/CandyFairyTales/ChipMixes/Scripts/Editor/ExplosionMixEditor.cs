using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YMatchThree.Core;
using Yurowm.Help;
using Yurowm.ObjectEditors;

namespace YMatchThree.Editor {
    public class ExplosionMixEditor : ObjectEditor<ExplosionMix> {
        public override void OnGUI(ExplosionMix mix, object context = null) {
            mix.distance = Mathf.Max(1, EditorGUILayout.IntField("Distance", mix.distance));
            EditorTips.PopLastRectByID("lc.explosionmix.distance");
            
            Edit("Explosion", mix.explosion);
            EditorTips.PopLastRectByID("lc.explosionmix.explosion");
        }
    }
}