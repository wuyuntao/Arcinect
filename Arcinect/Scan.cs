
namespace Arcinect
{
    class Scan : MainWindow.State
    {
        public Scan(MainWindow mainWindow)
            : base(mainWindow)
        {
            mainWindow.ScanButton.IsEnabled = false;
            mainWindow.RecordButton.IsEnabled = false;
            mainWindow.ReplayButton.IsEnabled = false;
            mainWindow.StopButton.IsEnabled = true;

            Scanner = Scanner.Open();
        }

        protected override void Become(MainWindow.State nextState)
        {
            if (Scanner != null)
            {
                Scanner.Dispose();
                Scanner = null;
            }

            base.Become(nextState);
        }

        public override void StopButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.StopButton_Click(sender, e);

            Become(new Idle(MainWindow));
        }
    }
}