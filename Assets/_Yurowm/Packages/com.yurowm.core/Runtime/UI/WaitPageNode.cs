﻿using System.Collections;
using System.Collections.Generic;
using Yurowm.Core;
using Yurowm.Extensions;
using Yurowm.Serialization;

namespace Yurowm.UI {
    public class WaitPageNode : UserPathFilter {
        public List<string> pageNames = new List<string>();

        public override IEnumerator Logic() {

            bool wait = true;
            
            void OnShowPage(Page page) {
                if (pageNames.Contains(page.ID))    
                    wait = false;
            }
            
            Page.onShow += OnShowPage;

            while (wait)
                yield return null;

            Page.onShow -= OnShowPage;
            
            yield return Page.WaitAnimation();
        }


        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("pages", pageNames.ToArray());
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            pageNames.Reuse(reader.ReadCollection<string>("pages"));
        }
    }
}