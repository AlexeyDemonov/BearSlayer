using System;

public interface IDestroyReporter
{
    event Action Destroying;
}