using UnityEditor;

namespace Pixyz.Commons.Editor
{
    public abstract class SingletonEditorWindow : EditorWindow
    {
        public abstract string WindowTitle { get; }
    }
}