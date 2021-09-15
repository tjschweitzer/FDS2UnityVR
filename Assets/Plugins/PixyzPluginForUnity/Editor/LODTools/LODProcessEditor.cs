using UnityEditor;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.LODTools.Editor
{    
    public class LODProcessEditor
    {
        [MenuItem("Pixyz/LOD/Create new LODProcess", false, 121)]
        [MenuItem("Assets/Create/Pixyz/LOD/LODProcess", false, 0)]
        public static LODProcess CreateAsset()
        {
            return EditorExtensions.CreateAsset<LODProcess>("", false);
        }
    }
}