using System;

public interface ITransitionOnDeath
{
    // Method defining code that should be run before an object
    //   is set to die (Free, QueueFree, etc.) by Transition (or similar)
    public void OnDeath();
}
