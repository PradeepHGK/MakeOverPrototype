using Yurowm.ObjectEditors;
using UnityEditor;
using YMatchThree.Core;
using Yurowm.Help;

namespace Yurowm.Editors {
    public class BombEditor : ObjectEditor<Bomb> {
        public override void OnGUI(Bomb bomb, object context = null) {
            Edit("Explosion", bomb.explosion);
            EditorTips.PopLastRectByID("lc.bomb.explosion");
        }
    }
}