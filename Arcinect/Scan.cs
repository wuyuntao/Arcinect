
using Microsoft.Win32;
using System.Threading;
namespace Arcinect
{
    class Scan : MainWindow.State
    {
        private Scanner scanner;

        private VolumeBuilder volume;

        private Recorder recorder;

        public Scan(MainWindow mainWindow, string timelineFileName)
            : base(mainWindow)
        {
            mainWindow.ScanButton.IsEnabled = false;
            mainWindow.RecordButton.IsEnabled = false;
            mainWindow.ReplayButton.IsEnabled = false;
            mainWindow.StopButton.IsEnabled = true;
            mainWindow.SaveButton.IsEnabled = true;

            this.scanner = Scanner.Open();
            this.scanner.Frame.OnDataUpdate += Frame_OnDataUpdate;

            if (timelineFileName != null)
                this.recorder = new Recorder(timelineFileName);

            this.volume = new VolumeBuilder(scanner, mainWindow.Dispatcher);

            mainWindow.ColorCamera.Source = this.scanner.Frame.ColorBitmap;
            mainWindow.DepthCamera.Source = this.scanner.Frame.DepthBitmap;
            mainWindow.VolumeCamera.Source = this.volume.VolumeBitmap;
        }

        protected override void DisposeManaged()
        {
            //SafeDispose(ref this.scanner);
            SafeDispose(ref this.volume);
            SafeDispose(ref this.recorder);

            MainWindow.ColorCamera.Source = null;
            MainWindow.DepthCamera.Source = null;
            MainWindow.VolumeCamera.Source = null;

            base.DisposeManaged();
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
                Filter = "OBJ Mesh Files|*.obj",
            };

            if (dialog.ShowDialog() == true)
            {
                using (var writer = new ObjMeshWriter(dialog.FileName))
                {
                    writer.Write(this.volume.CreateMesh());
                }
            }
        }

        private void Frame_OnDataUpdate(object sender)
        {
            if (this.recorder != null)
                this.recorder.AppendFrame(this.scanner.Frame.ColorData, this.scanner.Frame.DepthData);
        }
    }
}