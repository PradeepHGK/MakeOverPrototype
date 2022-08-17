using System.Collections.Generic;
using Yurowm.Effects;

namespace YMatchThree.Core {
    public class ColorNodeBomb : NodeBomb, IColored {
        protected override bool TargetFilter(SlotContent content) {
            return content is IColored c && c.colorInfo.IsMatchWith(colorInfo);
        }
        
        protected override IEnumerable<IEffectCallback> SetupNodeEffect() {
            yield return new NodeEffectCurveInterpolator(random.Range(.2f, .3f), 0, 0);
        }
    }
}