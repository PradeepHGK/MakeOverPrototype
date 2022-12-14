using Yurowm.ObjectEditors;
using UnityEditor;
using YMatchThree.Core;

namespace Yurowm.Editors {
    public class JumpingMixEditor : ObjectEditor<JumpingMix> {
        public override void OnGUI(JumpingMix mix, object context = null) {
            mix.count = EditorGUILayout.IntSlider(mix.count, 1, 20);
            Edit("Explosion (Start)", mix.startExplosion);
            Edit("Explosion (End)", mix.endExplosion);
        }
    }
}