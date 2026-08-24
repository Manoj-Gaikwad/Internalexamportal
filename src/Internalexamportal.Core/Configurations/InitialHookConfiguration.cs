namespace Internalexamportal.Core.Configurations
{
    public class InitialHookConfiguration
    {
        public string Commands { get; set; }

        public string[] _commandList
        {
            get { return Commands?.Split(","); }
        }
    }
}
