using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingAppDTO.Context;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppBL.Repository
{
    public class PlannerRepository
    {
        private WeddingAppUserContext Context { get; set; }

        public PlannerRepository(WeddingAppUserContext weddingAppUserContext)
        {
            this.Context = weddingAppUserContext;
        }

        public Task<bool> AddNewComment(DateTime dateTimeStamp, string comment)
        {
            this.Context.PlannerComments.Add(new PlannerComment
            {
                DateTime = dateTimeStamp,
                Comment = comment
            });
            return Task.FromResult(true);
        }

        public Task<List<PlannerComment>> GetPlannerComments()
        {
            return Task.FromResult(this.Context.PlannerComments.ToList());
        }

        public Task<bool> DeleteComment(int commentId)
        {
            PlannerComment tmpComment = this.Context.PlannerComments.FirstOrDefault(x => x.Id == commentId);
            this.Context.PlannerComments.Remove(tmpComment);
            return Task.FromResult(true);
        }

        public Task<bool> EditComment(PlannerComment plannerComment)
        {
            this.Context.PlannerComments.Update(plannerComment);
            return Task.FromResult(true);
        }
    }
}
