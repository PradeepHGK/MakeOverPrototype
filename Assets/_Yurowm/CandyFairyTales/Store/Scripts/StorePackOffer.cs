using System.Linq;
using Yurowm.Core;
using Yurowm.Offers;
using Yurowm.Serialization;

namespace Yurowm.Store {
    public abstract class StorePackOffer : UXOffer {
        
        public string packID;
        
        public override bool IsReady() {
            var pack = GetPack();
            
            if (pack == null)
                return false;
            
            var progressData = App.data.GetModule<StoreData>();

            if (progressData.HasKeychain(packID))
                return false;
            
            if (pack.GetAccessKeys().All(k => progressData.HasAccess(k)))
                return false;
            
            return true;
        }
        
        protected StorePack GetPack() {
            return StoreItem.storage
                .Items<StorePack>()
                .FirstOrDefault(p => p.ID == packID);
        }

        public override void Serialize(Writer writer) {
            base.Serialize(writer);
            writer.Write("packID", packID);
        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);
            reader.Read("packID", ref packID);
        }
    }
}