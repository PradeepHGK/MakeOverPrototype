using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Yurowm.Coroutines;
using Yurowm.Effects;
using Yurowm.Serialization;
using Yurowm.Utilities;
using Space = Yurowm.Spaces.Space;

namespace YMatchThree.Core {
    public class Bomb : BombChipBase, IColored {

        public ExplosionParameters explosion = new ExplosionParameters();
        
        int scorePoints;
        
        public override void OnAddToSpace(Space space) {
            base.OnAddToSpace(space);
            scorePoints = scoreReward;
            scoreReward = 0;
        }

        public override IEnumerator Exploding() {
            var slots = context.GetArgument<Slots>();

            var hitGroup = new List<Slot>();
            
            var center = slotModule.Center;

            foreach (Side side in Sides.all) {
                if (slots.all.TryGetValue(center + side, out var s)) {
                    var cc = s.GetCurrentContent();
                    if (cc is IDestroyable destroyable && !destroyable.destroying)
                        hitGroup.Add(s);
                }
            }
            
            hitGroup.AddRange(slotModule.Slots());
            
            var hitContext = new HitContext(context, hitGroup.Distinct(), HitReason.BombExplosion);
            
            field.Explode(position, explosion);
            
            field.Highlight(hitGroup.ToArray(), slotModule.Slot());
            
            scorePoints += hitGroup.Sum(s => s.Hit(hitContext));
            
            space.context.Get<Score>().AddScore(scorePoints);
            slotModule
                .Slots()
                .First()
                .ShowScoreEffect(scorePoints, colorInfo);
            
            BreakParent();
            
            yield return lcAnimator.PlayClipAndWait("Destroying");
            
            if (simulation.AllowToWait()) 
                yield return time.Wait(.1f);    
        }

        #region ISerializable

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("explosion", explosion);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader); 
            reader.Read("explosion", ref explosion);
        }

        #endregion
    }
}