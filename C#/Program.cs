using System.Diagnostics;
using System.IO.Ports;
using System.Management;

SerialPort serialPort = null;
uint Maxsp;
Double temperature = 0;
String instanceName = "";

try
{
    serialPort = new SerialPort("COM3", 9600);
    serialPort.Open();
    PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    PerformanceCounter ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
    PerformanceCounter cpuClock = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
    PerformanceCounter diskCounter = new ("PhysicalDisk", "% Disk Time", "_Total");
    using(ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
    {
        Maxsp = (uint)(Mo["MaxClockSpeed"]);
    }

    while (true)
    {
        int ramPercentage = (int)ramCounter.NextValue();
        int cpuUsage = (int)cpuCounter.NextValue();
        int diskUsage = Convert.ToInt32(diskCounter.NextValue());
        double cpuValue = cpuClock.NextValue();
        int clockValue = (int)(cpuValue * Maxsp)/100;

        Thread.Sleep(1000);
        Console.WriteLine("RAM Usage: {0}%", ramPercentage);
        Console.WriteLine("CPU Usage: {0}%", cpuUsage.ToString("00"));
        Console.WriteLine("Disk Usage: {0}%", diskUsage.ToString("00"));
        Console.WriteLine("CPU Clock: {0}MHz", ((int)cpuValue * Maxsp)/100);
        serialPort.WriteLine(cpuUsage.ToString() + ";" + clockValue.ToString() + "[" + diskUsage.ToString() + "]" + ramPercentage.ToString() + "\n");
    }
}
catch (Exception e)
{   
    Console.WriteLine(e);
    if (serialPort != null && serialPort.IsOpen)
        serialPort.Close();
    Console.WriteLine("Comunication with Arduino ended");
}
