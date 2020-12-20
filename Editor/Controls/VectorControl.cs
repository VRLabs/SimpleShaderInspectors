using UnityEditor;
using UnityEngine;

namespace VRLabs.SimpleShaderInspectors.Controls
{
    /// <summary>
    /// Represents a vector control.
    /// </summary>
    public class VectorControl : PropertyControl
    {
        private readonly int _visibleCount;
        public bool IsXVisible { get; protected set; }
        public bool IsYVisible { get; protected set; }
        public bool IsZVisible { get; protected set; }
        public bool IsWVisible { get; protected set; }
        /// <summary>
        /// Default constructor of <see cref="VectorControl"/>
        /// </summary>
        /// <param name="propertyName">Material property name.</param>
        /// <param name="isXVisible">Shows the x component. Optional (Default true).</param>
        /// <param name="isYVisible">Shows the y component. Optional (Default true).</param>
        /// <param name="isZVisible">Shows the z component. Optional (Default true).</param>
        /// <param name="isWVisible">Shows the w component. Optional (Default true).</param>
        /// <returns>A new <see cref="VectorControl"/> object.</returns>
        public VectorControl(string propertyName, bool isXVisible = true, bool isYVisible = true,
             bool isZVisible = true, bool isWVisible = true) : base(propertyName)
        {
            IsXVisible = isXVisible;
            IsYVisible = isYVisible;
            IsZVisible = isZVisible;
            IsWVisible = isWVisible;

            _visibleCount = 0;
            if (IsXVisible) _visibleCount++;
            if (IsYVisible) _visibleCount++;
            if (IsZVisible) _visibleCount++;
            if (IsWVisible) _visibleCount++;
        }

        /// <summary>
        /// Draws the control represented by this object.
        /// </summary>
        /// <param name="materialEditor">Material editor.</param>
        protected override void ControlGUI(MaterialEditor materialEditor)
        {
            Rect r = EditorGUILayout.GetControlRect(true);
            EditorGUI.LabelField(r, Content);
            EditorGUI.showMixedValue = Property.hasMixedValue;
            EditorGUI.BeginChangeCheck();

            r = new Rect(r.x + EditorGUIUtility.labelWidth, r.y, r.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            int i = 0;
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // I'll be honest, it looks somewhat retarded, but it's not worth to make an array and cycle it, it would waste more resources.
            Vector4 vector = new Vector4(0, 0, 0, 0);
            if (IsXVisible)
                vector.x = DrawSingleField("X", Property.vectorValue.x, GetFragmentedRect(r, _visibleCount, i++));
            else
                vector.x = Property.vectorValue.x;

            if (IsYVisible)
                vector.y = DrawSingleField("Y", Property.vectorValue.y, GetFragmentedRect(r, _visibleCount, i++));
            else
                vector.y = Property.vectorValue.y;

            if (IsZVisible)
                vector.z = DrawSingleField("Z", Property.vectorValue.z, GetFragmentedRect(r, _visibleCount, i++));
            else
                vector.z = Property.vectorValue.z;

            if (IsWVisible)
                vector.w = DrawSingleField("W", Property.vectorValue.w, GetFragmentedRect(r, _visibleCount, i++));
            else
                vector.w = Property.vectorValue.w;

            HasPropertyUpdated = EditorGUI.EndChangeCheck();
            if (HasPropertyUpdated)
            {
                materialEditor.RegisterPropertyChangeUndo(Property.displayName);
                Property.vectorValue = vector;
            }

            EditorGUI.showMixedValue = false;
            EditorGUI.indentLevel = oldIndentLevel;
        }

        private static Rect GetFragmentedRect(Rect r, int count, int current)
        {
            return new Rect(r.x + (r.width * current / count), r.y, r.width / count, r.height);
        }

        private static float DrawSingleField(string label, float value, Rect r)
        {
            Rect rt = new Rect(r.x, r.y, 15, r.height);
            GUI.Label(rt, label, Styles.CenterLabel);
            rt = new Rect(r.x + 15, r.y, r.width - 15, r.height);
            return EditorGUI.FloatField(rt, value);
        }
    }

    public static partial class ControlExtensions
    {
        /// <summary>
        /// Creates a new control of type <see cref="VectorControl"/> and adds it to the current container.
        /// </summary>
        /// <param name="container">Container of controls this method extends to.</param>
        /// <param name="propertyName">Material property name.</param>
        /// <param name="isXVisible">Shows the x component. Optional (Default true).</param>
        /// <param name="isYVisible">Shows the y component. Optional (Default true).</param>
        /// <param name="isZVisible">Shows the z component. Optional (Default true).</param>
        /// <param name="isWVisible">Shows the w component. Optional (Default true).</param>
        /// <returns>The <see cref="VectorControl"/> object that has been added.</returns>
        public static VectorControl AddVectorControl(this IControlContainer container, string propertyName,
            bool isXVisible = true, bool isYVisible = true, bool isZVisible = true, bool isWVisible = true)
        {
            VectorControl control = new VectorControl(propertyName, isXVisible, isYVisible, isZVisible, isWVisible);
            container.Controls.Add(control);
            return control;
        }
    }
}