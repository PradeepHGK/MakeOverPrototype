using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm.Coroutines;
using Yurowm.Serialization;
using Yurowm.Utilities;

namespace YMatchThree.Core {
    public class BombsBooster : LevelBooster {
        
        
        protected override IEnumerator Logic() {
            Field field = null;

            yield return puzzleSpace.context.Catch<Field>(f => field = f);

            var bomb = storage.GetItemByID<NodeBomb>("ChocolateBomb");

            using (var task = field.gameplay.NewExternalTask()) {
                yield return task.WaitAccess();
                
                var targets = field.slots.all.Values
                    .Where(s => s.GetCurrentContent() is Chip c && c.IsDefault)
                    .ToList();
                
                for (var i = 0; i < 3; i++) {
                    var target = targets.GrabRandom(random);
                    
                    if (!target) break;
                    
                    target.GetCurrentContent().HideAndKill();
                    
                    var c = bomb.Clone();
                    
                    c.emitType = SlotContent.EmitType.Script;
                    field.AddContent(c);
                            
                    target.AddContent(c);
                    c.localPosition = Vector2.zero;
                    
                    yield return new Wait(.2f);
                }
            }
            
            Redeem();
        }
    }
}