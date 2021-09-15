using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixyz.LODTools;

namespace Pixyz.ImportSDK
{
	public class Tolerances
	{
		LodQuality quality;
		public Tolerances(LodQuality quality = LodQuality.MEDIUM)
		{
			this.quality = quality;

            {
                // Tesselation
                double _maxSag, _maxAngle;
                _maxSag = -1;
                _maxAngle = -1;
                switch (quality)
                {
                    case LodQuality.MAXIMUM:
                        // In Studio = Decimate "High"
                        _maxSag = 0.1;
                        break;
                    case LodQuality.HIGH:
                        // In Studio = Decimate "Medium"
                        _maxSag = 0.2;
                        break;
                    case LodQuality.MEDIUM:
                        // In Studio = Decimate "Low"
                        _maxSag = 1;
                        break;
                    case LodQuality.LOW:
                        _maxSag = 3;
                        break;
                    
                    default:
                        _maxSag = 10;
                        break;
                }
                maxSag =_maxSag;
                maxAngle = _maxAngle;
            }
            {
                // Decimation
                double _surfacicTolerance, _lineicTolerance, _normalTolerance, _uvTolerance;
                _surfacicTolerance = _lineicTolerance = _normalTolerance = _uvTolerance = -1;
                switch (quality)
                {
                    case LodQuality.MAXIMUM:
                        _surfacicTolerance = 0.01;
                        break;
                    case LodQuality.HIGH:
                        // In Studio = Decimate "Low"
                        _surfacicTolerance = 0.5;
                        _lineicTolerance = 0.1;
                        _normalTolerance = 1;
                        break;
                    case LodQuality.MEDIUM:
                        // In Studio = Decimate "Medium"
                        _surfacicTolerance = 1;
                        _normalTolerance = 8;
                        break;
                    case LodQuality.LOW:
                        // In Studio = Decimate "Strong"
                        _surfacicTolerance = 3;
                        _normalTolerance = 15;
                        break;
                    case LodQuality.POOR:
                    default:
                        _surfacicTolerance = 10;
                        _normalTolerance = 20;
                        break;
                }
                doDecimation = (quality != 0);
                surfacicTolerance = _surfacicTolerance;
                normalTolerance = _normalTolerance;
                lineicTolerance = _lineicTolerance;
                uvTolerance = _uvTolerance;
            }
        }

		public double maxSag;
		public double maxAngle;
		public bool createNormals;
		public bool createFreeEdges;
		public bool doDecimation;
		public double surfacicTolerance;
		public double lineicTolerance;
		public double normalTolerance;
		public double uvTolerance;
	}

}
