namespace tech.sublink.SubLinkEditor.Undo;

public interface IUndoCommand
{
    void Redo();
    void Undo();
}
