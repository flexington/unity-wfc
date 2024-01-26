namespace flexington.WFC
{
    /// <summary>
    /// Represents a cell in a WFC model.
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Whether the cell has collapsed.
        /// </summary>
        bool IsCollapsed { get; }

        /// <summary>
        /// The possible patterns for the cell.
        /// </summary>
        /// <remarks>
        /// The length of this array changes as surrounding cells collapse. 
        /// </remarks>
        IPattern[] Options { get; }

        /// <summary>
        /// The pattern of the cell after it has collapsed.
        /// </summary>
        /// <remarks>
        /// This is null until the cell has collapsed.
        /// </remarks>
        IPattern Pattern { get; }
    }
}