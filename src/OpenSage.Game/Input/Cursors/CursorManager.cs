﻿using System.Collections.Generic;
using System.IO;
using OpenSage.Content;
using OpenSage.Gui;

namespace OpenSage.Input.Cursors
{
    internal sealed class CursorManager : DisposableBase
    {
        private readonly Dictionary<string, CursorBase> _cachedCursors;
        private CursorBase _currentCursor;

        private readonly GameWindow _window;
        private readonly AssetStore _assetStore;
        private readonly ContentManager _contentManager;

        private bool _useHardwareCursor;

        public bool UseHardwareCursor
        {
            get { return _useHardwareCursor; }
            set
            {
                _useHardwareCursor = value;
                _cachedCursors.Clear();
                _currentCursor = null;
            }
        }

        public CursorManager(GameWindow window, AssetStore assetStore, ContentManager contentManager)
        {
            _cachedCursors = new Dictionary<string, CursorBase>();

            _window = window;
            _assetStore = assetStore;
            _contentManager = contentManager;

            UseHardwareCursor = true;
        }

        public void SetCursor(string cursorName, in TimeInterval time)
        {
            if (!_cachedCursors.TryGetValue(cursorName, out var cursor))
            {
                var mouseCursor = _assetStore.MouseCursors.GetByName(cursorName);
                if (mouseCursor == null)
                {
                    return;
                }

                var cursorFileName = mouseCursor.Image;
                if (string.IsNullOrEmpty(Path.GetExtension(cursorFileName)))
                {
                    cursorFileName += ".ani";
                }

                string cursorDirectory;
                switch (_contentManager.SageGame)
                {
                    case SageGame.Cnc3:
                    case SageGame.Cnc3KanesWrath:
                        // TODO: Get version number dynamically.
                        cursorDirectory = Path.Combine("RetailExe", "1.0", "Data", "Cursors");
                        break;

                    default:
                        cursorDirectory = Path.Combine("Data", "Cursors");
                        break;
                }

                var cursorFilePath = Path.Combine(cursorDirectory, cursorFileName);
                var cursorEntry = _contentManager.FileSystem.GetFile(cursorFilePath);

                CursorBase newCursor = null;
                if(UseHardwareCursor)
                {
                    newCursor = new HardwareCursor(cursorEntry, _window);
                }
                else
                {
                    newCursor = new SoftwareCursor(cursorEntry, _window, _contentManager.GraphicsDevice);
                }

                _cachedCursors[cursorName] = cursor = AddDisposable(newCursor);
            }

            if (_currentCursor == cursor)
            {
                return;
            }

            _currentCursor = cursor;
            _currentCursor.Apply(time);
        }

        public void Update(in TimeInterval time)
        {
            _currentCursor?.Update(time);
        }

        public void Render(DrawingContext2D drawingContext)
        {
            _currentCursor?.Render(drawingContext);
        }
    }
}
