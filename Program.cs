using System;

class Program
{
    static void Main(string[] args)
    {
        List<double> voltages = new() {4.5, 5.2, 4.8, 5.5, 4.9};

        double maxLimit = 5.0;
        int passedCount = 0;
        double total = 0.0;

        foreach (double voltage in voltages)
        {
            bool passed = IsVoltageValid(voltage, maxLimit);
            
            if (passed)
            {
                Console.WriteLine($"{voltage} V: pass");
                passedCount++;
            }
            else
            {
                Console.WriteLine($"{voltage} V: fail");
            }

            total += voltage;
        }

        double average = total / voltages.Count;
        Console.WriteLine();
        Console.WriteLine($"Average voltage: {average} V");
    }

    static bool IsVoltageValid(double voltage, double maxLimit)
    {
        return voltage <= maxLimit;
    }
}