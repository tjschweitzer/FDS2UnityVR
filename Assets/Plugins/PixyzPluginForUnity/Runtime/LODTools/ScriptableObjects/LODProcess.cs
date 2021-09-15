using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Pixyz.LODTools
{
    [Serializable]
    public class RulesThresholds
    {
        [SerializeField]
        public List<double> thresholds = new List<double> { 0.8, 0.0 };

        public void AddThreshold(double t)
        {
            thresholds.Add(t);
            thresholds.Sort((a, b) => b.CompareTo(a));
        }
    }

    public class LODProcess : ScriptableObject
    {
        [SerializeField]
        private List<LODRule> _rules = new List<LODRule>();

        [SerializeField]
        private List<int> _sources = new List<int>();

        [SerializeField]
        private RulesThresholds rulesThresholds = new RulesThresholds();

        /// <summary>
        /// Rules that will apply for each LOD level, index represent the LOD level
        /// </summary>
        public ReadOnlyCollection<LODRule> Rules => _rules.AsReadOnly();

        /// <summary>
        /// The source used as input for the matching rule
        /// </summary>
        public ReadOnlyCollection<int> Sources => _sources.AsReadOnly();

        /// <summary>
        /// The thresholds associated to each rule
        /// </summary>
        public ReadOnlyCollection<double> Thresholds => rulesThresholds.thresholds.AsReadOnly();
		
        /// <summary>
        /// Create an instance of this scriptable object with the possibility to generate a default process (decim, decim, remesh)
        /// </summary>
        /// <param name="useDefault"></param>
        /// <returns></returns>
        public static LODProcess CreateInstance(bool useDefault = false)
        {
            if(useDefault)
            {
                LODProcess process = ScriptableObject.CreateInstance<LODProcess>();
                process.name = "Default";
                process.AddRule(LODRule.CreateInstance(1), 0.50, 0);
                process.AddRule(LODRule.CreateInstance(2), 0.30, 0);
                process.AddRule(LODRule.CreateInstance(3), 0.15, 0);

                return process;
            }

            return ScriptableObject.CreateInstance<LODProcess>();
        }

        /// <summary>
        /// Compute the hash for this LODProcess
        /// </summary>
        /// <returns></returns>
        public long ComputeHash()
        {
            long hash = 1;

            foreach(LODRule rule in _rules)
            {
                hash ^= rule.ComputeHash();
            }

            return hash;
        }

        /// <summary>
        /// Add a new rule to this process
        /// </summary>
        /// <param name="rule">The rule to add</param>
        /// <param name="threshold">The threashold of the associated lod in the LOD Group</param>
        /// <param name="source">The source that will be used as source for the provided rule. Must be an index of the current rule list</param>
        public void AddRule(LODRule rule, double threshold, int source)
        {
            if (source > 0 && source >= _rules.Count)
                return;

            _rules.Add(rule);
            rulesThresholds.AddThreshold(threshold);
            _sources.Add(source);
        }

        /// <summary>
        /// Remove the rule from this process
        /// </summary>
        /// <param name="rule"></param>
        public void RemoveRule(LODRule rule)
        {
            int index = _rules.IndexOf(rule);

            if(index != -1)
            {
                _rules.RemoveAt(index);
                _sources.RemoveAt(index);
                rulesThresholds.thresholds.RemoveAt(index);
            }
        }

        /// <summary>
        /// Change the source for the rule at the provided index to the provided source
        /// </summary>
        /// <param name="lodIndex"></param>
        /// <param name="sourceIndex"></param>
        public void SetLODSource(int lodIndex, int sourceIndex)
        {
            if (lodIndex > _rules.Count || sourceIndex > _rules.Count)
                return;

            _sources[lodIndex] = sourceIndex;
        }

        /// <summary>
        /// Set a new rule at the provided index
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="indexToSwap"></param>
        public void SwapRule(LODRule rule, int indexToSwap)
        {
            if (indexToSwap < _rules.Count)
                _rules[indexToSwap] = rule;
        }

        /// <summary>
        /// Clean the rule list to avoid null ref
        /// </summary>
        public void CheckRulesExistence()
        {
            for(int i = 0; i < _rules.Count; i++)
            {
                if (_rules.Count == 0)
                    break;
                if(_rules[i] == null)
                {
                    _rules.RemoveAt(i);
                    _sources.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}