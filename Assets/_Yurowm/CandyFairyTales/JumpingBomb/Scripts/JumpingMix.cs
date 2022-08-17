using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Effects;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.Utilities;

namespace YMatchThree.Core {
    public class JumpingMix : ChipMix {
        ItemColorInfo colorInfo;
        
        public int count = 3;
        
        public ExplosionParameters startExplosion = new ExplosionParameters();
        public ExplosionParameters endExplosion = new ExplosionParameters();
        
        JumpingBomb.SlotRating rating;
        
        protected override void Prepare(Chip centerChip, Chip secondChip) {
            if (secondChip is IColored c1)
                colorInfo = c1.colorInfo;
            else if (centerChip is IColored c2)
                colorInfo = c2.colorInfo;
            else
                colorInfo = ItemColorInfo.ByID(context.GetArgument<LevelColorSettings>().GetRandomColorID());
            
            centerChip.HideAndKill();
            secondChip.HideAndKill();
            
            rating = JumpingBomb.SlotRating.CreateOrFind(context);
            rating.Refresh();
        }
        
        public override IEnumerator Logic() {
            var targets = new List<Slot>();
            
            for (var i = 0; i < count; i++) { 
                var target = rating.GetTopRated().GetRandom(random);
                if (target) {
                    rating.SetBusy(target, true);
                    targets.Add(target); 
                } else
                    break;
            }
            
            if (targets.IsEmpty())
                yield break;
            
            yield return targets.Select(Jump).Parallel();
        }

        
        IEnumerator Jump(Slot target) {
            if (!target) yield break;
            
            var effectCallback = new TransitionEffectLogicProvider.Callback();
            var repaintCallback = new RepaintEffectLogicProvider.Callback();
            repaintCallback.colorInfo = colorInfo;
            
            effectCallback.destinationPoint = target.position;
            
            effectCallback.onComplete = () => {
                rating.SetBusy(target, false);
                
                field.Explode(position, endExplosion);
            
                field.Highlight(new [] { target }, target);
                
                target.HitAndScore(new HitContext(context, target, HitReason.BombExplosion));
            };
            
            var effect = Effect.Emit(field, effectName, position, 
                effectCallback, repaintCallback);
            
            if (!effect) {
                rating.SetBusy(target, false);
                yield break;
            }
            
            field.Explode(position, startExplosion);

            while (effect.IsAlive())
                yield return null;
        }

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("count", count);
            writer.Write("startExplosion", startExplosion);
            writer.Write("endExplosion", endExplosion);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            reader.Read("count", ref count);
            reader.Read("startExplosion", ref startExplosion);
            reader.Read("endExplosion", ref endExplosion);
        }
    }
}