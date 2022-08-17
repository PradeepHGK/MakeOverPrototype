using TMPro;
using Yurowm.ContentManager;

namespace YMatchThree.Core {
    public class LevelLimitationBody : SpaceObject, IReserved {
        public TMP_Text label;
        
        public void Rollout() {
            label.text = string.Empty;
        }
    }
}