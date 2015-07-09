
using Microsoft.Win32;
namespace Arcinect
{
    class Scan : MainWindow.State
    {
        private Scanner scanner;

        private VolumeBuilder volume;

        public Scan(MainWindow mainWindow)
            : base(mainWindow)
        {
            mainWindow.ScanButton.IsEnabled = false;
            mainWindow.RecordButton.IsEnabled = false;
            mainWindow.ReplayButton.IsEnabled = false;
            mainWindow.StopButton.IsEnabled = true;
            mainWindow.SaveButton.IsEnabled = true;

            this.scanner = Scanner.Open();
            this.volume = new VolumeBuilder(scanner);

            mainWindow.ColorCamera.Source = this.scanner.Frame.ColorBitmap;
            mainWindow.DepthCamera.Source = this.scanner.Frame.DepthBitmap;
            MainWindow.VolumeCamera.Source = this.volume.VolumeBitmap;
        }

        protected override void Become(MainWindow.State nextState)
        {
            SafeDispose(ref this.scanner);
            SafeDispose(ref this.volume);

            MainWindow.ColorCamera.Source = null;
            MainWindow.DepthCamera.Source = null;
            MainWindow.VolumeCamera.Source = null;

            base.Become(nextState);
        }

        public override void StopButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.StopButton_Click(sender, e);

            Become(new Idle(MainWindow));
        }

        public override void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            base.SaveButton_Click(sender, e);

            var dialog = new SaveFileDialog()
            {
                FileName = "ArcinectMesh.obj",
                Filter = "OBJ Mesh Files|*.obj|All Files|*.*",
            };

            if (dialog.ShowDialog() == true)
            {
                using (var writer = new ObjMeshWriter(dialog.FileName))
                {
                    writer.Write(this.volume.CreateMesh());
                }
            }
        }
    }
}