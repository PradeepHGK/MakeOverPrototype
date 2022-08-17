using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yurowm;
using Yurowm.ContentManager;
using Yurowm.Extensions;

namespace YMatchThree.Core {
    public class SlotContentBody : SpaceObject, IReserved {
        public void Rollout() {
            if (this.SetupComponent(out ContentAnimator animator)) {
                animator.Rewind("Awake");
                animator.SendOnAnimate();
            }
        }
    }
}