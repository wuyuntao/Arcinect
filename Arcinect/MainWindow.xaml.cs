using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arcinect
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Logger of current class
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Current behavior
        /// </summary>
        private State state;

        #region State

        /// <summary>
        /// Base class of MainWindow states
        /// </summary>
        internal abstract class State : Disposable
        {
            /// <summary>
            /// Logger of current class
            /// </summary>
            protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

            /// <summary>
            /// Reference of main window
            /// </summary>
            private MainWindow mainWindow;

            protected State(MainWindow mainWindow)
            {
                this.mainWindow = mainWindow;
            }

            protected virtual void Become(State nextState)
            {
                this.mainWindow.state = nextState;

                if (nextState != null)
                    logger.Trace("State changed from {0} to {1}", GetType().Name, nextState.GetType().Name);
                else
                    logger.Trace("State stoped from {0}", GetType().Name);
            }

            #region Events

            public virtual void ScanButton_Click(object sender, RoutedEventArgs e)
            {
                logger.Trace("ScanButton is clicked");
            }

            public virtual void RecordButton_Click(object sender, RoutedEventArgs e)
            {
                logger.Trace("RecordButton is clicked");
            }

            public virtual void ReplayButton_Click(object sender, RoutedEventArgs e)
            {
                logger.Trace("ReplayButton is clicked");
            }

            public virtual void StopButton_Click(object sender, RoutedEventArgs e)
            {
                logger.Trace("StopButton is clicked");
            }

            public virtual void SaveButton_Click(object sender, RoutedEventArgs e)
            {
                logger.Trace("SaveButton is clicked");
            }

            #endregion

            #region Properties

            protected MainWindow MainWindow
            {
                get { return this.mainWindow; }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logger.Trace("Loaded");

            state = new Idle(this);
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            logger.Trace("Closing");

            if (state != null)
            {
                state.Dispose();
                state = null;
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.state != null)
                this.state.ScanButton_Click(sender, e);
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.state != null)
                this.state.RecordButton_Click(sender, e);
        }

        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.state != null)
                this.state.ReplayButton_Click(sender, e);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.state != null)
                this.state.StopButton_Click(sender, e);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.state != null)
                this.state.SaveButton_Click(sender, e);
        }
    }
}