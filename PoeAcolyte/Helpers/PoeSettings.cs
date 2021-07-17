using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using PoeAcolyte.UI;


namespace PoeAcolyte.Helpers
{
    /// <summary>
    /// Settings library used to save session to session information (and certain defaults)
    /// </summary>
    public class PoeSettings
    {
        /// <summary>
        /// Used to specify areas of bounding boxes
        /// </summary>
        public struct FrameProps
        {
            /// <summary>
            /// X, Y (zero based) Location
            /// </summary>
            public Point Location { get; set; }
            /// <summary>
            /// Width, Height (zero based) Size
            /// </summary>
            public Size Size { get; set; }
        }

        /// <summary>
        /// Bounding area for the stash tab
        /// </summary>
        public FrameProps StashTab { get; set; }
        /// <summary>
        /// Bounding area for the trade controls
        /// </summary>
        public FrameProps Trades { get; set; }
        /// <summary>
        /// File path to the client.txt log for POE
        /// </summary>
        public string PoeClientLogPath { get; set; }

        private FrameControl _stashTabFrame;
        private FrameControl _tradeFrame;
        private const string DEFAULTFILENAME = "settings.json";

        private const string DEFAULTPOECLIENTLOG =
            @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\test.txt";

        /// <summary>
        /// Used to populate a generic <see cref="PoeSettings"/>
        /// </summary>
        /// <returns><see cref="PoeSettings"/></returns>
        public static PoeSettings Default()
        {
            return new()
            {
                StashTab = new FrameProps() {Location = new Point(500, 500), Size = new Size(100, 100)},
                Trades = new FrameProps() {Location = new Point(1000, 1000), Size = new Size(100, 100)},
                PoeClientLogPath = DEFAULTPOECLIENTLOG
            };
        }

        /// <summary>
        /// Show resizeable boxes within the supplied control collection (in this
        /// case the main form) and save their respective sizes to this class
        /// </summary>
        /// <param name="controlCollection"></param>
        public void ShowOverlay(Control.ControlCollection controlCollection)
        {
            InitFrameControls();
            controlCollection.Add(_stashTabFrame);
            controlCollection.Add(_tradeFrame);
            _stashTabFrame.BringToFront();
            _tradeFrame.BringToFront();
        }

        /// <summary>
        /// Initialize the <see cref="FrameControl"/> if they do not already exist
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitFrameControls()
        {
            if (_stashTabFrame is null)
            {
                _stashTabFrame = new FrameControl()
                {
                    Location = StashTab.Location,
                    Size = StashTab.Size,
                    Description = "Stash Tab"
                };
                // update our properties when the frame is resized
                // can probably be more efficient hooking to some type of resized.finished
                _stashTabFrame.Resize += (sender, _) =>
                {
                    if (sender?.GetType() != typeof(FrameControl)) return;
                    FrameControl frame = (FrameControl) sender;
                    StashTab = new FrameProps()
                    {
                        Size = frame.Size,
                        Location = frame.Location
                    };
                };
            }

            if (_tradeFrame is not null) return;
            {
                _tradeFrame ??= new FrameControl()
                {
                    Location = Trades.Location,
                    Size = Trades.Size,
                    Description = "Trade Notifications"
                };
                _tradeFrame.Resize += (sender, _) =>
                {
                    if (sender?.GetType() != typeof(FrameControl)) return;
                    FrameControl frame = (FrameControl) sender;
                    Trades = new FrameProps()
                    {
                        Size = frame.Size,
                        Location = frame.Location
                    };
                };
            }
        }

        /// <summary>
        /// remove the control from the supplied collection
        /// </summary>
        /// <param name="controlCollection"></param>
        public void HideOverlay(Control.ControlCollection controlCollection)
        {
            // Do we need to clean up the lambda for resizing from earlier?
            controlCollection.Remove(_stashTabFrame);
            controlCollection.Remove(_tradeFrame);
        }

        /// <summary>
        /// Save class to file "settings.json"
        /// </summary>
        public void Save()
        {
            File.WriteAllText(DEFAULTFILENAME, JsonSerializer.Serialize(this));
        }

        /// <summary>
        /// Load class from "settings.json"
        /// </summary>
        /// <returns>this populated from file or <see cref="Default"/> if file does not exist</returns>
        public static PoeSettings Load()
        {
            try
            {
                return !File.Exists(DEFAULTFILENAME)
                    ? Default()
                    : JsonSerializer.Deserialize<PoeSettings>(File.ReadAllText(DEFAULTFILENAME));
            }
            catch (Exception e)
            {
                Debug.Print(e + "\r\n using default instead");
                return Default();
            }
        }
    }
}