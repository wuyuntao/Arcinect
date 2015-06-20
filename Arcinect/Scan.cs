
namespace Arcinect
{
    class Scan : MainWindow.State
    {
        /// <summary>
        /// Active scanner 
        /// </summary>
        private Scanner scanner;

        public Scan(MainWindow mainWindow)
            : base(mainWindow)
        {
            mainWindow.ScanButton.IsEnabled = false;
            mainWindow.RecordButton.IsEnabled = false;
            mainWindow.ReplayButton.IsEnabled = false;
            mainWindow.StopButton.IsEnabled = true;

            this.scanner = Scanner.Open();

            mainWindow.ColorCamera.Source = this.scanner.Frame.ColorBitmap;
        }

        protected override void Become(MainWindow.State nextState)
        {
            SafeDispose(ref this.scanner);

            MainWindow.ColorCamera.Source = null;

            base.Become(nextState);
        }

        public override void StopButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.StopButton_Click(sender, e);

            Become(new Idle(MainWindow));
        }
    }
}