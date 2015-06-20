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
        }

        public override void ScanButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ScanButton_Click(sender, e);

            Become(new Scan(MainWindow));
        }

        public override void RecordButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.RecordButton_Click(sender, e);

            MessageBox.Show("Not implemented yet", "Record");
        }

        public override void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            base.ReplayButton_Click(sender, e);

            MessageBox.Show("Not implemented yet", "Replay");
        }
    }
}