using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using PoeAcolyte.UI;


namespace PoeAcolyte.Helpers
{
    public class Settings
    {
        public struct FrameProps
        {
            public Point Location { get; set; }
            public Size Size { get; set; }
        }

        public FrameProps StashTab { get; set; }
        public FrameProps Trades { get; set; }
        public string PoeClientLogPath { get; set; }

        private FrameControl _stashTabFrame;
        private FrameControl _tradeFrame;
        private const string DEFAULTFILENAME = "settings.json";

        private const string DEFAULTPOECLIENTLOG =
            @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\test.txt";

        public static Settings Default()
        {
            return new()
            {
                StashTab = new FrameProps() {Location = new Point(500, 500), Size = new Size(100, 100)},
                Trades = new FrameProps() {Location = new Point(1000, 1000), Size = new Size(100, 100)},
                PoeClientLogPath = DEFAULTPOECLIENTLOG
            };
        }

        /// <summary>
        /// Show resizeable boxes within the supplied controlcollection (in this
        /// case the main form) and save their respective sizes to this class
        /// </summary>
        /// <param name="controlCollection"></param>
        public void ShowOverlay(Control.ControlCollection controlCollection)
        {
            // TODO cleanup
            _stashTabFrame = new FrameControl()
            {
                Location = StashTab.Location,
                Size = StashTab.Size
            };
            _tradeFrame = new FrameControl()
            {
                Location = Trades.Location,
                Size = Trades.Size
            };
            // update our properties when the frame is resized
            _stashTabFrame.Resize += (sender, args) =>
            {
                if (sender?.GetType() != typeof(FrameControl)) return;
                FrameControl frame = (FrameControl) sender;
                StashTab = new FrameProps()
                {
                    Size = frame.Size,
                    Location = frame.Location
                };
            };
            _tradeFrame.Resize += (sender, args) =>
            {
                if (sender?.GetType() != typeof(FrameControl)) return;
                FrameControl frame = (FrameControl) sender;
                Trades = new FrameProps()
                {
                    Size = frame.Size,
                    Location = frame.Location
                };
            };

            controlCollection.Add(_stashTabFrame);
            controlCollection.Add(_tradeFrame);
            _stashTabFrame.BringToFront();
            _tradeFrame.BringToFront();
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
        /// <returns></returns>
        public static Settings Load()
        {
            try
            {
                return !File.Exists(DEFAULTFILENAME)
                    ? new Settings()
                    : JsonSerializer.Deserialize<Settings>(File.ReadAllText(DEFAULTFILENAME));
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString() + "\r\n using default instead");
                return Default();
            }
        }
    }
}