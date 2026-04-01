namespace Creatio.Copilot
{
    /// <summary>
    /// Defines a factory interface for creating instances of IntentToolExecutor.
    /// </summary>
    public interface IIntentToolExecutorFactory
    {
        /// <summary>
        /// Creates a new instance of IntentToolExecutor using the specified CopilotIntentSchema.
        /// </summary>
        /// <param name="copilotIntentSchema">Copilot intent schema.</param>
        /// <returns></returns>
        IntentToolExecutor Create(CopilotIntentSchema copilotIntentSchema);
    }
}

