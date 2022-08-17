using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.Utilities;
using static Yurowm.Effects.LinedExplosionLogicProvider;
using LineInfo = Yurowm.Effects.LinedExplosionLogicProvider.LineInfo;

namespace YMatchThree.Core {
    public class LineExplosionMix : ChipMix {
        
        public int waveOffset = 0;
        
        public ExplosionParameters explosion = new ExplosionParameters();

        List<LineInfo> lineInfos = new List<LineInfo>();
            
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
            var destroyingClip = lcAnimator.PlayClip("Destroying");
            
            var center = slot.coordinate;
            
            var hitGroup = new List<Slot>();

            var slots = context.GetArgument<Slots>();
            
            var lines = SelectedSides()
                .SelectMany(s => Enumerator.For(-waveOffset, waveOffset, 1)
                    .Select(i => new LinedBomb.Line(s, i)))
                .ToList();
            
            yield return time.Wait(.2f);   

            for (int step = 0; lines.Count > 0; step++) {
                for (int i = 0; i < lines.Count; i++) {
                    var line = lines[i];
                    
                    int2 coord = center 
                                 + line.direction.ToInt2() * step
                                 + line.direction.Rotate(2).ToInt2() * line.offset;
                    
                    var slot = slots.all.Get(coord);
                    
                    if (slot) {
                        hitGroup.Add(slot);
                        var lineInfo = lineInfos.FirstOrDefault(
                            i => i.Equal(line.direction, line.offset));
                        
                        if (lineInfo == null) {
                            lineInfo = new LineInfo(line.direction, line.offset);
                            lineInfos.Add(lineInfo);
                        }
                        
                        lineInfo.size = step.ClampMin(lineInfo.size);
                    }
                    
                    if (!field.Area.Contains(coord) || LinedBomb.IsLineBreaker(slot)) {
                        lines.RemoveAt(i);
                        i--;
                    }
                }
            }
            
            hitGroup = hitGroup.NotNull().Distinct().ToList();
            hitGroup.Remove(slot);
            
            var hitContext = new HitContext(context, hitGroup, HitReason.BombExplosion);

            field.Highlight(hitContext.group.ToArray(), slot, colorInfo);
            
            for (int step = 1; hitGroup.Count > 0; step++) {
                hitGroup.RemoveAll(s => {
                    if (center.EightSideDistanceTo(s.coordinate) != step) return false;
                    
                    field.Explode(s.position, explosion);
                    
                    s.HitAndScore(hitContext);
                    return true;
                });
                
                yield return time.Wait(LinedBomb.hitSpeed);
            }
            
            yield return lcAnimator.StopAndWait(destroyingClip);
        }

        public override IEnumerable GetEffectCallbacks() {
            yield return base.GetEffectCallbacks();
            yield return new Callback {
                lines = lineInfos
            };
        }

        #region Sides

        public Side side;

        IEnumerable<Side> SelectedSides() {
            return Sides.all.Where(s => side.HasFlag(s));
        }

        #endregion

        #region ISerializable

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("side", side);
            writer.Write("offset", waveOffset);
            writer.Write("explosion", explosion);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            reader.Read("side", ref side);
            reader.Read("offset", ref waveOffset);
            reader.Read("explosion", ref explosion);
        }

        #endregion
    }
}