using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Algo;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class ReplaceBy : PixyzFunction
    {
        public override int id => 58476964;
        public override int order => 10;
        public override string menuPathRuleEngine => "Hierarchy/Replace by...";
        public override string menuPathToolbox => "Hierarchy/Replace by...";
        public override string tooltip => ToolboxTooltips.replaceByAction;

        public enum ReplaceByMode
        {
            GameObject,
            BoundingBox,
        }

        private bool isReplaceByBB() => replaceBy == ReplaceByMode.BoundingBox;
        private bool isReplaceByGO() => replaceBy == ReplaceByMode.GameObject;

        private bool checkReplaceMode()
        {
            if (replaceBy == ReplaceByMode.BoundingBox)
                _isAsync = true;
            else
                _isAsync = false;

            return true;
        }

        [UserParameter("checkReplaceMode")]
        public ReplaceByMode replaceBy = ReplaceByMode.GameObject;

        [UserParameter("isReplaceByBB")]
        public ReplaceByBoxType boundingBox = ReplaceByBoxType.LocallyAligned;

        [UserParameter("isReplaceByGO")]
        public GameObject gameobject = null;

        [UserParameter(tooltip: ToolboxTooltips.replaceByRotation)]
        public bool replaceRotation;

        [UserParameter(tooltip: ToolboxTooltips.replaceByScale)]
        public bool replaceScale;


        public override bool preProcess(IList<GameObject> input)
        {
            if(replaceBy == ReplaceByMode.GameObject)
            {
                _input = input;
                _output = _input;
                return true;
            }
            else
            {
                return base.preProcess(input);
            }
        }

        protected override void process()
        {            
            if(replaceBy == ReplaceByMode.BoundingBox)
            {
                NativeInterface.PushAnalytic("ReplaceByBox", "");
                NativeInterface.ReplaceByBox(Context.pixyzMeshes,Context.pixyzMatrices, boundingBox);
            }
        }

        protected override void postProcess()
        {
            if(replaceBy == ReplaceByMode.GameObject)
            {
                for (int i = 0; i < _input.Count; ++i)
                {
                    Transform transform = _input[i].transform;
                    var localPosition = transform.localPosition;
                    var localScale = transform.localScale;
                    var localRotation = transform.localRotation;
                    var parent = transform.parent;

                    SceneExtensionsEditor.DestroyImmediateSafe(transform.gameObject);

                    var goRotation = gameobject.transform.localRotation;
                    var goScale = gameobject.transform.localScale;

                    _input[i] = SceneExtensions.Instantiate(gameobject);
                    transform = _input[i].transform;
                    transform.parent = parent;
                    transform.localPosition = localPosition;
                    transform.localRotation = replaceRotation ? goRotation : localRotation;
                    transform.localScale = replaceScale ? goScale : localScale;
                }
            }
            else
            {
                GameObject[] outputParts = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);
                ReplaceInHierarchy(InputParts, outputParts);
            }
        }

        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (isReplaceByGO() && gameobject == null)
            {
                errors.Add("Gameobject field must be set !");
            }
            return errors;
        }
    }
}

