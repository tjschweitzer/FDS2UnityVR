using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Pixyz.Commons.Utilities;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Commons.UI.Editor
{
    /// <summary>
    /// Base class for making input-storable and invokable Actions.
    /// This is the very base class for all RuleEngine and Toolbox Actions.
    /// </summary>
    public abstract class ActionBase {

        /// <summary>
        /// The id of the Acton implementation. This id should be unique and is readonly (written in code).
        /// The id is the actual reference if the Action is serialized (in a set of RuleEngine Rules for example), hence, this shouldn't be changed if possible when someone uses it.
        /// </summary>
        public abstract int id { get; }

        /// <summary>
        /// The order to use when placing this action in a list.
        /// If the value is -1, the display name will be used instead;
        /// </summary>
        public virtual int order => -1;

        /// <summary>
        /// The menu path for the RuleEngine.
        /// Use forward slashes for submenus (ex:"Custom/My Custom Action")
        /// If left null or empty, this Action won't be listed in the RuleEngine.
        /// </summary>
        public virtual string menuPathRuleEngine { get; }

        /// <summary>
        /// The menu path for the Toolbox.
        /// Use forward slashes for submenus (ex:"Custom/My Custom Action")
        /// If left null or empty, this Action won't be listed in the Toolbox.
        /// </summary>
        public virtual string menuPathToolbox { get; }

        /// <summary>
        /// Returns true if this Action is available trough the RuleEngine.
        /// </summary>
        public bool isInRuleEngine => !string.IsNullOrEmpty(menuPathRuleEngine);

        /// <summary>
        /// Returns true if this Action is available trough the Toolbox.
        /// </summary>
        public bool isInToolbox => !string.IsNullOrEmpty(menuPathToolbox) && inputType == typeof(IList<GameObject>) && outputType == typeof(IList<GameObject>);

        /// <summary>
        /// Returns the tooltip.
        /// </summary>
        public virtual string tooltip { get; }

        /// <summary>
        /// Specifies what kind of data enters your actions.
        /// </summary>
        public abstract Type inputType { get; }

        /// <summary>
        /// Specifies what kind of data the actions outputs.
        /// </summary>
        public abstract Type outputType { get; }


        /// <summary>
        /// The data that had entered your actions.
        /// </summary>
        public virtual object Input { get; protected internal set; }

        /// <summary>
        ///The data your action ouputed
        /// </summary>
        public virtual object Output { get; protected internal set; }

        /// <summary>
        /// Eventual fields.
        /// Fields can be added for an Action implementation thanks to the attribute @link Pixyz.Tools.UserParameter @endlink.
        /// </summary>
        public FieldInstance[] fieldInstances { get; protected internal set; }

        /// <summary>
        /// Eventual helper methods.
        /// Helper methods can be added for an Action implementation thanks to the attribute @link Pixyz.Tools.HelperMethod @endlink.
        /// </summary>
        public MethodInfo[] helpersMethods { get; protected internal set; }

        /// <summary>
        /// Undo Action.
        /// </summary>
        public virtual Action undo { get; private set; }

        /// <summary>
        /// Should the action be fired in as a Task
        /// </summary>
        public virtual bool isAsync => false;


        protected bool _isRunning = false;
        protected string _stepName = "";
        protected float _stepProgress = 0.0f;

        public bool IsRunning => _isRunning;
        public string StepName => _stepName;
        public float StepProgress => _stepProgress;

        public UnityEvent completed = new UnityEvent();
        public UnityEvent progressChanged = new UnityEvent();

        /// <summary>
        /// Virtual method for Error checks before execution.
        /// Returning a non null string array will display each string as an Error on the RuleEngine and Toolbox.
        /// </summary>
        /// <returns></returns>
        public virtual IList<string> getErrors() {
            return null;
        }

        public List<string> executionErrors = new List<string>();

        /// <summary>
        /// Virtual method for Warning checks before execution.
        /// Returning a non null string array will display each string as a Warning on the RuleEngine and Toolbox.
        /// </summary>
        /// <returns></returns>
        public virtual IList<string> getWarnings() {
            return null;
        }

        /// <summary>
        /// Virtual method for Info checks before execution.
        /// Returning a non null string array will display each string as an Information on the RuleEngine and Toolbox.
        /// </summary>
        /// <returns></returns>
        public virtual IList<string> getInfos() {
            return null;
        }

        /// <summary>
        /// Virtual method for implementing operations before the Action gets drawn in the RuleEngine or the Toolbox.
        /// </summary>
        public virtual void onBeforeDraw() { }

        public virtual void onAfterDraw() { }

        protected virtual void postProcess() { }

        public IEnumerator postProcessCoroutine()
        {
            yield return Dispatcher.GoMainThread();
            try
            {
                postProcess();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
                executionErrors.Add(e.Message);
            }
            completed.Invoke();
        }

        public virtual void Dispose() { }

        public virtual void onSelectionChanged(IList<GameObject> selection) { }

        /// <summary>
        /// Initialize parameters from RuleEngineParameters attributes.
        /// </summary>
        public virtual void initialize() {
            List<FieldInstance> fieldInstancesList = new List<FieldInstance>();
            foreach (FieldInfo fieldInfo in GetType().GetFields()) {
                UserParameter userParameter;
                if (fieldInfo.HasAttribute(out userParameter)) {
                    fieldInstancesList.Add(new FieldInstance(fieldInfo, this, userParameter.visibilityCheck, userParameter.enablingCheck, userParameter.tooltip, userParameter.displayName));
                }
            }
            fieldInstances = fieldInstancesList.ToArray();
            //fieldInstances = GetType().GetFields().Where(x => x.HasAttribute<UserParameter>()).Select(x => new FieldInstance(x, this)).ToArray();
            helpersMethods = GetType().GetMethods().Where(x => x.HasAttribute<HelperMethod>()).ToArray();
        }

        protected void UpdateProgressBar(float value, string name = "Processing...")
        {
            Dispatcher.StartCoroutine(UpdateProgressBarCoroutine(value, name));
        }

        protected IEnumerator UpdateProgressBarCoroutine(float value, string name = "Processing...")
        {
            yield return Dispatcher.GoMainThread();
            //EditorUtility.DisplayProgressBar("Pixyz", name, value);
            _stepName = name;
            _stepProgress = value;
            progressChanged.Invoke();
        }

        /// <summary>
        /// Returns the display name (without menu path).
        /// </summary>
        public string displayNameRuleEngine {
            get {
                return menuPathRuleEngine.Replace("/", " > ");
            }
        }

        /// <summary>
        /// Returns the display name (without menu path)
        /// </summary>
        public string displayNameToolbox {
            get {
                return menuPathToolbox.Replace("/", " > ");
            }
        }

        /// <summary>
        /// Runs the action through reflection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object invoke(object data, bool forceSynchronous = false) {

            /// Takes the generic type
            /// Builds an instance an run !
            Type genericType;

            try {
                if (inputType == null && outputType == null)
                {
                    typeof(ActionVoid).GetMethod(isAsync && !forceSynchronous ? "runAsync" : "run").Invoke(this, new object[0]);
                    return null;
                }

                if (inputType == null) 
                {
                    genericType = typeof(ActionOut<>).MakeGenericType(new Type[] { outputType });
                    return genericType.GetMethod(isAsync && !forceSynchronous ? "runAsync" : "run").Invoke(this, new object[0]);
                }

                if (outputType == null) 
                {
                    genericType = typeof(ActionIn<>).MakeGenericType(new Type[] { inputType });
                    if(isAsync && !forceSynchronous)
                    {
                        genericType.GetMethod("runAsync").Invoke(this, new object[0]);
                    }
                    else
                    {
                        genericType.GetMethod("run").Invoke(this, new object[] { data });
                    }
                    return null;
                }

                genericType = typeof(ActionInOut<,>).MakeGenericType(new Type[] { inputType, outputType });
                if(isAsync && !forceSynchronous)
                {
                    return genericType.GetMethod("runAsync").Invoke(this, new object[0]);
                }
                else
                {
                    return genericType.GetMethod("run").Invoke(this, new object[] { data });
                }

            } catch (TargetInvocationException exception) {
                if (exception.InnerException is PrefabModificationException) {
                    Debug.LogError($"The Action '{displayNameToolbox ?? displayNameRuleEngine}' failed since at least one of the input is linked to a prefab.\nPlease click 'Unpack Prefab' on the concerned GameObjects and try again.");
                    return null;
                }
                throw exception;
            }
        }

        public bool invokePreProcess(object data)
        {
            /// Takes the generic type
            /// Builds an instance an run !
            Type genericType;

            try
            {
                if (inputType == null && outputType == null)
                {
                    return (bool)typeof(ActionVoid).GetMethod("preProcess").Invoke(this, new object[0]);
                }

                if (inputType == null)
                {
                    genericType = typeof(ActionOut<>).MakeGenericType(new Type[] { outputType });
                    return (bool)genericType.GetMethod("preProcess").Invoke(this, new object[0]);
                }

                if (outputType == null)
                {
                    genericType = typeof(ActionIn<>).MakeGenericType(new Type[] { inputType });
                    return (bool)genericType.GetMethod("preProcess").Invoke(this, new object[] { data });
                }

                genericType = typeof(ActionInOut<,>).MakeGenericType(new Type[] { inputType, outputType });
                return (bool)genericType.GetMethod("preProcess").Invoke(this, new object[] { data });

            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException is PrefabModificationException)
                {
                    Debug.LogError($"The Action '{displayNameToolbox ?? displayNameRuleEngine}' failed since at least one of the input is linked to a prefab.\nPlease click 'Unpack Prefab' on the concerned GameObjects and try again.");
                    return false;
                }
                throw exception;
            }
        }
    }

    public class PrefabModificationException : Exception {

    }

    /// <summary>
    /// Abstract class for In-Out Actions.<br/>
    /// Inherit from this class to create an Action that returns some data from a given input.<br/>
    /// Can be used to create a Toolbox Actions and/or RuleEngine Actions.<br/>
    /// Check <see cref="ActionBase"/> for more implementation details.<br/>
    /// <remarks>If creating a Toolbox Action, the Input and Output must be of type IList&lt;GameObject&gt;.</remarks>
    /// </summary>
    /// <typeparam name="Input">Specifies what kind of data enters your actions.</typeparam>
    /// <typeparam name="Output">Specifies what kind of data the actions outputs.</typeparam>
    public abstract class ActionInOut<Input, Output> : ActionBase
    {
        /// <summary>
        /// Input type. Specifies what kind of data enters your actions.
        /// </summary>
        public override Type inputType => typeof(Input);
        /// <summary>
        /// Output type. Specifies what kind of data the actions outputs.
        /// </summary>
        public override Type outputType => typeof(Output);

        public virtual bool preProcess(Input input) { return true; }
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code.
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        /// <returns>Output data, of the given outputType</returns>
        public abstract Output run(Input input);
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code. (should be run async)
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        /// <returns>Output data, of the given outputType</returns>
        public virtual void runAsync()
        {
            throw new Exception("[Action] This action does not support async mode");
        }
    }

    /// <summary>
    /// Abstract class for Out Actions.<br/>
    /// Inherit from this class to create an Action that returns some data.<br/>
    /// Can be used to create a RuleEngine Actions (starting points).<br/>
    /// </summary>
    /// <typeparam name="Output">Specifies what kind of data the actions outputs.</typeparam>
    public abstract class ActionOut<Output> : ActionBase {
        /// <summary>
        /// Input type. This is always null in the case of a Out-only Action.
        /// </summary>
        public override Type inputType => null;
        /// <summary>
        /// Output type. Specifies what kind of data the actions outputs.
        /// </summary>
        public override Type outputType => typeof(Output);
        public virtual bool preProcess() { return true; }
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code.
        /// </summary>
        /// <returns>Output data, of the given outputType</returns>
        public abstract Output run();
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code. (should be run async)
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        /// <returns>Output data, of the given outputType</returns>
        public virtual void runAsync()
        {
            throw new Exception("[Action] This action does not support async mode");
        }
    }

    /// <summary>
    /// Abstract class for In Actions.<br/>
    /// Inherit from this class to create an Action that do something from a given input.<br/>
    /// Can be used to create RuleEngine Actions (end points).<br/>
    /// </summary>
    /// <typeparam name="Input">Specifies what kind of data enters your actions.</typeparam>
    public abstract class ActionIn<Input> : ActionBase {
        /// <summary>
        /// Input type. Specifies what kind of data enters your actions.
        /// </summary>
        public override Type inputType => typeof(Input);
        /// <summary>
        /// Output type. This is always null in the case of a In-only Action.
        /// </summary>
        public override Type outputType => null;
        public virtual bool preProcess(Input input) { return true; }
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code.
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        public abstract void run(Input input);
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code. (should be run async)
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        /// <returns>Output data, of the given outputType</returns>
        public virtual void runAsync()
        {
            throw new Exception("[Action] This action does not support async mode");
        }
    }

    /// <summary>
    /// Abstract class for In Actions.<br/>
    /// Inherit from this class to create an Action that do something from a given input.<br/>
    /// Can be used to create RuleEngine Actions (end points).<br/>
    /// </summary>
    /// <typeparam name="Input">Specifies what kind of data enters your actions.</typeparam>
    public abstract class ActionVoid : ActionBase
    {
        /// <summary>
        /// Input type. Specifies what kind of data enters your actions.
        /// </summary>
        public override Type inputType => null;
        /// <summary>
        /// Output type. This is always null in the case of a In-only Action.
        /// </summary>
        public override Type outputType => null;
        public virtual bool preProcess() { return true; }
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code.
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        public abstract void run();
        /// <summary>
        /// Abstract execution method. When creating a new action, this method implementation will hold all the processing code. (should be run async)
        /// </summary>
        /// <param name="input">Input data, of the given inputType</param>
        /// <returns>Output data, of the given outputType</returns>
        public virtual void runAsync()
        {
            throw new Exception("[Action] This action does not support async mode");
        }
    }

    /// <summary>
    /// Attribute for declaring an Action's field as an UserParameter.
    /// This will result in a UI control if the Action is used in the RuleEngine or the Toolbox.
    /// </summary>
    public class UserParameter : Attribute {

        public readonly string visibilityCheck;
        public readonly string enablingCheck;
        public readonly string tooltip;
        public readonly string displayName;

        public UserParameter(string visibilityCheck = null, string enablingCheck = null, string tooltip = null, string displayName = null) {
            this.visibilityCheck = visibilityCheck;
            this.enablingCheck = enablingCheck;
            this.tooltip = tooltip;
            this.displayName = displayName;
        }
    }

    /// <summary>
    /// Attribute for declaring an Action's method as an Helper Method.
    /// This method can then be ran from the [...] in the RuleEngine GUI.
    /// <remarks>The method should take no parameters.</remarks>
    /// </summary>
    public class HelperMethod : Attribute {

    }
    public class FilterParameter : Attribute
    {
        public readonly string[] filter;
        public FilterParameter(string[] filter)
        {
            this.filter = filter;
        }
    }
}
