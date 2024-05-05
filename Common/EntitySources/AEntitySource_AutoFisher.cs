namespace AutoFisher.Common.EntitySources
{
    public interface IEntitySource_AutoFisher : IEntitySource
    {

    }

    public abstract class AEntitySource_AutoFisher : IEntitySource_AutoFisher
    {
        private readonly string _context;
        public string Context => _context;

        public AEntitySource_AutoFisher()
        {
            _context = string.Empty;
        }
        public AEntitySource_AutoFisher(string context)
        {
            _context = context;
        }
    }
}
