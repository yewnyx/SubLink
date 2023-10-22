using System.Collections;

namespace tech.sublink.SubLinkEditor.Controls.Utils;

/// <summary>
/// Arguments to the ItemsAdded and ItemsRemoved events.
/// </summary>
internal class CollectionItemsEventArgs : EventArgs {
    /// <summary>
    /// The list of items that were cleared from the list.
    /// </summary>
    private readonly ICollection _items = null;

    public CollectionItemsEventArgs(ICollection items) {
        _items = items;
    }

    /// <summary>
    /// The list of items that were cleared from the list.
    /// </summary>
    public ICollection Items => _items;
}
