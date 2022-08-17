using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YMatchThree.Seasons;
using Yurowm;
using Yurowm.ComposedPages;
using Yurowm.Core;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Integrations;
using Yurowm.Serialization;
using Yurowm.UI;
using Yurowm.Utilities;
using Behaviour = Yurowm.Behaviour;
using Button = Yurowm.Button;
using Page = Yurowm.UI.Page;
using Space = Yurowm.Spaces.Space;

namespace YMatchThree.Core {
    public static class GameBinder {
        const string bindKeyFormat = "Bind.{0}";
        
        [OnLaunch(Behaviour.INITIALIZATION_ORDER + 1)]
        public static void Launch() {
            Bind();
        }

        #region Processing
        
        static bool processing = false;
        static GameObject processingPanel;
        public static bool Processing {
            get => processing;
            set {
                if (processing != value || !processingPanel) {
                    if (!processingPanel)
                        processingPanel = ObjectTag.Get("Processing");
                    processing = value;
                    processingPanel.SetActive(processing);
                    if (processing)
                        processingPanel.transform.SetAsLastSibling();
                }
            }
        }

        #endregion
        
        static void Bind() {
            BindButtons("Next", PuzzleSpace.NextLevel);

            BindButtons("Close", PuzzleSpace.Close);
            
            BindButtons("Restart", PuzzleSpace.Restart);

            BindButtons("Pause", () => Page.Get("Pause"));
            
            BindButtons("Settings", () => 
                ComposedPage.ByID("Sidebar").Show(new SettingsPage()));
        }
        
        static void SetVisisbleButtons(string key, bool visible) {
            Behaviour.GetAllByID<Button>(bindKeyFormat.FormatText(key))
                .ForEach(b => b.gameObject.SetActive(visible));
        }
        
        static void BindButtons(string key, Action onClick) {
            Behaviour.GetAllByID<Button>(bindKeyFormat.FormatText(key))
                .ForEach(b => b.onClick.AddListener(onClick.Invoke));
        } 
    
    }
}