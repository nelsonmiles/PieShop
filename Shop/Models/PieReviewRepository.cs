using Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class PieReviewRepository : IPieReviewRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieReviewRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddPieReview(PieReview pieReview)
        {
            _appDbContext.PieReviews.Add(pieReview);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<PieReview> GetReviewsForPie(int pieId)
        {
            return _appDbContext.PieReviews.Where(p => p.Pie.PieId == pieId);
        }
    }
}
