using BackEndProject.Data;
using BackEndProject.Interfaces;

namespace BackEndProject.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly JuanDbContext _context;

        public LayoutService(JuanDbContext context)
        {
            _context = context;
        }

        public IDictionary<string, string> GetSettings() => _context.Settings
            .Where(s => !s.IsDelete)
            .ToDictionary(s => s.Key, s => s.Value);

    }
}
