using System;
using System.Collections.Generic;
using flexington.Tools;
using UnityEditor;
using UnityEngine;

namespace flexington.WFC
{
    [CustomEditor(typeof(WaveFunctionCollapseEditorComponent))]
    public class WaveFunctionCollapseEditorInspector : Editor
    {
        private WaveFunctionCollapseEditorComponent _target;
        private Event _e;
        private Cell _selectedCell;
        private int _cellSelection = 0;
        private Vector2 _scrollPosition = Vector2.zero;

        private void OnEnable()
        {
            _target = (WaveFunctionCollapseEditorComponent)target;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnDrawButton(SceneView scene)
        {
            Handles.BeginGUI();
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            using (new HorizontalGroup())
            {
                using (new VerticalGroup(true, false))
                {
                    if (GUILayout.Button("Initialize")) _target.Initialize();
                    _cellSelection = EditorGUILayout.Popup("Pattern Size", _cellSelection, new[] { "1", "2", "3", "4", "5" });
                }
            }

            Handles.EndGUI();
        }



        private void OnSceneGUI(SceneView scene)
        {
            if (!_target.EditorIsActive) return;

            // Disable default scene view controls
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            // Cache current event
            _e = Event.current;

            // Handle mouse input
            var mouseWorldPosition = GetMouseWorldPosition();
            if (mouseWorldPosition.HasValue) _selectedCell = _target.GetCell(mouseWorldPosition.Value);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Initialize Grid")) _target.Initialize();
            if (GUILayout.Button("Reset Grid")) _target.Reset();
            if(GUILayout.Button("Save Grid")) _target.Save();

            if (_selectedCell != null)
            {
                EditorGUILayout.Space(10);
                if (GUILayout.Button("Deselect Cell")) _selectedCell = null;
                if (GUILayout.Button("Reset Cell")) _target.ResetCell(_selectedCell);
            }

            DrawOptions();

        }

        public Nullable<Vector2> GetMouseWorldPosition()
        {
            if (_e.type != EventType.MouseDown) return null;
            if (_e.button != 0) return null;

            var invertedPosition = new Vector3(_e.mousePosition.x, Camera.current.pixelHeight - _e.mousePosition.y);

            var mouseWorldPosition = SceneView
                .currentDrawingSceneView
                .camera
                .ScreenToWorldPoint(invertedPosition);

            return mouseWorldPosition;
        }

        private void DrawOptions()
        {
            if (_selectedCell == null) return;

            HashSet<KeyValuePair<int, Texture2D>> textures = new HashSet<KeyValuePair<int, Texture2D>>();
            foreach (var option in _selectedCell.Options)
            {
                var sprites = new Sprite[option.Tiles.GetLength(0), option.Tiles.GetLength(1)];
                for (int px = 0; px < option.Tiles.GetLength(0); px++)
                {
                    for (int py = 0; py < option.Tiles.GetLength(1); py++)
                    {
                        sprites[px, py] = (option.Tiles[px, py].TileBase as UnityEngine.Tilemaps.Tile).sprite;
                    }
                }
                var texture = sprites.ToTexture2D();
                textures.Add(new KeyValuePair<int, Texture2D>(textures.Count, texture));
            }

            using (new VerticalGroup())
            {
                using (new HorizontalGroup())
                {
                    EditorGUILayout.LabelField("Selected Cell", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Count: {_selectedCell.Position}");
                }
                using (new HorizontalGroup())
                {

                    EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Count: {textures.Count}");
                }
                foreach (var texture in textures)
                {
                    if (GUILayout.Button(texture.Value)) _target.CollapseCell(_selectedCell, texture.Key);
                }

            }
        }
    }

    public class Option
    {
        public int PixelHash { get; set; }

        public int Index { get; set; }

        public Texture2D Texture { get; set; }

        public override int GetHashCode()
        {
            return PixelHash;
        }
    }
}