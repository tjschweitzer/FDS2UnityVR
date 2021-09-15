using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Pixyz.OptimizeSDK.Utils;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.Utilities;

namespace Pixyz.RuleEngine.Editor
{
    /// <summary>
    /// A RuleEngine set of Rules.
    /// </summary>
    public sealed class RuleSet : ScriptableObject {

        /// <summary>
        /// Triggered when a property has been changed.
        /// </summary>
        public VoidHandler changed;

        public void invokeChanged() {
            changed?.Invoke();
        }

        /// <summary>
        /// Callback function triggered everytime the importer has progressed.
        /// Always occurs in the main thread.
        /// </summary>
        public ProgressHandler progressed;

        public Action OnCompleted;

        private int _currentTotalBlocks = 0;
        private int _currentBlocksProcessed = 0;
        private object _currentData = null;
        private TaskCompletionSource<bool> _actionCompleted;

        public bool IsRunning
        {
            get;
            private set;
        }

        [SerializeField]
        private List<Rule> rules = new List<Rule>();

        public void run(bool forceSynchronous = false) {
            OptimizeSDK.Native.NativeInterface.PushAnalytic("RunRuleSet", "");
            IsRunning = true;
            SceneExtensionsEditor.AutoUnpack = false;
            /*
            Profiling.Start("RuleEngine");

            progressed?.Invoke(0, "Initializing...");

            _currentTotalBlocks = getTotalBlocksCount();

            int total = getTotalBlocksCount();
            int current = 0;
            try {
                for (int r = 0; r < rules.Count; r++) {
                    Rule rule = rules[r];
                    if (!rule.isEnabled)
                        continue;
                    object data = null;
                    for (int b = 0; b < rule.blocksCount; b++) {
                        RuleBlock block = rule.getBlock(b);
                        progressed?.Invoke(1f * current++ / total, $"{rule.name} > {block.action.displayNameRuleEngine} ({current}/{total})");
                        data = block.run(data);
                    }
                }
                progressed?.Invoke(1f, "Done !");
            } catch (Exception exception) {
                Debug.LogError("Rule Engine Exception : " + exception);
                progressed?.Invoke(1f, "Failure !");
            }
            */
            _ = RunRules(forceSynchronous);
        }

        public int getTotalBlocksCount() {
            int total = 0;
            for (int r = 0; r < rules.Count; r++) {
                for (int b = 0; b < rules[r].blocksCount; b++) {
                    total++;
                }
            }
            return total;
        }

        private async Task RunRules(bool forceSynchronous)
        {
            _currentTotalBlocks = getTotalBlocksCount();
            _currentBlocksProcessed = 0;
            try
            {
                foreach (Rule rule in rules)
                {
                    if (!rule.isEnabled)
                        continue;

                    foreach (RuleBlock ruleBlock in rule.Blocks)
                    {
                        ++_currentBlocksProcessed;
                        Dispatcher.StartCoroutine(Progress(1f * _currentBlocksProcessed / _currentTotalBlocks, $"{rule.name} > {ruleBlock.action.displayNameRuleEngine} ({_currentBlocksProcessed}/{_currentTotalBlocks})"));

                        if(!ruleBlock.action.invokePreProcess(_currentData))
                        {
                            throw new Exception($"[Action] Pre process failed.(<b>{ruleBlock.action.displayNameRuleEngine}</b> in <b>{rule.name}</b>)");
                        }

                        if (ruleBlock.action.isAsync && !forceSynchronous)
                        {
                            _actionCompleted = new TaskCompletionSource<bool>();
                            ruleBlock.action.completed.AddListener(ActionCompleted);

                            Task actionRun = Task.Factory.StartNew(() =>
                            {
                                ruleBlock.action.invoke(_currentData);
                                Dispatcher.StartCoroutine(ruleBlock.action.postProcessCoroutine());
                            });

                            await Task.WhenAll(actionRun, _actionCompleted.Task);

                            ruleBlock.action.completed.RemoveListener(ActionCompleted);
                            _currentData = ruleBlock.action.Output;
                        }
                        else
                        {
                            _currentData = ruleBlock.action.invoke(_currentData, forceSynchronous);
                        }

                        ruleBlock.action.Dispose();
                    }
                }

                IsRunning = false;
                progressed?.Invoke(1f, "Done !");
                OnCompleted?.Invoke();
                Profiling.EndAndPrint("RuleEngine");
            }
            catch (Exception exception)
            {
                IsRunning = false;
                Debug.LogError("Rule Engine Exception : " + exception);
                progressed?.Invoke(1f, "Failure !");
                OnCompleted?.Invoke();
                Profiling.EndAndPrint("RuleEngine");
            }
        }

        private IEnumerator Progress(float progress, string message)
        {
            yield return Dispatcher.GoMainThread();
            progressed?.Invoke(progress, message);
        }

        private void ActionCompleted()
        {
            _actionCompleted.SetResult(true);
        }

        private void OnEnable() {
            for (int r = 0; r < rules.Count; r++) {
                for (int b = 0; b < rules[r].blocksCount; b++) {
                    rules[r].getBlock(b).deserializeParameters();
                }
            }
        }

        public Rule getRule(int i)
        {
            return rules[i];
        }


        public void setRule(int i, Rule rule)
        {
            if (IsRunning)
            {
                Debug.LogError("[RuleSet] Rule can't be accessed while the ruleSet is processing rules..");
                return;
            }
            rules[i] = rule;
        }

        public int getRuleIndex(Rule rule)
        {
            if (IsRunning)
            {
                Debug.LogError("[RuleSet] Rule can't be accessed while the ruleSet is processing rules..");
                return -1;
            }
            return rules.IndexOf(rule);
        }

        public void removeRuleAt(int index)
        {
            if (IsRunning)
            {
                Debug.LogError("[RuleSet] Rule can't be accessed while the ruleSet is processing rules..");
                return;
            }
            rules.Remove(rules[index]);
        }

        public void removeRule(Rule rule)
        {
            if (IsRunning)
            {
                Debug.LogError("[RuleSet] Rule can't be accessed while the ruleSet is processing rules..");
                return;
            }
            rules.Remove(rule);
        }

        public void appendRule(Rule rule)
        {
            if (IsRunning)
            {
                Debug.LogError("[RuleSet] Rule can't be accessed while the ruleSet is processing rules..");
                return;
            }
            rules.Add(rule);
        }

        public int rulesCount => rules.Count;
    }
}
