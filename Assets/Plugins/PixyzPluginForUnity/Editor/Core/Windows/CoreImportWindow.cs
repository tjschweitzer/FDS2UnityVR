using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pixyz.Plugin4Unity;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.UI.Editor;
using System;
using Pixyz.ImportSDK;
using Pixyz.Plugin4Unity.Core;
#if PIXYZ_RULE_ENGINE
using Pixyz.RuleEngine.Editor;
#endif

namespace Pixyz.Plugin4Unity.EditorWindows
{
    public class CoreImportWindow : ImportWindow
    {
#if PIXYZ_RULE_ENGINE
        public RuleSet RuleSet;
#endif
        public static new void Open(string file)
        {
            var window = EditorExtensions.OpenWindow<CoreImportWindow>();
            window._fileToImport = file;

            if (Preferences.AutomaticUpdate)
                UpdateWindow.AutoPopup();
        }
        protected override void DrawPostProcess()
        {
#if PIXYZ_RULE_ENGINE

                RuleSet = (RuleSet)EditorGUILayout.ObjectField(new GUIContent("Rules", "If a RuleEngine set of rule (RuleSet) is referenced here, it will be used to automatically process the imported data.\n" +
        "The processing occurs before the prefab creation (if the prefab option is ticked)."), RuleSet, typeof(RuleSet), false);
#endif
            base.DrawPostProcess();
        }

        protected override void RunAdditionalPostProcess()
        {
            base.RunAdditionalPostProcess();
            
#if PIXYZ_RULE_ENGINE
            // Rules
            if (RuleSet != null)
            {

#if UNITY_2020_1_OR_NEWER
                _progressBar = new BackgroundProgressBar(null, "Processing Rules...");
#else
                _progressBar = new ProgressBar(null, "Processing Rules...");
#endif
                try
                {
                    RuleSet.progressed = delegate { };
                    RuleSet.progressed += rulesProgressed;
                    RuleSet.OnCompleted += () => { postProcessOver?.Invoke(); };
                    RuleSet.run();
                }
                catch (Exception rulesException)
                {
                    /// An exception has occured in the rules
                    Debug.LogException(rulesException);
                    _progressBar.SetProgress(1f, "Failed");
                }
            }
            else
            {
                postProcessOver?.Invoke();
            }
#endif
        }

        private void rulesProgressed(float progress, string message)
        {
            _progressBar.SetProgress(progress, message);
            
        }
    }
}
