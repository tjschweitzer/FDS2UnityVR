using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixyz.Toolbox
{
    public static class ToolboxTooltips
    {
        #region Repair Mesh

        public const string repairAction = "Repair all given meshes that present flaws: Holes, wrong junctions, faces' orientation, etc..";
        public const string repairDistanceTolerance = "Merge any vertices whose proximity is under the given tolerance in your Unity scale system (by default in meters).";
        public const string repairOrientFaces = "Orient Normals of adjacent faces consistently (unification of all triangles orientation).";

        #endregion

        #region Decimate to Quality

        public const string decimQualityAction = "Reduce the number of triangles on the meshes driven by quality.";
        public const string decimQualityStrategy = "Vertex Removal: Standard decimation process appropriate in most use cases. Adapted for hard edges coming from CAD data.\n\n Edge Collapse (Preview): Longer decimation process tailored to better preserve UVs. Recommended for game assets. This function is in preview, please share feedback for effective improvement.";
        public const string decimQualityPreset = "Surfacic tolerance values recommanded by Pixyz.";
        public const string decimQualityNormalDeviation = "Constraint the maximum normals deviation (angle threshold)";
        public const string decimQualityNormalTolerance = "Maximum angle between original normals and those interpolated on the simplified surface";
        public const string decimQualitySurfacic = "Surfacic tolerance values recommanded by Pixyz.";
        public const string decimQualityUV = "Maximum distance (in UV space) between original textcoords and those interpolated on the simplified surface.";
        public const string decimQualityUVSeam = "Constraint the maximum UV seams deviation (displacement).";
        public const string decimQualityUVFoldovers = "Forbid UVs to folder over and overlap each other.";
        public const string decimQualityLineic = "Maximum distance between lineic vertices and resulting simplified lines";

        #endregion

        #region Decimate to Target

        public const string decimTargetAction = "Reduce the number of triangles on the meshes, up to a given triangle count ratio.";
        public const string decimTargetStrategy = "Triangle count: Driven by a specific targeted number of triangles.\n\nRatio: Driven by pourcentage of mesh total number of triangles.";
        public const string decimTargetRatio = "Choose 0% to 100%";
        public const string decimTargetCount = "Targeted number of triangles.";
        public const string decimTargetWeightScale = "Give more importance to colored vertices preservation";
        public const string decimTargetVertex = "Use vertex weights if any. Vertex weights are computed from red vertices colors.";
        public const string decimTargetBoundary = "Preserve the edges defining the mesh boundaries (free edges) from being distorted.";
        public const string decimTargetNormal = "Preserve the edges defining the mesh boundaries (free edges) from being distorted.";
        public const string decimTargetUV = "Preserve UVs from being distorted.";
        public const string decimTargetSharpNormal = "Preserve sharp edges (or hard edges) from being distorted.";
        public const string decimTargetUVSeam = "Preserve UV seams (UV islands contours) from being distorted.";
        public const string decimTargetUVFoldovers = "Forbid UVs to fold over and overlap each other.";

        #endregion

        #region RemoveHidden
        
        public const string removeHiddenAction = "Delete occluded triangles.";
        public const string removeHiddenStrategy = "Standard: Remove occluded triangles within the meshes.\n\nAdvanced: Keep triangles that are part of an internal area that must be preserved (for example, a car interior, the inside of a house, ...).";
        public const string removeHiddenLevel = "The type of element to be removed.";
        public const string removeHiddenPrecisionPreset = "Precision values recommanded by Pixyz.";
        public const string removeHiddenPrecision = "The higher the precision is, the longer the function will take, but also higher are the chances it does not miss a visible triangle.";
        public const string removeHiddenTransparent = "If true, transparent materials will be considered as opaque, meaning that geometry behind transparent materials will be removed.";
        public const string removeHiddenNeighboors = "Preserve the N neighbors of not occluded triangles. It's recommanded to put 0 on low-detail LODs. Put >0 for higher detailed LODs.";
        public const string removeHiddenCavities = "Preserve input internal cavities. Car cockpits, inside of a house...";
        public const string removeHiddenCavitiesMinimum = "Cavity volume in cubic meters.\n\nFor instance, to preserve what's inside your living room in a house, put a bit less than its full volume. The algorithm will then remove hidden triangle/Gamobject/sub-mesh inside your living's room furnitures but not the furnitures themselves.";

        #endregion

        #region Retopologize
        
        public const string retopologizeAction = "Retopologize meshes.";
        public const string retopologizeType = "Standard: Create a rough mesh approximation driven by quality target.\n\nField-Aligned (Preview): Create a field-aligned quad-dominant mesh with control on triangle count. This function is in preview, please share with us your feedback.";
        public const string retopologizeQuality = "Quality values recommanded by Pixyz.";
        public const string retopologizeQualityValue = "The higher the number is, the longer the execution will be & potentially with a higher number of triangles.\n\nKeep it low for rough approximation.";
        public const string retopologizeStrategy = "Triangle count: Driven by a specific targeted number of triangles.\n\nTriangle count: Driven by a specific targeted number of triangles.";
        public const string retopologizeRatio = "Approximate output ratio triangle count. Choose 0% to 100%.";
        public const string retopologizeTriangles = "Approximate output triangle count.";
        public const string retopologizeUseFeature = "If activated, Pixyz will try to preserve mesh details larger than chosen feature size.";
        public const string retopologizeFeatureSize = "Minimum feature size to preserve.";
        public const string retopologizeBake = "Transfer original mesh information into texture maps (Albedo, Normal, Opacity, Roughness, Specular, Metallic, Ambiant Occlusion).";
        public const string retopologizeMapResolution = "Generated textures size.";
        public const string retopologizePtCloud = "Generate Collider components.";


        #endregion

        #region CreateColliders

        public const string createColliderAction = "Generate Collider components.";
        public const string createColliderStrategy = "Retopology: Create a mesh approximation (low-poly) for an assigned Collider.\n\nConvex Decomposition: Create set of convex mesh Colliders from original geometry. Ideal for advanced Physics collision. Colliders are assigned on existing GameObjects.\n\nAxis-aligned Bounding-box: Create set of Box Colliders for each Mesh. Colliders are assigned on existing GameObjects.\n\nOriginal Mesh: Use existing mesh as Collider.";
        public const string createColliderMaxDecompo = "Maximum number of decompositon to generate, per mesh.\n\nOn a fork for instance, you would put 5: 4 for the spikes + 1 for the base.";
        public const string createColliderTriangles = "Maximum number of triangles per output convex collider mesh";
        public const string createColliderResolution = "Concavity resolution (the higher the more concave).";

        #endregion

        #region RemoveColliders

        public const string removeColliderAction = "Generates UVs for Unity lightmapping on the channel 1.";

        #endregion

        #region CreateBillboard

        public const string billboardAction = "Create a new mesh composed of multiple planes from given meshes.";
        public const string billboardResolution = "Generated textures size.";

        #endregion

        #region Merge

        public const string mergeAction = "Merges all input GameObjects into a single object. By merging, you will lose additional components such as Metadata.";
        public const string mergeKeepParent = "Will merge each GameObjects with all of its own children.";

        #endregion

        #region Combine

        public const string combineAction = "Combine all given meshes to one mesh with one baked material.";
        public const string combineMapResolution = "Generated textures size.";
        public const string combineUVGen = "Override existing UVs.";

        #endregion

        #region Replace By

        public const string replaceByAction = "Replace each GameObject by another object.";
        public const string replaceByRotation = "If true, use the rotation of the newly placed object, otherwise, use the rotation of the original object";
        public const string replaceByScale = "If true, use the scale of the newly placed object, otherwise, use the scale of the original object";

        #endregion

        #region Explode

        public const string explodeAction = "Explodes each GameObject in multiple GameObjects depending on its submesh count (if it has a Mesh).";

        #endregion

        #region MovePivot

        public const string movePivotAction = "Moves the origin (pivot) to another point. Make sure your \"Handle position\" is set to Center to check the new position.";

        #endregion

        #region CreateNormals

        public const string createNormalsAction = "Creates normals for the given smoothing angle";

        #endregion

        #region FlipNormals

        public const string flipNormalsAction = "Flips Meshes Normals. As this function changes data at Mesh level, any modification to a Mesh will be visible for each GameObject using that Mesh, regardless of the input.";

        #endregion

        #region CreateUVs

        public const string createUVAction = "Create projected UVs. As this function changes data at Mesh level, any modification to a Mesh will be visible for each GameObject using that Mesh, regardless of the input.";
        public const string uvSize = "In your Unity scale system (by default in meters).";

        #endregion


        #region CreateUVLightMaps

        public const string createUVLightMapsAction = "Generates UVs for Unity lightmapping on the channel 1.";

        #endregion
    }
}