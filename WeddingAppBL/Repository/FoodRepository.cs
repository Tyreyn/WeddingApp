using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingAppDTO.Context;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppBL.Repository
{
    public class FoodRepository
    {
        private WeddingAppUserContext Context { get; set; }

        public FoodRepository(WeddingAppUserContext weddingAppUserContext)
        {
            this.Context = weddingAppUserContext;
        }

        public Task<bool> AddNewFood(string name, string type)
        {
            this.Context.Foods.Add(new Food
            {
                Name = name,
                Type = type
            });
            this.Context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<List<Food>> GetFoods()
        {
            return Task.FromResult(this.Context.Foods.ToList());
        }

        public Task<bool> DeleteFood(int foodId)
        {
            this.Context.ChangeTracker.Clear();
            Food tmpFood = this.Context.Foods.FirstOrDefault(x => x.Id == foodId);
            this.Context.Foods.Remove(tmpFood);
            this.Context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> EditFood(Food food)
        {
            this.Context.ChangeTracker.Clear();
            this.Context.Foods.Update(food);
            this.Context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
