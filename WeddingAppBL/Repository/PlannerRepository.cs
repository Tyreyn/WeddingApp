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

        public Task<bool> AddNewComment(TimeSpan dateTimeStamp, string comment)
        {
            this.Context.PlannerComments.Add(new PlannerComment
            {
                DateTime = dateTimeStamp,
                Comment = comment
            });
            this.Context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<List<PlannerComment>> GetPlannerComments()
        {
            return Task.FromResult(this.Context.PlannerComments.ToList());
        }

        public Task<bool> DeleteComment(int commentId)
        {
            this.Context.ChangeTracker.Clear();
            PlannerComment tmpComment = this.Context.PlannerComments.FirstOrDefault(x => x.Id == commentId);
            this.Context.PlannerComments.Remove(tmpComment);
            this.Context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> EditComment(PlannerComment plannerComment)
        {
            this.Context.ChangeTracker.Clear();
            this.Context.PlannerComments.Update(plannerComment);
            this.Context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
