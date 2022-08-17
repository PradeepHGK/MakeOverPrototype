using System.Collections.Generic;
using UnityEngine;
using Yurowm.Extensions;

namespace Yurowm.ComposedPages {
    public class ComposedContainer : ComposedElement {
        public Transform root;

        Transform Root => root ? root : transform;
        
        List<ComposedElement> elements = new List<ComposedElement>();

        public void SetHeight(float height) {
            layout.preferredHeight = height;
        }

        public T AddElement<T>(string name = null) where T : ComposedElement {
            T result = page.AddElement<T>(name);
            if (result) {
                result.transform.SetParent(Root);
                result.transform.Reset();
                elements.Add(result);
            }
            return result;
        }

        public override void OnKill() {
            base.OnKill();
            elements.ForEach(e => e.Kill()); 
            elements.Clear();
        }
    }
}