using System.Runtime.CompilerServices;
using TestFlow.Models;

namespace TestFlow.Tests;

public class RangeTestTests
{
    [Fact]
    public void IsPassed_WhenValueWithinLimits_ReturnsTrue()
    {
        RangeTest test = new()
        {
            TestName = "Voltage Test",
            MeasuredValue = 4.8,
            MinLimit = 4.5,
            MaxLimit = 5.0
        };

        bool result = test.IsPassed();
        Assert.True(result);
    }

    [Fact]
    public void IsPassed_WhenValueAboveMaxLimits_ReturnsFalse()
    {
        RangeTest test = new()
        {
            TestName = "voltage Test 2",
            MeasuredValue = 5.5,
            MinLimit = 4.5,
            MaxLimit = 5.0
        };

        bool result = test.IsPassed();
        Assert.False(result);
    }

    [Fact]
    public void IsPassed_WhenMinLimitAboveMaxLimit_ThrowsArgumentException()
    {
        RangeTest test = new()
        {
            TestName = "Invalid Voltage Test",
            MeasuredValue = 4.8,
            MinLimit = 6.0,
            MaxLimit = 5.0
        };

        Assert.Throws<ArgumentException>(() => test.IsPassed());
    }

}
