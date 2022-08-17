using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm.Effects;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.Utilities;

namespace YMatchThree.Core {
    public class NodeBomb : NodeBombBase, IMixableChip {
        
        protected override IEnumerable<IEffectCallback> SetupNodeEffect() {
            yield return new NodeEffectLinearInterpolator(random.Range(3f, 9f), 0);
        }

        protected override IEnumerable<SlotContent> GetNodeTargets() {
            return field.slots.all.Values
                .Select(s => s.GetCurrentContent())
                .NotNull()
                .Where(c => c != this && c is IDestroyable 
                    && c.birthDate < gameplay.matchDate
                    && TargetFilter(c));
        }
        
        ItemColorInfo targetColor = ItemColorInfo.None;
        
        protected virtual bool TargetFilter(SlotContent content) {
            if (targetColor == ItemColorInfo.None)
                targetColor = field.slots.all.Values
                    .Where(s => s.visible)
                    .Select(s => s.GetCurrentContent() as IColored)
                    .Where(c => c != null && c.colorInfo.IsMatchableColor())
                    .GetRandom(random)
                    .colorInfo;

            return content is IColored c && c.colorInfo.IsMatchWith(targetColor);
        }

        public override void OnCreateNodeEffect(Effect effect) {
            base.OnCreateNodeEffect(effect);
            effect.body.gameObject.Repaint(gameplay.colorPalette, colorInfo);
        }

        public override void OnReachedTheTarget(Effect effect, Slot target, HitContext hitContext) {
            target.HitAndScore(hitContext);
        }

        #region IMixableChip

        public int MixPriority { get; set; }

        Chip secondMixChip;

        public void PrepareMixWith(Chip secondChip) {
            secondMixChip = secondChip;
            if (secondChip is IColored c && c.colorInfo.IsMatchableColor())
                targetColor = c.colorInfo;
        }

        public IEnumerator MixLogic() {
            if (secondMixChip is NodeBombBase) {
                nodeTargets = context.GetArgument<Slots>().all.Values
                    .Select(s => s.GetCurrentContent())
                    .NotNull()
                    .Where(c => c != this && c is IDestroyable 
                        && c.birthDate < gameplay.matchDate)
                    .ToArray();
                
                yield return EmitNodes(OnReachedTheTarget);
                
            } else if (secondMixChip is BombChipBase bomb) {
                var bombInfo = new ContentInfo(bomb);
                
                var bombs = new List<BombChipBase>();
                var field = this.field;

                yield return EmitNodes((effect, target, hitContext) => {
                    if (target == secondMixChip.slotModule.Slot()) return;
                    
                    var slot = target;

                    var newBomb = bombInfo.Reference.Clone() as BombChipBase;
                    
                    var color = target.GetCurrentColor();
                    
                    if (color.IsKnown())
                        newBomb.SetupVariable(new ColoredVariable {
                            info = color
                        });

                    var currentContent = target.GetCurrentContent();
                    
                    currentContent.HideAndKill();
                    
                    field.AddContent(newBomb);

                    slot.AddContent(newBomb);
                    
                    newBomb.localPosition = Vector2.zero;
                    
                    bombs.Add(newBomb);
                });

                var hc = new HitContext(context,
                    bombs.Select(b => b.slotModule.Slot()).ToList(),
                    HitReason.BombExplosion);
                
                yield return time.Wait(.2f);
                
                while (!bombs.IsEmpty()) {
                    bombs.RemoveAll(b => b.destroying || !b.IsAlive());
                    bombs.GetRandom(random)?.HitAndScore(hc);
                    yield return time.Wait(.1f);
                }
            } else
                yield return Destroying();
            
            if (!destroying) 
                HideAndKill();
        }
        
        #endregion

        #region ISerializable

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("MixPriority", MixPriority);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            MixPriority = reader.Read<int>("MixPriority");
        }

        #endregion
    }
}