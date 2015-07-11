using Microsoft.Win32;
using System.Windows;

namespace Arcinect
{
    class Idle : MainWindow.State
    {
        public Idle(MainWindow mainWindow)
            : base(mainWindow)
        {
            mainWindow.ScanButton.IsEnabled = true;
            mainWindow.RecordButton.IsEnabled = true;
            mainWindow.ReplayButton.IsEnabled = true;
            mainWindow.StopButton.IsEnabled = false;
            mainWindow.SaveButton.IsEnabled = false;
        }

        public override void ScanButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ScanButton_Click(sender, e);

            Become(new Scan(MainWindow, null));
        }

        public override void RecordButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.RecordButton_Click(sender, e);

            var dialog = new SaveFileDialog()
            {
                FileName = "ArcinectFrameTimeline.ftl",
                Filter = "Frame Timeline Files|*.ftl",
            };

            if (dialog.ShowDialog() == true)
            {
                logger.Trace("Record to file {0}", dialog.FileName);

                Become(new Scan(MainWindow, dialog.FileName));
            }
        }

        public override void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            base.ReplayButton_Click(sender, e);

            MessageBox.Show("Not implemented yet", "Replay");
        }
    }
}