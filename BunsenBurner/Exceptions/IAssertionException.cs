namespace BunsenBurner.Exceptions;

/// <summary>
/// Marker interface for xUnit assertion exceptions.
/// Implementing this interface indicates that an exception represents a test assertion failure
/// and should be handled appropriately by the xUnit test runner.
/// See <a href="https://xunit.net/docs/getting-started/v3/whats-new">Third party assertion library extension points</a>
/// for more information.
/// </summary>
public interface IAssertionException;
