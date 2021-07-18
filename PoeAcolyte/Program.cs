﻿using System;
using System.Windows.Forms;
using PoeAcolyte.Helpers;
using PoeAcolyte.UI;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PoeAcolyte
{
    static class Program
    {
        private static Logger _logger;

        /// <summary>
        /// Default logging for POE Acolyte
        /// </summary>
        public static Logger Log
        {
            get
            {
                if (_logger is null)
                {
                    _logger ??= new LoggerConfiguration()
                        .WriteTo.File("testinfo.txt")
                        .MinimumLevel.Verbose()
                        .Enrich.FromLogContext()
                        .CreateLogger();
                }

                return _logger;
            }
        }

        public static PoeGameBroker GameBroker { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameBroker = new PoeGameBroker();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Overlay());
        }
    }
}