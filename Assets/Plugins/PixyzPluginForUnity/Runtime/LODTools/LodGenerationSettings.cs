using UnityEngine;
using System;

namespace Pixyz.LODTools
{
    /// <summary>
    /// Serializable container class for all LODs
    /// </summary>
    [Serializable]
    public struct LodsGenerationSettings
    {

        public static LodsGenerationSettings Default()
        {
            return new LodsGenerationSettings(new LodGenerationSettings[] {
                new LodGenerationSettings { threshold = 0.50, quality = LodQuality.MAXIMUM },
                new LodGenerationSettings { threshold = 0.20, quality = LodQuality.MEDIUM },
                new LodGenerationSettings { threshold = 0.05, quality = LodQuality.LOW },
                new LodGenerationSettings { threshold = 0.0, quality = LodQuality.POOR } });
        }

        public LodsGenerationSettings(LodGenerationSettings[] lods)
        {
            _locked = false;
            _lods = lods;
        }

        [SerializeField]
        private LodGenerationSettings[] _lods;

        /// <summary>
        /// Get or Set settings for each LOD.
        /// Check @link Pixyz.LODSettings @endlink for information on how to set up a LOD.
        /// </summary>
        public LodGenerationSettings[] lods
        {
            get
            {
                if (_lods == null || _lods.Length == 0)
                {
                    _lods = new LodGenerationSettings[] { new LodGenerationSettings { threshold = 1, quality = LodQuality.MAXIMUM } };
                }
                return _lods;
            }
            set
            {
                if (_lods == value || value == null)
                    return;
                _lods = value;
            }
        }

        public LodGenerationSettings quality
        {
            get
            {
                return lods[0];
            }
            set
            {
                lods[0] = value;
            }
        }

        public bool isLocked
        {
            get { return _locked; }
            set { _locked = value; }
        }

        [SerializeField]
        private bool _locked;
    }

    /// <summary>
    /// Serializable container class for a single LOD.
    /// </summary>
    [Serializable]
    public struct LodGenerationSettings
    {

        /// <summary>
        /// The quality for that LOD.
        /// </summary>
        public LodQuality quality;

        /// <summary>
        /// The threshold [0 to 1] at which this LOD ends.
        /// For example : 
        /// A threshold of 0 means that this LOD will be visible between (previousLOD.threshold * 100)% and 0% visibility.
        /// A threshold of 0.3 means that this LOD will be visible between (previousLOD.threshold * 100)% and 30% visibility.
        /// </summary>
        public double threshold;
    }
}
