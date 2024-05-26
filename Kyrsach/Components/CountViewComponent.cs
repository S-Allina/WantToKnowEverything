using DocumentFormat.OpenXml.Office2010.Excel;
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
        public string CreateString(int idT, int idQ)
        {
            return $"{_context.Questions.Where(t => t.IdTest == idT && t.IdQuestion <= idQ).Count()} / {_context.Questions.Where(t => t.IdTest == idT).Count()}";

        }
        public string Invoke(int idT, int idQ)
        {
            return CreateString(idT, idQ);
        }
    }
}
