using Pixyz.Plugin4Unity;
using Pixyz.Commons.UI.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace Pixyz.RuleEngine.Editor
{
    public class SetTag : ActionInOut<IList<GameObject>, IList<GameObject>> {

        [UserParameter]
        public string tagName;

        public override int id => 16465110;
        public override string menuPathRuleEngine => "Set/Tag";
        public override string menuPathToolbox => null;
        public override string tooltip => "Set GameObjects' tag";

        public override IList<GameObject> run(IList<GameObject> input) {
            if (!Configuration.CheckLicense()) throw new NoValidLicenseException();

#if UNITY_EDITOR
            var tags = ReferencableEditorUtils.GetTags();
            if (!tags.Contains(tagName))
                ReferencableEditorUtils.AddTag(tagName);
#endif

            for (int i = 0; i < input.Count; i++) {
                input[i].tag = tagName;
            }
            return input;
        }
    }
}