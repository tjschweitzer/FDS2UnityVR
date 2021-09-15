using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.LODTools.Editor
{
    [CustomPropertyDrawer(typeof(RulesThresholds))]
    public class LODsSettingsDrawer : PropertyDrawer {

        const int SPLITTER_WIDTH = 12;
        const float MINIMUM_LOD_RANGE = 0.01f;

        public static readonly Color[] LOD_COLORS_FOCUS = new Color[] {
            new Color(0.38039f, 0.49020f, 0.01961f),
            new Color(0.21961f, 0.32157f, 0.45882f),
            new Color(0.16471f, 0.41961f, 0.51765f),
            new Color(0.41961f, 0.12549f, 0.01961f),
            new Color(0.30196f, 0.22745f, 0.41569f),
            new Color(0.63137f, 0.34902f, 0.00000f),
            new Color(0.35294f, 0.32157f, 0.03922f),
            new Color(0.61176f, 0.50196f, 0.01961f),
        };

        // Todo : Light theme colors are different
        public static readonly Color[] LOD_COLORS = new Color[] {
            new Color(0.23529f, 0.27451f, 0.10196f),
            new Color(0.18039f, 0.21569f, 0.26275f),
            new Color(0.15686f, 0.25098f, 0.28627f),
            new Color(0.25098f, 0.14510f, 0.10588f),
            new Color(0.20784f, 0.18039f, 0.24706f),
            new Color(0.32549f, 0.22745f, 0.09804f),
            new Color(0.22745f, 0.21569f, 0.11373f),
            new Color(0.32157f, 0.27843f, 0.10588f),
        };

        public static readonly Color CULLED_COLOR = new Color(0.31373f, 0f, 0f);
        public static readonly Color CULLED_COLOR_FOCUS = new Color(0.62745f, 0f, 0f);
        public static readonly Color FRAME_COLOR_FOCUS = new Color(0.23922f, 0.37647f, 0.56863f);

        private int grabbing = -1;
        private bool isLodGrabbed => grabbing > -1;

        public static Color GetLodColor(int lodNbr, bool isCulled)
        {
            return isCulled ? CULLED_COLOR : LOD_COLORS[lodNbr];
        }

        public static GUIStyle _LodPercentTextStyle;
        public static GUIStyle LodPercentTextStyle {
            get {
                if (_LodPercentTextStyle == null) {
                    _LodPercentTextStyle = new GUIStyle();
                    _LodPercentTextStyle.alignment = TextAnchor.MiddleRight;
                    _LodPercentTextStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(.8f, .8f, .8f) : new Color(.1f, .1f, .1f);
                }
                return _LodPercentTextStyle;
            }
        }

        private float scale(float value)
        {
            return Mathf.Pow(value, 0.5f);
        }

        private float descale(float value)
        {
            return Mathf.Pow(value, 2);
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var thresholdsProperty = property.FindPropertyRelative("thresholds");
            List<double> thresholds = new List<double>();
            for (int i = 0; i < thresholdsProperty.arraySize; i++)
            {
                thresholds.Add((float)thresholdsProperty.GetArrayElementAtIndex(i).doubleValue);
            }

            var count = thresholds.Count;
            Rect sliderRect = EditorGUILayout.GetControlRect();
            sliderRect.x -= 10;
            sliderRect.y -= 2;
            sliderRect.height = 30;
            GUILayout.Space(20);
            float previousThreshold = 1f;

            float[] widths = new float[count];

            for (int i = 0; i < count; i++) {

                bool isLast = i == count - 1;
                float currentThreshold = (float)thresholds[i];

                widths[i] = scale(previousThreshold) - scale(currentThreshold);

                // Draw Block
                Rect labelRect = new Rect(
                    new Vector2(sliderRect.position.x + (1 - scale(previousThreshold)) * sliderRect.width, sliderRect.position.y),
                    new Vector2(sliderRect.width * widths[i], sliderRect.height)
                );
                GUIContent title = isLast ? new GUIContent($" Culled\n") : new GUIContent($" LOD {i}\n");
                title.tooltip = "";

                EditorGUIExtensions.GUIDrawRect(labelRect, GetLodColor(i, isLast), GetLodColor(i, isLast) * 0.8f, 1, title, TextAnchor.MiddleLeft);

                // Draw Splitter if not last
                if (!isLast) {
                    Rect splitter = new Rect(labelRect.x + labelRect.width, labelRect.y, SPLITTER_WIDTH, labelRect.height);
                    EditorGUI.LabelField(new Rect(splitter.x - 20, splitter.y - 20, 40, 20), (Math.Round(currentThreshold * 100)) + "%", LodPercentTextStyle);
                    EditorGUIUtility.AddCursorRect(splitter, MouseCursor.ResizeHorizontal);
                    if (splitter.Contains(Event.current.mousePosition) && (Event.current.type == EventType.MouseDown && Event.current.button == 0)) {
                        if (i < count)
                            grabbing = i;
                    }
                }

                previousThreshold = currentThreshold;
            }

            if (Event.current.type == EventType.MouseUp)
            {
                grabbing = int.MinValue;
            }
                    

            float mouseDeltaX = 0;
            if (grabbing != int.MinValue && Event.current.type == EventType.MouseDrag) {
                mouseDeltaX = Event.current.delta.x;
                // Triggers change (for Repaint in Editors)
                setDirty();
            }

            if (mouseDeltaX != 0 && grabbing < thresholds.Count && grabbing >= 0)
            {
                float threshold = (float)thresholds[grabbing];
                float delta = -mouseDeltaX / sliderRect.width;

                // Moves dragging LOD
                float max = (grabbing > 0) ? (float)thresholds[grabbing - 1] - MINIMUM_LOD_RANGE : 1 - MINIMUM_LOD_RANGE;
                float min = (grabbing < count) ? (float)thresholds[grabbing + 1] + MINIMUM_LOD_RANGE : MINIMUM_LOD_RANGE;
                float newThreshold = descale(scale(threshold) + delta);
                newThreshold = Mathf.Clamp(newThreshold, min, max);
                if (grabbing >= 0)
                    thresholdsProperty.GetArrayElementAtIndex(grabbing).doubleValue = newThreshold;
                // Triggers change (for Repaint in Editors)
                setDirty();
            }

            EditorGUI.EndProperty();
        }

        private void setDirty()
        {
            EditorGUIExtensions.Dirty = true;
        }

        private void deleteLOD(SerializedProperty lodsArray, int index)
        {
            lodsArray.DeleteArrayElementAtIndex(index);

            grabbing = int.MinValue;

            rearrangeThresholds(lodsArray);
        }

        private void insertLOD(SerializedProperty lodsArray, int index)
        {
            double before = index == 0 ? 1 : lodsArray.GetArrayElementAtIndex(index - 1).FindPropertyRelative("threshold").doubleValue;
            double after = lodsArray.GetArrayElementAtIndex(index).FindPropertyRelative("threshold").doubleValue;

            lodsArray.InsertArrayElementAtIndex(index);
            lodsArray.GetArrayElementAtIndex(index).FindPropertyRelative("threshold").doubleValue = (after + before) / 2;

            rearrangeQualities(lodsArray, 0);
            rearrangeQualities(lodsArray, lodsArray.arraySize - 1);
            rearrangeThresholds(lodsArray);
        }

        /// <summary>
        /// Rearranges lod qualities so that it is continuous, from LOD0 with the highest quality and LODN the lowest
        /// </summary>
        /// <param name="lodsArray"></param>
        private void rearrangeQualities(SerializedProperty lodsArray, int startingIndex)
        {
            int count = lodsArray.arraySize;
            int previousQuality = 0;

            // Propagate to to LOD0
            for (int i = startingIndex; i >= 0; i--) {
                int currentQuality = lodsArray.GetArrayElementAtIndex(i).FindPropertyRelative("quality").enumValueIndex;
                if (i != startingIndex && currentQuality >= previousQuality && previousQuality > 0) {
                    currentQuality = lodsArray.GetArrayElementAtIndex(i).FindPropertyRelative("quality").enumValueIndex = previousQuality - 1;
                }
                previousQuality = currentQuality;
            }
            // Propagate to to LODN
            for (int i = startingIndex; i < count; i++) {
                int currentQuality = lodsArray.GetArrayElementAtIndex(i).FindPropertyRelative("quality").enumValueIndex;
                if (i != startingIndex && currentQuality <= previousQuality) {
                    currentQuality = lodsArray.GetArrayElementAtIndex(i).FindPropertyRelative("quality").enumValueIndex = previousQuality + 1;
                }
                previousQuality = currentQuality;
            }
        }

        /// <summary>
        /// Rearranges thresholds to that the order is kept and there is a minimum delta between two lods
        /// </summary>
        /// <param name="lodsArray"></param>
        private void rearrangeThresholds(SerializedProperty lodsArray)
        {
            for (int i = 0; i < lodsArray.arraySize; ++i) {
                if (i + 1 < lodsArray.arraySize) {
                    SerializedProperty leftLOD = lodsArray.GetArrayElementAtIndex(i);
                    SerializedProperty rightLOD = lodsArray.GetArrayElementAtIndex(i + 1);
                    if (leftLOD.FindPropertyRelative("threshold").doubleValue <= rightLOD.FindPropertyRelative("threshold").doubleValue + 0.027)
                        leftLOD.FindPropertyRelative("threshold").doubleValue = rightLOD.FindPropertyRelative("threshold").doubleValue + 0.05;
                }
                if (i == lodsArray.arraySize - 1) {
                    lodsArray.GetArrayElementAtIndex(i).FindPropertyRelative("threshold").doubleValue = 0;
                }
            }
        }
    }
}