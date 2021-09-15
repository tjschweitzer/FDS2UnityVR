using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public abstract class PixyzFunction : ActionInOut<IList<GameObject>, IList<GameObject>>
    {
        public override bool isAsync => _isAsync;

        protected bool _isAsync = true;
        protected IList<GameObject> _output = null;
        protected IList<GameObject> _input = null;
        protected IList<GameObject> _inputParts = null;
        protected PixyzContext _context = null;

        private MeshData _data = null;

        protected virtual MaterialSyncType SyncMaterials => MaterialSyncType.SyncFull;
        protected PixyzContext Context => _context;
        public override object Input => _input;
        public override object Output => _output;
        public IList<GameObject> InputParts => _inputParts;

        public override bool preProcess(IList<GameObject> input)
        {
            executionErrors = new List<string>();
            try
            {
                _input = input;
                _output = input;
                _inputParts = input.GetParts();
                _data = _inputParts.GetMeshData();
                _context = new PixyzContext(SyncMaterials);
                _context.UnityMeshesToPixyzCreate(_data.renderers, _data.meshes, _data.materials);
            } catch (Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
                executionErrors.Add(e.Message);
                completed.Invoke();
                Dispose();
                return false;
            }
            return true;
        }

        public override void runAsync()
        {
            try
            {
                //Tell pixyz that the main thread has changed
                OptimizeSDK.Native.NativeInterface.SetPixyzMainThread();
                _isRunning = true;
                process();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
                executionErrors.Add(e.Message);
                completed.Invoke();
                Dispose();
            }
        }

        public override IList<GameObject> run(IList<GameObject> input)
        {
            Debug.Log(this.ToString());
            preProcess(input);
            process();
            postProcess();

            return _output;
        }

        public override void Dispose()
        {
            _isRunning = false;
            
            if (_context == null)
                return;
            
            _context.Dispose();
            _context = null;
        }

        protected virtual void process()
        {
            
        }

        protected override void postProcess()
        {
            foreach (GameObject go in _output)
            {
                if (go != null)
                    Undo.RegisterCreatedObjectUndo(go, displayNameToolbox);
            }
        }

        protected IList<GameObject> CleanList(IList<GameObject> list)
        {
            List<GameObject> newList = new List<GameObject>();
            foreach (GameObject go in list)
            {
                if (go != null)
                {
                    newList.Add(go);
                }
            }
            return newList;
        }

        protected IList<GameObject> AddElements(IList<GameObject> list, List<GameObject> elements)
        {
            List<GameObject> newList = new List<GameObject>(list);
            foreach (GameObject go in elements)
            {
                newList.Add(go);
            }
            return newList;
        }

        protected void ReplaceInHierarchy(IList<GameObject> replacedParts, IList<GameObject> replacingParts)
        {
            if (replacingParts.Count != replacedParts.Count) return;
            for (int i = 0; i < replacingParts.Count; i++)
            {
                Renderer replacingRenderer = replacingParts[i].GetComponent<Renderer>();
                Renderer replacedRenderer = replacedParts[i].GetComponent<Renderer>();
                Mesh mesh = null;
                if (replacingRenderer == null || replacedRenderer == null) continue;
                if (replacingRenderer is MeshRenderer)
                {
                    mesh = replacingRenderer.GetComponent<MeshFilter>().sharedMesh;
                    replacedRenderer.GetComponent<MeshFilter>().sharedMesh = mesh;
                    replacedRenderer.sharedMaterials = replacingRenderer.sharedMaterials;

                    if (mesh.vertexCount == 0)
                    {
                        var meshFilter = replacedRenderer.GetComponent<MeshFilter>();
                        MeshRenderer.DestroyImmediate(replacedRenderer);
                        MeshFilter.DestroyImmediate(meshFilter);
                    }
                    else
                    {
                        replacedRenderer.GetComponent<MeshFilter>().sharedMesh = mesh;
                    }
                }
                else if (replacingRenderer is SkinnedMeshRenderer)
                {
                    mesh = replacingRenderer.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    if (mesh.vertexCount == 0)
                    {
                        SkinnedMeshRenderer.DestroyImmediate(replacedRenderer);
                    } else
                    {
                        replacedRenderer.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
                        replacedRenderer.sharedMaterials = replacingRenderer.sharedMaterials;
                    }
                }
            }
            foreach (GameObject go in replacingParts)
            {
                GameObject.DestroyImmediate(go);
            }
        }

        protected void DeleteAllInput()
        {
            foreach (GameObject go in _input)
            {
                GameObject.DestroyImmediate(go);
            }
            _input = CleanList(_input);
        }

        protected void DisableAllInput()
        {
            foreach (GameObject go in _input)
            {
                go.SetActive(false);
            }
            _input = CleanList(_input);
        }

        protected void DisableInput()
        {
            if(_input.Count > 0)
            {
                _input[_input.Count-1].SetActive(false);
            }
            _input = CleanList(_input);
        }
        
    }
}