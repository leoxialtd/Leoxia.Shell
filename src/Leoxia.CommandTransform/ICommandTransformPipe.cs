namespace Leoxia.CommandTransform
{
    public interface ICommandTransformPipe
    {
        string Transform(string commandLine);
    }
}