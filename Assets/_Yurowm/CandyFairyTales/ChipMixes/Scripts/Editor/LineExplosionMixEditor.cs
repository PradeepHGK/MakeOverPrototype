using UnityEditor;
using YMatchThree.Core;
using Yurowm.Help;
using Yurowm.ObjectEditors;
using Yurowm.Utilities;

namespace YMatchThree.Editor {
    public class LineExplosionMixEditor : ObjectEditor<LineExplosionMix> {
        public override void OnGUI(LineExplosionMix mix, object context = null) {
            mix.waveOffset = EditorGUILayout.IntSlider("Wave Offset", mix.waveOffset, 0, 2);
            EditorTips.PopLastRectByID("lc.lineexplosion.waveoffset");
            
            mix.side = (Side) EditorGUILayout.EnumFlagsField("Sides", mix.side);
            EditorTips.PopLastRectByID("lc.lineexplosion.sides");
            
            Edit("Explosion", mix.explosion);
            EditorTips.PopLastRectByID("lc.lineexplosion.explosion");
        }
    }
}