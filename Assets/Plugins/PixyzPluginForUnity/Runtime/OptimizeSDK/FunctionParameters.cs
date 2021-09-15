namespace Pixyz.OptimizeSDK.Runtime
{
    public enum DecimateQualityLevels
    {
        High = 0,
        Medium,
        Low,
        Custom
    }

    public enum Weight
    {
        Low = 1,
        Normal = 10,
        Important = 100,
        VeryImportant = 1000
    }

    public enum OcclusionMode
    {
        Standard = 0,
        Advanced
    }

    [System.Serializable]
    public class RepairMeshesParameters
    {
        public double tolerance;

        public RepairMeshesParameters(double tolerance = 0.0001)
        {
            this.tolerance = tolerance;
        }

        public long ComputeHash()
        {
            return 0x4E1D2BB603113A3 ^ tolerance.GetHashCode() ^ 3;
        }
    }

    [System.Serializable]
    public class CombineMeshesParameters
    {
        public bool forceUVGeneration;
        public int resolution;
        public int padding;

        public CombineMeshesParameters(bool forceUVGeneration = false,int resolution = 1024, int padding = 1)
        {
            this.forceUVGeneration = forceUVGeneration;
            this.resolution = resolution;
            this.padding = padding;
        }

        public long ComputeHash()
        {
            return 0x4E1D2BC1031F347 ^ resolution.GetHashCode() ^ padding.GetHashCode() ^ 43;
        }
    }

    [System.Serializable]
    public class RemeshParameters
    {
        public double featureSize;
        public bool transferAnimations;
        public bool bakeMaps;
        public int mapsResolution;

        public RemeshParameters(double featureSize = 0.1, bool transferAnimations = true, bool bakeMaps = true, int mapsResolution = 1024)
        {
            this.featureSize = featureSize;
            this.transferAnimations = transferAnimations;
            this.bakeMaps = bakeMaps;
            this.mapsResolution = mapsResolution;
        }

        public long ComputeHash()
        {
            return 0x1B0D2DF1031A1000 ^ featureSize.GetHashCode() ^ transferAnimations.GetHashCode() ^ 23 ^ bakeMaps.GetHashCode() ^ mapsResolution.GetHashCode();
        }
    }

    [System.Serializable]
    public class RemeshFieldAlignedParameters
    {
        public bool isTargetCount;

        public bool fullQuad;
        public bool transferAnimations;
        public bool bakeMaps;

        public double featureSize;
        public int mapsResolution;

        /// <summary>
        /// From 0.0f(0%) to 1.0f(100%)
        /// </summary>
        public float targetRatio;
        public int targetTriangleCount;

        public RemeshFieldAlignedParameters(int targetTriangleCount, double featureSize = -1, bool transferAnimations = true, bool fullQuad = true, bool bakeMaps = true, int mapsResolution = 1024)
        {
            isTargetCount = true;
            this.targetTriangleCount = targetTriangleCount;
            this.featureSize = featureSize;
            this.fullQuad = fullQuad;
            this.transferAnimations = transferAnimations;
            this.bakeMaps = bakeMaps;
            this.mapsResolution = mapsResolution;
        }

        public RemeshFieldAlignedParameters(float targetRatio = 0.25f, double featureSize = -1, bool transferAnimations = true, bool fullQuad = true, bool bakeMaps = true, int mapsResolution = 1024)
        {
            isTargetCount = false;
            this.targetRatio = targetRatio;
            this.featureSize = featureSize;

            this.fullQuad = fullQuad;
            this.transferAnimations = transferAnimations;
            this.bakeMaps = bakeMaps;
            this.mapsResolution = mapsResolution;
        }

        public long ComputeHash()
        {
            return 0x2E0D2AF5031C7020 ^ targetTriangleCount.GetHashCode() ^ featureSize.GetHashCode() ^ 7 ^ fullQuad.GetHashCode() ^ transferAnimations.GetHashCode() ^ bakeMaps.GetHashCode() ^ mapsResolution.GetHashCode();
        }
    }

    [System.Serializable]
    public class DecimateToQualityVertexRemovalParameters
    {
        public DecimateQualityLevels quality;
        public double surfacicTolerance;
        public double lineicTolerance;
        public double normalTolerance;
        public double uvTolerance;

        public DecimateToQualityVertexRemovalParameters(double surfacicTolerance = 0.001, double lineicTolerance = -1, double normalTolerance = 8, double texCoordTolerance = -1, DecimateQualityLevels quality = DecimateQualityLevels.Medium)
        {
            this.surfacicTolerance = surfacicTolerance;
            this.lineicTolerance = lineicTolerance;
            this.normalTolerance = normalTolerance;
            this.uvTolerance = texCoordTolerance;
            this.quality = quality;
        }

        public static DecimateToQualityVertexRemovalParameters High()
        {
            return new DecimateToQualityVertexRemovalParameters(0.0005, 0.0001, 1, -1, DecimateQualityLevels.High);
        }

        public static DecimateToQualityVertexRemovalParameters Medium()
        {
            return new DecimateToQualityVertexRemovalParameters(0.001, -1, 8, -1, DecimateQualityLevels.Medium);
        }

        public static DecimateToQualityVertexRemovalParameters Low()
        {
            return new DecimateToQualityVertexRemovalParameters(0.003, -1, 15, -1, DecimateQualityLevels.Low);
        }

        public long ComputeHash()
        {
            return 0x7E027AF1051D3023 ^ quality.GetHashCode() ^ surfacicTolerance.GetHashCode() ^ lineicTolerance.GetHashCode() ^ 19 ^ normalTolerance.GetHashCode() ^ uvTolerance.GetHashCode();
        }
    }

    [System.Serializable]
    public class DecimateToQualityParameters
    {
        public DecimateQualityLevels quality;

        public bool useVertexWeights;
        public bool useVertexColorsAsWeight = true;
        public bool forbidUVFoldovers;

        public double errorMax;

        public double vertexWeightScale;
        public double boundaryWeight;
        public double normalWeight;
        public double uvWeight;
        public double sharpNormalWeight;
        public double uvSeamWeight;
        public double normalTolerance;
        public double uvTolerance;
        public double uvSeamTolerance;

        public DecimateToQualityParameters(double errorMax = 0.001, bool useVertexWeights = true, double vertexWeightScale = (double)Weight.Normal, bool forceTarget = true, bool forbidUVFoldovers = true, double boundaryWeight = (double)Weight.Low, double normalWeight = (double)Weight.Low, double uvWeight = (double)Weight.Low, double sharpNormalWeight = (double)Weight.Low, double uvSeamWeight = (double)Weight.Normal, double normalTolerance = -1, double uvTolerance = -1, double uvSeamTolerance = -1, DecimateQualityLevels quality = DecimateQualityLevels.Medium)
        {
            this.errorMax = errorMax;
            this.useVertexWeights = useVertexWeights;
            this.forbidUVFoldovers = forbidUVFoldovers;
            this.boundaryWeight = boundaryWeight;
            this.normalWeight = normalWeight;
            this.uvWeight = uvWeight;
            this.sharpNormalWeight = sharpNormalWeight;
            this.uvSeamWeight = uvSeamWeight;
            this.normalTolerance = normalTolerance;
            this.uvTolerance = uvTolerance;
            this.uvSeamTolerance = uvSeamTolerance;
            this.vertexWeightScale = vertexWeightScale;
            this.quality = quality;
        }

        public static DecimateToQualityParameters High()
        {
            return new DecimateToQualityParameters(errorMax: 0.0005, normalTolerance: -1, uvTolerance: -1, quality: DecimateQualityLevels.High);
        }

        public static DecimateToQualityParameters Medium()
        {
            return new DecimateToQualityParameters(errorMax: 0.001, normalTolerance: -1, uvTolerance: -1, quality: DecimateQualityLevels.Medium);
        }

        public static DecimateToQualityParameters Low()
        {
            return new DecimateToQualityParameters(errorMax: 0.003, normalTolerance: -1, uvTolerance: -1, quality: DecimateQualityLevels.Low);
        }

        public long ComputeHash()
        {
            return 0x2B627AF1051A3C23 ^ errorMax.GetHashCode() ^ 5 ^ useVertexWeights.GetHashCode() ^ vertexWeightScale.GetHashCode() ^ useVertexColorsAsWeight.GetHashCode() ^ 51 ^ forbidUVFoldovers.GetHashCode() ^ boundaryWeight.GetHashCode() ^ normalWeight.GetHashCode() ^ 41 ^ sharpNormalWeight.GetHashCode() ^ uvSeamWeight.GetHashCode() ^ normalTolerance.GetHashCode() ^ uvTolerance.GetHashCode() ^ uvSeamTolerance.GetHashCode();
        }
    }

    [System.Serializable]
    public class DecimateToTargetParameters
    {
        public bool isTargetCount;
        public bool useVertexWeights;
        public bool useVertexColorsAsWeight = true;
        public bool forceTarget;
        public bool forbidUVFoldovers;

        /// <summary>
        /// From 0.0f(0%) to 1.0f(100%)
        /// </summary>
        public double ratio;
        public int polycount;

        public double vertexWeightScale;
        public double boundaryWeight;
        public double normalWeight;
        public double uvWeight;
        public double sharpNormalWeight;
        public double uvSeamWeight;
        //public double normalTolerance;
        //public double uvTolerance;
        //public double uvSeamTolerance;

        public DecimateToTargetParameters(double ratio = 0.5, bool useVertexWeights = true, double vertexWeightScale = (double)Weight.Normal, bool forceTarget = true, bool forbidUVFoldovers = true, double boundaryWeight = (double)Weight.Low, double normalWeight = (double)Weight.Low, double uvWeight = (double)Weight.Low, double sharpNormalWeight = (double)Weight.Low, double uvSeamWeight = (double)Weight.Normal)
        {
            isTargetCount = false;
            this.ratio = ratio;

            this.useVertexWeights = useVertexWeights;
            //this.forceTarget = forceTarget;
            this.forbidUVFoldovers = forbidUVFoldovers;
            this.boundaryWeight = boundaryWeight;
            this.normalWeight = normalWeight;
            this.uvWeight = uvWeight;
            this.sharpNormalWeight = sharpNormalWeight;
            this.uvSeamWeight = uvSeamWeight;
            //this.normalTolerance = normalTolerance;
            //this.uvTolerance = uvTolerance;
            //this.uvSeamTolerance = uvSeamTolerance;
            this.vertexWeightScale = vertexWeightScale;
        }

        public DecimateToTargetParameters(int polycount, bool useVertexWeights = true, double vertexWeightScale = (double)Weight.Normal, bool forceTarget = true, bool forbidUVFoldovers = true, double boundaryWeight = (double)Weight.Low, double normalWeight = (double)Weight.Low, double uvWeight = (double)Weight.Low, double sharpNormalWeight = (double)Weight.Low, double uvSeamWeight = (double)Weight.Normal)
        {
            isTargetCount = true;
            this.polycount = polycount;

            this.useVertexWeights = useVertexWeights;
            //this.forceTarget = forceTarget;
            this.forbidUVFoldovers = forbidUVFoldovers;
            this.boundaryWeight = boundaryWeight;
            this.normalWeight = normalWeight;
            this.uvWeight = uvWeight;
            this.sharpNormalWeight = sharpNormalWeight;
            this.uvSeamWeight = uvSeamWeight;
            //this.normalTolerance = normalMaxDeviation;
            //this.uvTolerance = uvMaxDeviation;
            //this.uvSeamTolerance = uvSeamTolerance;
            this.vertexWeightScale = vertexWeightScale;
        }

        public long ComputeHash()
        {
            return 0x2B354AB1051A3E ^ ratio.GetHashCode() ^ 5 ^ polycount.GetHashCode() ^ isTargetCount.GetHashCode() ^ useVertexWeights.GetHashCode() ^ vertexWeightScale.GetHashCode() ^ useVertexColorsAsWeight.GetHashCode() ^ 51 ^ forceTarget.GetHashCode() ^ forbidUVFoldovers.GetHashCode() ^ boundaryWeight.GetHashCode() ^ normalWeight.GetHashCode() ^ 41 ^ sharpNormalWeight.GetHashCode() ^ uvSeamWeight.GetHashCode();
        }
    }

    [System.Serializable]
    public class OcclusionCullingParameters
    {
        public bool considerTransparentOpaque;
        public double voxelSize;
        public double minimumCavityVolume;
        public int adjacencyDepth;
        public OcclusionMode mode = OcclusionMode.Standard;
        public int cameraResolution;

        public OcclusionCullingParameters(bool considerTransparentOpaque = true, double voxelSize = 0.1, double minimumCavityVolume = -1, OcclusionMode mode = OcclusionMode.Standard, int adjacencyDepth = 1, int cameraResolution = 1024)
        {
            this.considerTransparentOpaque = considerTransparentOpaque;
            this.voxelSize = voxelSize;
            this.minimumCavityVolume = minimumCavityVolume;
            this.mode = mode;
            this.adjacencyDepth = adjacencyDepth;
            this.cameraResolution = cameraResolution;
        }

        public long ComputeHash()
        {
            return 0x2A0D435F19EFC4CF ^ considerTransparentOpaque.GetHashCode() ^ 1223 ^ mode.GetHashCode() ^ voxelSize.GetHashCode() ^ minimumCavityVolume.GetHashCode() ^ adjacencyDepth.GetHashCode();
        }
    }

    [System.Serializable]
    public class ImposterParameters
    {
        public bool XPositiveEnable;
        public bool XNegativeEnable;
        public bool YPositiveEnable;
        public bool YNegativeEnable;
        public bool ZPositiveEnable;
        public bool ZNegativeEnable;

        public int resolution;

        public ImposterParameters(int resolution = 1024, bool XPositiveEnable = true, bool XNegativeEnable = true, bool YPositiveEnable = true, bool YNegativeEnable = true, bool ZPositiveEnable = true, bool ZNegativeEnable = true)
        {
            this.resolution = resolution;
            this.XPositiveEnable = XPositiveEnable;
            this.XNegativeEnable = XNegativeEnable;
            this.YPositiveEnable = YPositiveEnable;
            this.YNegativeEnable = YNegativeEnable;
            this.ZPositiveEnable = ZPositiveEnable;
            this.ZNegativeEnable = ZNegativeEnable;
        }

        public long ComputeHash()
        {
            return 0x336AB6FDD003EBC2 ^ resolution.GetHashCode() ^ 1543 ^ XPositiveEnable.GetHashCode() ^ XNegativeEnable.GetHashCode() ^ YPositiveEnable.GetHashCode() ^ 521 ^ YNegativeEnable.GetHashCode() ^ ZPositiveEnable.GetHashCode() ^ ZNegativeEnable.GetHashCode();
        }
    }
}
