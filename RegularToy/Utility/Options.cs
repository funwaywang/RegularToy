using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using RegularToy.Properties;

namespace RegularToy
{
    static class Options
    {
        static WindowState _WindowState;
        static string _LastExpression;
        static string _LastText;
        static Size? _MainFormSize;
        static RegexOptions _RegexOptions;
        static string[] _History;

        // Methods
        static Options()
        {
            _WindowState = (WindowState)Settings.Default.WindowState;
            _LastExpression = Settings.Default.LastExpression;
            _LastText = Settings.Default.LastText;
            _MainFormSize = Str.GetSize(Settings.Default.MainFormSize);
            _RegexOptions = (RegexOptions)Settings.Default.RegexOptions;
            _History = Settings.Default.History.Split('\n');
        }

        public static WindowState WindowState
        {
            get
            {
                return _WindowState;
            }

            set
            {
                if (_WindowState != value)
                {
                    _WindowState = value;
                    Settings.Default.WindowState = (int)value;
                }
            }
        }

        public static string LastExpression
        {
            get
            {
                return _LastExpression;
            }

            set
            {
                if (_LastExpression != value)
                {
                    _LastExpression = value;
                    Settings.Default.LastExpression = value;
                }
            }
        }

        public static string LastText
        {
            get
            {
                return _LastText;
            }

            set
            {
                if (_LastText != value)
                {
                    _LastText = value;
                    Settings.Default.LastText = value;
                }
            }
        }

        public static Size? MainFormSize
        {
            get
            {
                return _MainFormSize;
            }

            set
            {
                if (_MainFormSize != value)
                {
                    _MainFormSize = value;
                    if (value.HasValue)
                        Settings.Default.MainFormSize = string.Format("{0},{1}", (int)value.Value.Width, (int)value.Value.Height);
                    else
                        Settings.Default.MainFormSize = string.Empty;
                }
            }
        }

        public static RegexOptions RegexOptions
        {
            get
            {
                return _RegexOptions;
            }

            set
            {
                if (_RegexOptions != value)
                {
                    _RegexOptions = value;
                    Settings.Default.RegexOptions = (int)value;
                }
            }
        }

        public static string[] History
        {
            get
            {
                return _History;
            }

            set
            {
                if (_History != value)
                {
                    _History = value;
                    Settings.Default.History = Str.JoinString(value, "\n");
                }
            }
        }

        public static void PushRegexHistory(string regular)
        {
        }
    }
}
