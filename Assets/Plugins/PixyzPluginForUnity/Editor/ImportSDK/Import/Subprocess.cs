using Pixyz.Plugin4Unity;
using Pixyz.Commons.UI.Editor;
using Pixyz.ImportSDK;

namespace Pixyz.ImportSDK
{
    public abstract class SubProcess : ActionIn<Importer> {

        public override int id { get { return 0; } }
        public override string menuPathRuleEngine { get { return null; } }
        public override string menuPathToolbox { get { return null; } }
        public override string tooltip { get { return "SubProcess"; } }

        public abstract void onBeforeDraw(ImportSettings importSettings);
        public abstract string name { get; }
    }
}