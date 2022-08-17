using System.Collections;
using System.Collections.Generic;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Effects;
using Yurowm.Serialization;

namespace YMatchThree.Core {
    public class ExplosionMix : ChipMix {
        
        public int distance = 2;
        
        public ExplosionParameters explosion = new ExplosionParameters();

        protected override void Prepare(Chip centerChip, Chip secondChip) {
            if (context.SetupItem(out Score score)) {
                var points = 0;
                if (centerChip is IDestroyable d1) points += d1.scoreReward;
                if (secondChip is IDestroyable d2) points += d2.scoreReward;
                score.AddScore(points);
            }
            
            ApplyColor(centerChip, secondChip);
            
            centerChip.HideAndKill();
            secondChip.HideAndKill();
        }

        public override IEnumerator Logic() {
            var slots = context.GetArgument<Slots>();
            
            var hitGroup = new List<Slot>();
            for (int d = 0; d <= distance; d++) {
                foreach (var slot in slots.all.Values)
                    if (slot.coordinate.EightSideDistanceTo(this.slot.coordinate) == d)
                        hitGroup.Add(slot);
            }
            
            var hitContext = new HitContext(context, hitGroup, HitReason.BombExplosion);
            
            field.Highlight(hitGroup.ToArray(), this.slot, colorInfo);

            field.Explode(position, explosion);
            
            for (int d = 1; d <= distance; d++) {
                foreach (var slot in slots.all.Values)
                    if (slot.coordinate.EightSideDistanceTo(this.slot.coordinate) == d)
                        slot.HitAndScore(hitContext);
                yield return time.Wait(0.02f);
            }
        }

        #region ISerializable

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("distance", distance);
            writer.Write("explosion", explosion);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            reader.Read("distance", ref distance);
            reader.Read("explosion", ref explosion);
        }

        #endregion
    }
}