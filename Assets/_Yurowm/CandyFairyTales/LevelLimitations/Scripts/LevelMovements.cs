using System.Collections;
using UnityEngine;
using Yurowm.Serialization;

namespace YMatchThree.Core {
    public class LevelMovements : LevelLimitation {
        public int movesLeft = 15;
        
        public override IEnumerator GetVariblesTypes() {
            yield return base.GetVariblesTypes();
            yield return typeof (CountVariable);
        }

        public override void SetupVariable(ISerializable variable) {
            base.SetupVariable(variable);
            switch (variable) {
                case CountVariable count: {
                    if (count.value > 1) {
                        movesLeft = count.value;
                        Refresh();
                    }
                    return;
                } 
            }
        }

        protected override void UpdateCounter(LevelLimitationBody body) {
            body.label.text = movesLeft.ToString();
        }

        public override int GetUnits(int max) {
            return Mathf.Min(movesLeft, max);
        }

        public override void OnMove() {
            movesLeft = Mathf.Max(0, movesLeft - 1); 
            Refresh();
        }

        public override bool AllowToMove() {
            return movesLeft > 0;
        }
    }
}