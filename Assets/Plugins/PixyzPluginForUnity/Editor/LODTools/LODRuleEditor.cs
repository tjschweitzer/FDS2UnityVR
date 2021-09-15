using UnityEditor;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.LODTools.Editor
{
    public class LODRuleEditor
    {

        [MenuItem("Pixyz/LOD/Create new LODRule", false, 121)]
        [MenuItem("Assets/Create/Pixyz/LOD/LODRule", false, 1)]
        public static LODRule CreateAsset()
        {
            return EditorExtensions.CreateAsset<LODRule>("", false);
        }
    }
}