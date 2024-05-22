using Kyrsach.Models;

namespace Kyrsach.Components
{
    public class CountViewComponent
    {
        private readonly SerovaContext _context;

        public CountViewComponent(SerovaContext context)
        {
            _context = context;
        }
        public string Invoke(int idT, int idQ)
        {
            string s = $"{_context.Questions.Where(t => t.IdTest == idT && t.IdQuestion <= idQ).Count()} / {_context.Questions.Where(t => t.IdTest == idT).Count()}";
            return s;
        }
    }
}
