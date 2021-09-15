using System;
using System.Threading.Tasks;
using UnityEngine.Events;
using Pixyz.OptimizeSDK.Runtime;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;
using Pixyz.OptimizeSDK;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Polygonal;
using Pixyz.OptimizeSDK.Native.Geom;
using Pixyz.OptimizeSDK.Native.Algo;

namespace Pixyz.LODTools.Editor
{
    public class LODBuilder
    {
        public Action<bool> generationCompleted;
        private PixyzContext[] _contexts = null;
        private LODProcess _process = null;
        private int _currentLODIndex;
        public int currentIndex { get { return _currentLODIndex; } set { _currentLODIndex = value; indexChanged.Invoke(); } }
        public UnityEvent indexChanged = new UnityEvent();

        public PixyzContext[] Contexts => _contexts;

        public async Task<PixyzContext[]> BuildLOD(PixyzContext sourceContext, LODProcess process, bool async = true)
        {
            if (_contexts != null && _contexts.Length > 0)
            {
                _contexts[0].Dispose();
            }
            indexChanged = new UnityEvent();
            _process = process;
            currentIndex = 1;
            _contexts = new PixyzContext[process.Rules.Count + 1];
            _contexts[0] = sourceContext;

            bool finishWithoutErrors = true;
            try
            {
                while(_currentLODIndex < _contexts.Length)
                {
                    if(async)
                    {
                        if (!await Task.Factory.StartNew(StartProcess))
                            break;
                    }
                    else
                    {
                        if (!StartProcess())
                            break;
                    }
                    ++currentIndex;
                }
            }
            catch(System.Exception e)
            {
                UnityEngine.Debug.LogError($"[LODGenerationError] {e.Message}\n{e.StackTrace}");
                finishWithoutErrors = false;
            }
            generationCompleted?.Invoke(finishWithoutErrors);

            return _contexts;
        }

        public bool StartProcess()
        {
            try
            {
                NativeInterface.SetPixyzMainThread();

                LODRule rule = _process.Rules[_currentLODIndex - 1];

                PixyzContext srcContext = _contexts[_process.Sources[_currentLODIndex - 1]];
                PixyzContext context = srcContext.Clone() as PixyzContext;
                srcContext.LinkContext(context);
                _contexts[_currentLODIndex] = context;

                if (rule.isRepairEnable)
                    NativeInterface.RepairMeshes(context.pixyzMeshes, rule.repairParameters.tolerance, true, false, context.pixyzMatrices);
                else
                    NativeInterface.WeldVertices(context.pixyzMeshes, 0.0000001f, context.pixyzMatrices);

                if(rule.isImposterActivated)
                {
                    uint outputMesh = NativeInterface.CreateBillboard(context.pixyzMeshes, context.pixyzMatrices, rule.imposterParameters.resolution, rule.imposterParameters.XPositiveEnable,
                        rule.imposterParameters.XNegativeEnable,rule.imposterParameters.YPositiveEnable, rule.imposterParameters.YNegativeEnable, rule.imposterParameters.ZPositiveEnable,
                        rule.imposterParameters.ZNegativeEnable, true);

                    context.pixyzMeshes = new MeshList(new uint[] { outputMesh });
                    context.pixyzMatrices = new Matrix4List(new Matrix4[] { Conversions.Identity() });

                    return true;
                }

                if (rule.isRemeshFieldAlignedActivated || rule.isRemeshActivated)
                {
                    uint newMesh = 0;
                    int mapResolution = 1024;
                    bool bakeMaps = true;

                    if (rule.isRemeshFieldAlignedActivated)
                    {
                        if(rule.remeshFieldAlignedParameters.isTargetCount)
                        {
                            newMesh = NativeInterface.RemeshFieldAligned(context.pixyzMeshes, rule.remeshFieldAlignedParameters.targetTriangleCount, context.pixyzMatrices, rule.remeshFieldAlignedParameters.fullQuad,
                                rule.remeshFieldAlignedParameters.featureSize, rule.remeshFieldAlignedParameters.transferAnimations);
                        }
                        else
                        {
                            newMesh = NativeInterface.RemeshFieldAlignedToRatio(context.pixyzMeshes, rule.remeshFieldAlignedParameters.targetRatio, context.pixyzMatrices, rule.remeshFieldAlignedParameters.fullQuad,
                                rule.remeshFieldAlignedParameters.featureSize, rule.remeshFieldAlignedParameters.transferAnimations);
                        }

                        mapResolution = rule.remeshFieldAlignedParameters.mapsResolution;
                        bakeMaps = rule.remeshFieldAlignedParameters.bakeMaps;
                    }
                    else if (rule.isRemeshActivated)
                    {
                        newMesh = NativeInterface.Remesh(context.pixyzMeshes, rule.remeshParameters.featureSize, false, context.pixyzMatrices, rule.remeshParameters.transferAnimations);

                        mapResolution = rule.remeshParameters.mapsResolution;
                        bakeMaps = rule.remeshParameters.bakeMaps;
                    }

                    if(bakeMaps)
                    {
                        NativeInterface.BakeMaterials(context.pixyzMeshes, newMesh, mapResolution, 1, true, BakingMethod.RayOnly, new MapTypeList(new MapType[] { MapType.Diffuse, MapType.Normal, MapType.Metallic, MapType.Roughness, MapType.AO }), context.pixyzMatrices, Conversions.Identity());
                    }

                    context.pixyzMeshes = new MeshList(new uint[] { newMesh });
                    context.pixyzMatrices = new Matrix4List(new Matrix4[] { Conversions.Identity()});
                }
                else if (rule.isOcclusionActivated)
                {
                    if (rule.occlusionParameters.mode == OcclusionMode.Standard)
                    {
                        NativeInterface.RemoveHidden(context.pixyzMeshes, context.pixyzMatrices, SelectionLevel.Polygons, rule.occlusionParameters.considerTransparentOpaque, rule.occlusionParameters.adjacencyDepth, rule.occlusionParameters.cameraResolution);
                    }
                    else
                    {
                        // convert minimumCavityVolume to m3
                        var minimumCavityVolumeMeters = rule.occlusionParameters.minimumCavityVolume * UnityEngine.Mathf.Pow(10, -9);

                        NativeInterface.RemoveHiddenAdvanced(context.pixyzMeshes, context.pixyzMatrices, SelectionLevel.Polygons, rule.occlusionParameters.voxelSize, minimumCavityVolumeMeters,
                            rule.occlusionParameters.considerTransparentOpaque, rule.occlusionParameters.adjacencyDepth, rule.occlusionParameters.cameraResolution);
                    }
                }

                if (rule.isDecimateToQualityActivated)
                {
                    NativeInterface.DecimateToQuality(context.pixyzMeshes, rule.decimateToQualityParam.errorMax, context.pixyzMatrices, rule.decimateToQualityParam.useVertexWeights, 1.0, rule.decimateToTarget.vertexWeightScale, rule.decimateToTarget.boundaryWeight, rule.decimateToTarget.normalWeight,
                            rule.decimateToQualityParam.uvWeight, rule.decimateToQualityParam.sharpNormalWeight, rule.decimateToQualityParam.uvSeamWeight,
                            rule.decimateToQualityParam.forbidUVFoldovers, rule.decimateToQualityParam.normalTolerance,
                            rule.decimateToQualityParam.uvTolerance, rule.decimateToQualityParam.uvSeamTolerance, rule.decimateToQualityParam.useVertexColorsAsWeight);
                }
                else if (rule.isDecimateToTargetActivated)
                {
                    if (rule.decimateToTarget.isTargetCount)
                    {
                        NativeInterface.DecimateToPolycount(context.pixyzMeshes, rule.decimateToTarget.polycount, context.pixyzMatrices,
                            rule.decimateToTarget.useVertexWeights, 1.0, rule.decimateToTarget.vertexWeightScale, rule.decimateToTarget.boundaryWeight, rule.decimateToTarget.normalWeight,
                            rule.decimateToTarget.uvWeight, rule.decimateToTarget.sharpNormalWeight, rule.decimateToTarget.uvSeamWeight,
                            rule.decimateToTarget.forbidUVFoldovers, rule.decimateToTarget.useVertexColorsAsWeight);
                    }
                    else
                    {
                        NativeInterface.DecimateToRatio(context.pixyzMeshes, rule.decimateToTarget.ratio, context.pixyzMatrices,
                            rule.decimateToTarget.useVertexWeights, 1.0f, rule.decimateToTarget.vertexWeightScale, rule.decimateToTarget.boundaryWeight, rule.decimateToTarget.normalWeight,
                            rule.decimateToTarget.uvWeight, rule.decimateToTarget.sharpNormalWeight, rule.decimateToTarget.uvSeamWeight,
                            rule.decimateToTarget.forbidUVFoldovers, rule.decimateToTarget.useVertexColorsAsWeight);
                    }
                }

                if (rule.isCombineMaterialsActivated) 
                {
                    uint mergedMesh = NativeInterface.CombineMeshes(context.pixyzMeshes, context.pixyzMatrices, rule.combineMeshesParameters.forceUVGeneration, rule.combineMeshesParameters.resolution, rule.combineMeshesParameters.padding, BakingMethod.RayOnly);

                    context.pixyzMeshes = new MeshList(new uint[] { mergedMesh });
                    context.pixyzMatrices = new Matrix4List(new Matrix4[] { Conversions.Identity() });
                }
                else if (rule.isCombineMeshesActivated)
                {
                    NativeInterface.CombineSubMeshesByMaterial(context.pixyzMeshes);
                }

                return true;
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"[LODGenerationError] {e.Message}\n{e.StackTrace}");
                return false;
            }
        }
    }
}