using System.Diagnostics;
using System.IO.Ports;

SerialPort serialPort = null;
try
{
    serialPort = new SerialPort("To replaced by Arduino Port, eg. COM5", 9600);
    serialPort.Open();
    PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    PerformanceCounter ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
    while (true)
    {
        int ramPercentage = (int)ramCounter.NextValue();
        int cpuUsage = (int)cpuCounter.NextValue();
        Thread.Sleep(2000);
        cpuUsage = (int)cpuCounter.NextValue();
        ramPercentage = (int)ramCounter.NextValue();

        Console.WriteLine("RAM Usage: {0}%", ramPercentage);
        Console.WriteLine("CPU Usage: {0}%", cpuUsage.ToString("00"));
        serialPort.WriteLine(cpuUsage.ToString() + ";" + ramPercentage.ToString() + "\n");
    }
}
catch (Exception)
{
    if (serialPort != null && serialPort.IsOpen)
        serialPort.Close();
    Console.WriteLine("Comunication with Arduino ended");
}
