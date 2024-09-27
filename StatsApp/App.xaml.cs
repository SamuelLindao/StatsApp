using System;
using System.Windows;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Windows.Threading;
using System.Management;

namespace StatsApp
{
    public partial class App : Application
    {
        private IKeyboardMouseEvents _globalHook;
        public DateTime initTime; 
        
        public PerformanceCounter ramCounter;
        public PerformanceCounter cpuCounter;
        
        public DispatcherTimer timer;
        
        public int _keyPressCount = 0;
        public int _mouseClickCount = 0;

        public event Action<int, int> CountsUpdated;
        public event Action<float, float> RamUsageUpdated;
        public event Action<float> CpuUsageUpdated;
        public event Action<float, float> GpuMemoryUpdated;
        
        public int KeyPressCount => _keyPressCount;
        public int MouseClickCount => _mouseClickCount;
        

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHook_KeyDown;
            _globalHook.MouseDown += GlobalHook_MouseDown;
            
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            initTime = DateTime.Now;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var totalMemory = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / (1024 * 1024); 
            var availableMemory = ramCounter.NextValue(); 
            var usedMemory = totalMemory - availableMemory;

            RamUsageUpdated?.Invoke(usedMemory, totalMemory);
            
            var cpuUsage = cpuCounter.NextValue(); // Uso de CPU em %
            CpuUsageUpdated?.Invoke(cpuUsage);

            var gpuMemoryInfo = GetGpuMemoryUsage();
            if (gpuMemoryInfo != null)
            {
                GpuMemoryUpdated?.Invoke(gpuMemoryInfo.Item1, gpuMemoryInfo.Item2); // (memória usada, memória total)
            }
        }

        private Tuple<float, float> GetGpuMemoryUsage()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var adapterRAM = Convert.ToSingle(obj["AdapterRAM"]) / (1024 * 1024); // Memória total em MB
                        var usedMemory = adapterRAM - Convert.ToSingle(obj["CurrentRefreshRate"]); // Exemplo de cálculo de memória usada (valor fictício, pode variar)
                        return Tuple.Create(usedMemory, adapterRAM);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao acessar informações da GPU: {ex.Message}");
            }
            return null;
        }

        
        protected override void OnExit(ExitEventArgs e)
        {
            _globalHook.KeyDown -= GlobalHook_KeyDown;
            _globalHook.MouseDown -= GlobalHook_MouseDown;
            _globalHook.Dispose();
            base.OnExit(e);
        }

        private void GlobalHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            _keyPressCount++;
            CountsUpdated?.Invoke(_keyPressCount, _mouseClickCount); // Aciona o evento
        }

        private void GlobalHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _mouseClickCount++;
            CountsUpdated?.Invoke(_keyPressCount, _mouseClickCount); // Aciona o evento
        }
    }
}