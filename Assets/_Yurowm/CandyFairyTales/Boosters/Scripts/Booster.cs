using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm;
using Yurowm.ContentManager;
using Yurowm.Core;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Localizations;
using Yurowm.Serialization;
using Yurowm.Store;
using Yurowm.UI;
using Behaviour = Yurowm.Behaviour;
using Space = Yurowm.Spaces.Space;

namespace YMatchThree.Core {
    public abstract class Booster : LevelContent, ILocalized, IVirtualizedScrollItem {
        
        public string buttonName;
        public string iconName;
        public string animationName;

        public override Type GetContentBaseType() {
            return typeof(Booster);
        }

        public virtual void SetupBody(VirtualizedScrollItemBody body) {
            if (!(body is BoosterButton bb))
                return;
            
            bb.booster = this;
            
            if (bb.icon) {
                var boosterIcon = AssetManager.GetAsset<Sprite>(iconName);
                
                bb.icon.sprite = boosterIcon;
                bb.icon.color = boosterIcon ? Color.white : Color.clear;
            }

            var boosterCount = App.data.GetModule<Inventory>().GetItemCount(ID);

            bb.empty?.SetActive(boosterCount <= 0);
            if (bb.count) {
                bb.count.gameObject.SetActive(boosterCount > 0);
                bb.count.text = boosterCount.ToString();
            }
        }

        public string GetBodyPrefabName() {
            return buttonName;
        }

        public bool CanBeRedeemed() {
            return App.data
                .GetModule<Inventory>()
                .GetItemCount(ID) > 0;
        }
        
        public bool Redeem() {
            if (!CanBeRedeemed())
                return false;
            
            App.data.GetModule<Inventory>().SpendItems(ID, 1);
            scriptEvents.onRedeemBooseter?.Invoke(this);
            UIRefresh.Invoke();

            return true;
        }
        
        protected ContentAnimator animator;
        
        public abstract void OnClick();
        
        public IEnumerator PlayAnimation(Action<string> onCallback) {
            var body = AssetManager.Emit<BoosterAnimationBody>(animationName, context);
            
            if (!body) yield break;
            
            var called = false;
            
            void Call(string _ = null) {
                called = true;
                onCallback?.Invoke(_);
            }
            
            body.transform.SetParent(field.root);
            body.transform.Reset();
            body.transform.localPosition = position;
            
            body.callback = Call;
            
            if (body.SetupComponent(out animator))
                yield return animator.PlayAndWait("Hit");
            
            if (!called)
                Call();
            
            body.Kill();
        }
        
        public IEnumerator ShowInStore() {
            yield return Page.Get("Store")?.ShowAndWait();
            
            var list = Behaviour.Get<StoreList>();
            
            if (!list) yield break;
            
            var targetItem = StoreItem.storage
                .Items<StorePack>()
                .FirstOrDefault(i => i.items
                    .Any(i => i is StorePackItemCount spic && spic.itemID == ID));

            list.CenterTo(targetItem, 1);
        }
        
        #region Localizion

        public virtual IEnumerable GetLocalizationKeys() {
            yield break;
        }
        
        #endregion
        
        #region ISerializable
        
        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("iconName", iconName);
            writer.Write("animationName", animationName);
            writer.Write("buttonName", buttonName);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            reader.Read("iconName", ref iconName);
            reader.Read("animationName", ref animationName);
            reader.Read("buttonName", ref buttonName);
        }

        #endregion
    }
    
    public class BoosterSetVariable : ContentInfoVariable {
        
        public List<string> IDs = new List<string>();

        public override void Serialize(Writer writer) {
            writer.Write("set", IDs.ToArray());
        }

        public override void Deserialize(Reader reader) {
            IDs.Reuse(reader.ReadCollection<string>("set"));
        }
    }
}