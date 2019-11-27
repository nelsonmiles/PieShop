using Microsoft.EntityFrameworkCore;
using Shop.Model;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Models
{
    public class PieRepository: IPieRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Pie> Pies
        {
            get
            {
                return _appDbContext.Pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _appDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie GetPieById(int pieId)
        {
            return _appDbContext.Pies.Include(p => p.PieReviews).FirstOrDefault(p => p.PieId == pieId);
        }

        public void UpdatePie(Pie pie)
        {
            _appDbContext.Pies.Update(pie);
            _appDbContext.SaveChanges();
        }

        public void CreatePie(Pie pie)
        {
            _appDbContext.Pies.Add(pie);
            _appDbContext.SaveChanges();
            }

        public void Delete(Pie pie)
            {
            _appDbContext.Pies.Remove(pie);
            _appDbContext.SaveChanges();
            }
        }
}
