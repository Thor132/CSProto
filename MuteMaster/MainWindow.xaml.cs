using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;


namespace MuteMaster
{
    public class Test
    {
        public Dictionary<NAudio.CoreAudioApi.MMDevice, bool> audioDevices = new Dictionary<NAudio.CoreAudioApi.MMDevice, bool>();

        public Test()
        {
            GetAudioDevices();
            SetDeviceMute(true);
        }

        public void GetAudioDevices()
        {
            NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            NAudio.CoreAudioApi.MMDeviceCollection devices = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
            try
            {
                foreach (NAudio.CoreAudioApi.MMDevice dev in devices)
                {
                    try
                    {
                        bool mute = dev.AudioEndpointVolume.Mute;
                        System.Diagnostics.Debug.Print("Device {0} mute value: {1} ", dev.FriendlyName, mute);
                        audioDevices[dev] = mute;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public void SetDeviceMute(bool mute)
        {
            if (mute)
            {
                foreach (var device in this.audioDevices)
                {
                    if (!device.Value)
                    {
                        System.Diagnostics.Debug.Print("Muting device {0}", device.Key.FriendlyName);
                        device.Key.AudioEndpointVolume.Mute = true;
                    }
                }
            }
            else
            {
                foreach (var device in this.audioDevices)
                {
                    if (!device.Value)
                    {
                        System.Diagnostics.Debug.Print("Unmuting device {0}", device.Key.FriendlyName);
                        device.Key.AudioEndpointVolume.Mute = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Test test = null;

        public MainWindow()
        {
            InitializeComponent();
            test = new Test();
            test = null;
        }

        ~MainWindow()
        {
            test = null;
        }
    }
}
