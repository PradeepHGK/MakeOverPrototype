using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Effects;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.Utilities;

namespace YMatchThree.Core {
    public abstract class NodeBombBase : BombChipBase {
        
        public string nodeEffect;
        
        public ExplosionParameters explosion = new ExplosionParameters();
        
        public override IEnumerator Exploding() {
            yield return EmitNodes(OnReachedTheTarget);
        }
        
        protected virtual IEnumerable<IEffectCallback> SetupNodeEffect() {
            yield break;
        }

        protected IEnumerator EmitNodes(Action<Effect, Slot, HitContext> onReachTarget) {
            
            var hitGroup = slotModule.Slots().ToList();
            
            if (nodeTargets.IsEmpty())
                nodeTargets = GetNodeTargets().ToArray();
            
            var targets = nodeTargets
                .Where(t => t != this)
                .Select(t => t.slotModule.Slot())
                .Distinct()
                .Shuffle(random)
                .ToList();
            
            hitGroup.AddRange(targets);
            
            var hitContext = new HitContext(context, hitGroup.Distinct(), HitReason.BombExplosion);
            
            field.Highlight(hitContext.group, slotModule.Slot());

            if (!nodeEffect.IsNullOrEmpty() && simulation.AllowEffects()) {
                var wait = 0;
                
                List<IEffectCallback> callbacks = new List<IEffectCallback>();
                
                foreach (var target in targets) {
                    var _target = target;
                    
                    Effect effect = null;
                    
                    callbacks.Reuse(SetupNodeEffect());
                    
                    callbacks.Add(new CompleteCallback {
                        onComplete = () => wait --
                    });
                    
                    wait ++;
                    
                    callbacks.Add(new NodeEffectItemProvider(target) {
                        onReachTarget = () => {
                            field.Explode(_target.position, explosion); 
                            onReachTarget(effect, _target, hitContext);
                        }
                    });
                    
                    effect = Effect.Emit(field, nodeEffect, position, callbacks.ToArray());
                    
                    field.AddContent(effect);
                    
                    OnCreateNodeEffect(effect);
                    
                    if (simulation.AllowToWait())
                        yield return null;
                }
                
                while (wait != 0)
                    yield return null;
                
                
                
            } else {
                targets.ForEach(t => field.Explode(t.position, explosion));
                targets.ForEach(t => onReachTarget(null, t, hitContext));
            }
        }
        
        protected SlotContent[] nodeTargets;
        
        protected abstract IEnumerable<SlotContent> GetNodeTargets();
        
        public virtual void OnCreateNodeEffect(Effect effect) {}
        
        public abstract void OnReachedTheTarget(Effect effect, Slot target, HitContext hitContext);

        #region ISerializable

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("nodeEffect", nodeEffect);
            writer.Write("explosion", explosion);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            reader.Read("nodeEffect", ref nodeEffect);
            reader.Read("explosion", ref explosion);
        }

        #endregion
    }
}