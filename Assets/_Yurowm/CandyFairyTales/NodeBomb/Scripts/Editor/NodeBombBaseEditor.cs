using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YMatchThree.Core;
using Yurowm.ContentManager;
using Yurowm.Effects;
using Yurowm.ObjectEditors;
using Yurowm.Spaces;

namespace YMatchThree.Editor {
    public class NodeBombBaseEditor : ObjectEditor<NodeBombBase> {
        public override void OnGUI(NodeBombBase bomb, object context = null) {
            BaseTypesEditor.SelectAsset<EffectBody>(bomb, nameof(bomb.nodeEffect));
            Edit("Explosion", bomb.explosion);
        }
    }
}