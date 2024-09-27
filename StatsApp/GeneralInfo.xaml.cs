using System.Windows.Controls;
using System.Windows.Forms;

namespace StatsApp;

public partial class GeneralInfo : Page
{
    public GeneralInfo()
    {
        App app = (App)System.Windows.Application.Current;
        InitializeComponent();
        app.CountsUpdated += UpdateCounts;  
        UpdateCounts(app._keyPressCount, app._mouseClickCount);
        app.RamUsageUpdated += UpdateRamInfo;
        app.CpuUsageUpdated += UpdateCpuInfo;
        app.GpuMemoryUpdated += UpdateGpuInfo;
        
    }
    private void UpdateRamInfo(float usedMemory, float totalMemory)
    {
        RAMinfo.Content = $"Uso de RAM: {usedMemory} MB / {totalMemory} MB";
        RAMBar.Maximum = 1;
        RAMBar.Value = usedMemory / totalMemory;
    }
    private void UpdateCpuInfo(float cpuUsage)
    {
        CPUBar.Maximum = 100;
        CPUBar.Value = cpuUsage;
        CPUinfo.Content = $"Uso de CPU: {cpuUsage}%";
    }

    private void UpdateGpuInfo(float usedGpuMemory, float totalGpuMemory)
    {
        App app = (App)System.Windows.Application.Current;
        TimeSpan timeNow = (DateTime.Now - app.initTime);
        UseInfo.Content = timeNow.Hours.ToString("00") + ":" + timeNow.Minutes.ToString("00") + ":"  + timeNow.Seconds.ToString("00");
        
        GPUinfo.Content = $"Uso de Memória da GPU: {usedGpuMemory} MB / {totalGpuMemory} MB";
        GPUBar.Maximum = 1;
        GPUBar.Value = usedGpuMemory / totalGpuMemory;
    }
    private void UpdateCounts(int keyPressCount, int mouseClickCount)
    {
       
        KeyBoardInfo.Content = $"k{keyPressCount}";
        MouseInfo.Content = $"m{mouseClickCount}";
    }
    
}