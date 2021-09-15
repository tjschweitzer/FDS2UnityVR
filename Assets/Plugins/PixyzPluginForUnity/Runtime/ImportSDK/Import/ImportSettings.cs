using UnityEngine;
using Pixyz.Commons.Utilities;
using Pixyz.LODTools;
using System;

namespace Pixyz.ImportSDK {

    /// <summary>
    /// Serializable container class for import settings. This class inherits from Unity's ScriptableObject and can be serialized.
    /// This allows users to save presets and to use them either from the Import Window or from scripting by declaring a public field in a MonoBehaviour class.
    /// This class can also be created at runtime.
    /// </summary>
    public sealed class ImportSettings : ScriptableObject {

        private bool _locked;
        public bool locked {
            get { return _locked; }
            set { _locked = value; qualities.isLocked = value; }
        }

        public event VoidHandler changed;

        public void invokeChanged() {
            changed?.Invoke();
        }

        #region Imports

        /// <summary>
        /// Imports metadata from the imported model. Only available fi model has metadata and no processing is applied on the hierarchy.
        /// </summary>
        public bool loadMetadata = true;

        /// <summary>
        /// Import lines such as laser marking. Only works if model has lines.
        /// </summary>
        public bool importLines = false;

        /// <summary>
        /// Imports patch boundaries as lines. Only works if model is a CAD model (has BReps).
        /// </summary>
        public bool importPatchBorders = false;

        /// <summary>
        /// Import points, also called 'free vertices'. Points can be CAD position information or point cloud data for instance.
        /// </summary>
        public bool importPoints = false;

        #endregion

        #region Transforms

        /// <summary>
        /// Scale factor applied to the whole assembly.
        /// </summary>
        public float scaleFactor = 0.001f;

        /// <summary>
        /// Mirror CAD right or left.
        /// </summary>
        public bool isLeftHanded = false;

        /// <summary>
        /// Use this setting to rotate model from Z-up axis to Y-up axis.
        /// </summary>
        public bool isZUp = false;

        #endregion

        #region Hierarchy

        /// <summary>
        /// Merge final assemblies: Use this setting to assemble together unconnected CAD surfaces prior to any data treatment at import (it is a pre-process)
        /// </summary>
        public bool mergeFinalLevel = false;

        /// <summary>
        /// Use these functions to simplify the tree (or hierarchy) of your model.
        /// </summary>
        public TreeProcessType treeProcess = TreeProcessType.FULL;

        #endregion

        #region Quality & LODS

        public bool repair = true;

        public bool combinePatchesByMaterial = true;

        /// <summary>
        /// The LODs (Level Of Detail) is an optimization feature that allows Unity to automatically switch a model quality depending on its occupancy relatively to the screen (in %).
        /// </summary>
        public bool hasLODs = false;

        /// <summary>
        /// The LODs level tells if the LOD should be managed at assembly (Root) or at model (Leaves) level.
        /// </summary>
        public LodGroupPlacement lodsMode = LodGroupPlacement.LEAVES;

        /// <summary>
        /// The LODs settings describes LODs to be generated with their associated threshold and quality.
        /// </summary>
        public LodsGenerationSettings qualities = LodsGenerationSettings.Default();

        // Pt cloud
        public int voxelizeGridSize = 10;
        public int lodCount = 3;

        #endregion

        #region Misc

        /// <summary>
        /// Create a new projected primary UV set (channel #0).
        /// </summary>
        public bool mapUV = false;

        /// <summary>
        /// The size of the projection box used to create UVs.
        /// </summary>
        public float mapUV3dSize = 1f;

        /// <summary>
        /// Create Lightmap UVs.
        /// </summary>
        public bool createLightmapUV = false;

        /// <summary>
        /// Resolution (in pixels).
        /// </summary>
        public int lightmapResolution = 1024;

        /// <summary>
        /// Set the padding (in pixels) between UV islands.
        /// </summary>
        public int uvPadding = 4;

        /// <summary>
        /// Orient normals of adjacent faces consistently.
        /// </summary>
        public bool orient = false;

        /// <summary>
        /// Use this setting if you wish to create meshes limited to 65k vertices. Meshes will be created with a 16 bit index buffer (32 bit by default).<br/>
        /// Consider using this setting if you wish to publish the model on platform with limited power.
        /// </summary>
        public bool splitTo16BytesIndex = false;

        /// <summary>
        /// If specified, the import will use the given shader to create materials contained in the imported model. If not specified, it will use the default shader for the current Render Pipeline.
        /// </summary>
        public Shader shader;

        /// <summary>
        /// If true, Pixyz will attempt to load materials whose name match the name of the imported one.<br/>
        /// Materials to load needs to be placed at the root of a Resources folder, somewhere in the project Assets.
        /// </summary>
        public bool useMaterialsInResources = false;

        /// <summary>
        /// If true, all GameObjects that are considered as symmetries will have their transform baked in their mesh vertices.<br/>
        /// This will break instances for those GameObject's meshes, but avoid negative scales in transform that could cause issues such as lightmapping issues.
        /// </summary>
        public bool singularizeSymmetries = false;

        /// <summary>
        /// If true, Pixyz will attempt to detect similar GameObjects and recreate instances of their meshes.<br/>
        /// This can be used to repair instances or to simplify a model that has similar parts that could be instanciated instead to reduce the number of unique meshes (reduces drawcalls, GPU memory usage and file size).<br/>
        /// </summary>
        public bool repairInstances = false;

        #endregion

        public ImportSettings Verify(ImportSettingsTemplate template)
        {
            ImportSettings nsettings = ScriptableObject.CreateInstance<ImportSettings>();
            nsettings.importLines = (template.importLines.status == ParameterAvailability.Available) ? importLines : template.importLines.defaultValue;
            nsettings.importPatchBorders = (template.importPatchBorders.status == ParameterAvailability.Available) ? importPatchBorders : template.importPatchBorders.defaultValue;
            nsettings.importPoints = (template.importPoints.status == ParameterAvailability.Available) ? importPoints : template.importPoints.defaultValue;
            nsettings.mapUV3dSize = (template.mapUV.status == ParameterAvailability.Available) ? (mapUV ? mapUV3dSize * 1000 : -1) : (template.mapUV.defaultValue ? template.mapUV3dSize.defaultValue : -1);
            nsettings.uvPadding = (template.uvPadding.status == ParameterAvailability.Available) ? uvPadding : template.uvPadding.defaultValue;
            nsettings.lightmapResolution = (template.createLightmapUV.status == ParameterAvailability.Available) ? (createLightmapUV ? lightmapResolution : -1) : (template.createLightmapUV.defaultValue ? template.lightmapResolution.defaultValue : -1);
            nsettings.mergeFinalLevel = (template.mergeFinalLevel.status == ParameterAvailability.Available) ? mergeFinalLevel : template.mergeFinalLevel.defaultValue;
            nsettings.treeProcess = (template.treeProcess.status == ParameterAvailability.Available) ? treeProcess : template.treeProcess.defaultValue; ;
            nsettings.voxelizeGridSize = (template.voxelizeGridSize.status == ParameterAvailability.Available) ? voxelizeGridSize : template.voxelizeGridSize.defaultValue;
            nsettings.orient = (template.orient.status == ParameterAvailability.Available) ? orient : template.orient.defaultValue;
            nsettings.splitTo16BytesIndex = ((template.splitTo16BytesIndex.status == ParameterAvailability.Available) ? splitTo16BytesIndex : template.splitTo16BytesIndex.defaultValue);
            //var lodSettings = ((template.qualities.status == ParameterAvailability.Available) ? settings.qualities : template.qualities.defaultValue).Verify(settings, template);
            //nsettings.qualities.quality. = lodSettings.Item1;
            //nsettings.qualities = lodSettings.Item2;
            nsettings.qualities = (template.qualities.status == ParameterAvailability.Available) ? VerifyLODSettings(qualities, this, template) : template.qualities.defaultValue;
            nsettings.hasLODs = (template.hasLODs.status == ParameterAvailability.Available) ? hasLODs : template.hasLODs.defaultValue;
            nsettings.lodCount = lodCount;
            nsettings.lodsMode = (template.lodsMode.status == ParameterAvailability.Available) ? lodsMode : template.lodsMode.defaultValue;
            nsettings.singularizeSymmetries = (template.singularizeSymmetries.status == ParameterAvailability.Available) ? singularizeSymmetries : template.singularizeSymmetries.defaultValue;
            nsettings.repairInstances = (template.repairInstances.status == ParameterAvailability.Available) ? repairInstances : template.repairInstances.defaultValue;
            nsettings.repair = (template.repair.status == ParameterAvailability.Available) ? repair : template.repair.defaultValue;
            nsettings.combinePatchesByMaterial = (template.combinePatchesByMaterial.status == ParameterAvailability.Available) ? combinePatchesByMaterial : template.combinePatchesByMaterial.defaultValue;
            nsettings.isZUp = (template.isZUp.status == ParameterAvailability.Available) ? isZUp : template.isZUp.defaultValue;
            nsettings.isLeftHanded = (template.isLeftHanded.status == ParameterAvailability.Available) ? isLeftHanded : template.isLeftHanded.defaultValue;
            nsettings.scaleFactor = (template.scaleFactor.status == ParameterAvailability.Available) ? scaleFactor : template.scaleFactor.defaultValue;
            nsettings.shader = (template.shader.status == ParameterAvailability.Available) ? shader : template.shader.defaultValue;
            return nsettings;

        }
        public static LodsGenerationSettings VerifyLODSettings(LodsGenerationSettings unityObject, ImportSettings settings, ImportSettingsTemplate template)
        {
            LodsGenerationSettings lods = settings.qualities;
            if (settings.hasLODs)
            {
                if (template.name == "Point Cloud")
                {
                    lods.lods = new LodGenerationSettings[settings.lodCount+1];
                    LodGenerationSettings[] tmp = LodsGenerationSettings.Default().lods;
                    Array.Copy(tmp, lods.lods, Mathf.Min(settings.lodCount+1, tmp.Length));
                }
                else
                {
                    return settings.qualities;
                }
            }
            else
            {
                lods.lods[0].quality = unityObject.lods[0].quality;
                lods.lods[0].threshold = (6.0 - (int)unityObject.lods[0].quality) / 6.0;
            }

            return lods;
        }
    }
}