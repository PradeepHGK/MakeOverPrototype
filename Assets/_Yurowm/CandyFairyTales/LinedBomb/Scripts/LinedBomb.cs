using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm;
using Yurowm.Coroutines;
using Yurowm.Effects;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.Utilities;
using LineInfo = Yurowm.Effects.LinedExplosionLogicProvider.LineInfo;

namespace YMatchThree.Core {
    public class LinedBomb : BombChipBase, IColored {

        public ExplosionParameters explosion = new ExplosionParameters();
        
        #region Destroying
        
        public int waveOffset = 0;

        bool rotate = false;
        
        List<LineInfo> lineInfos = new List<LineInfo>();
        
        protected override bool CanBeHit(HitContext hitContext) {
            if (hitContext.reason == HitReason.BombExplosion 
                && hitContext.driver is LinedBomb linedBomb
                && linedBomb.side == side 
                && !linedBomb.rotate)
                rotate = true;
            return base.CanBeHit(hitContext);
        }

        public override IEnumerator Exploding() {
            var destroyingClip = lcAnimator.PlayClip("Destroying");
            
            var center = slotModule.Center;
            
            var hitGroup = slotModule.Slots().ToList();

            var slots = context.GetArgument<Slots>();
            
            var lines = SelectedSides()
                .SelectMany(s => Enumerator.For(-waveOffset, waveOffset, 1)
                    .Select(i => new Line(s, i)))
                .ToList();

            for (int step = 1; lines.Count > 0; step++) {
                for (int i = 0; i < lines.Count; i++) {
                    var line = lines[i];
                    
                    var coord = center 
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
                    
                    if (!field.Area.Contains(coord) || IsLineBreaker(slot)) {
                        lines.RemoveAt(i);
                        i--;
                    }
                }
            }
            
            hitGroup = hitGroup.NotNull().Distinct().ToList();
            
            var hitContext = new HitContext(context, hitGroup, HitReason.BombExplosion);
            
            hitContext.driver = this;

            field.Highlight(hitContext.group, slotModule.Slot());

            hitGroup.RemoveAll(s => slotModule.Has(s));

            for (int step = 1; hitGroup.Count > 0; step++) {
                hitGroup.RemoveAll(s => {
                    if (center.EightSideDistanceTo(s.coordinate) != step) return false;
                    
                    field.Explode(s.position, explosion);
                    s.HitAndScore(hitContext);
                    return true;
                });
                
                if (simulation.AllowToWait()) 
                    yield return time.Wait(hitSpeed);
            }

            yield return lcAnimator.StopAndWait(destroyingClip);
        }

        public static bool IsLineBreaker(Slot slot) {
            if (slot && slot.GetCurrentContent() is ILineBreaker lineBreaker)
                return lineBreaker.BreakTheLine();
            return false;
        }

        public override IEnumerable GetDestroyingEffectCallbacks() {
            yield return base.GetDestroyingEffectCallbacks();
            yield return new LinedExplosionLogicProvider.Callback {
                lines = lineInfos
            };
        }

        #endregion

        public struct Line {
            public Side direction;
            public int offset;

            public Line(Side direction, int offset) {
                this.direction = direction;
                this.offset = offset;
            }
        }
        
        #region Sides

        public Side side;
        public const float hitSpeed = .05f; // Slots per 'this value' seconds;

        IEnumerable<Side> SelectedSides() {
            var result = Sides.all.Where(s => side.HasFlag(s));
            if (rotate)
                result = result.Select(s => s.Rotate(2));
            return result;
        }

        #endregion

        #region ISerializalbe

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
    
    public interface ILineBreaker {
        bool BreakTheLine();
    }
}