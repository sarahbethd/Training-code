using System.Collections.Generic;
using System.Linq;
using Entity = RRDL.Entities;
using Model = RRModels;

namespace RRDL
{
    public class RespositoryCloud : IRepository
    {
        private Entity.RRDatabaseContext _context;
        public RespositoryCloud(Entity.RRDatabaseContext p_context) 
        {
            _context = p_context;
        }

        public Model.Restaurant AddRestaurant(Model.Restaurant p_rest)
        {
            _context.Restaurants.Add
            (
                new Entity.Restaurant()
                {
                    RestName = p_rest.Name,
                    RestCity = p_rest.City,
                    RestState = p_rest.State
                }
            );

            //This method will save the changes made to the database
            _context.SaveChanges();

            return p_rest;
        }

        public List<Model.Restaurant> GetAllRestaurant()
        {
            //Method Syntax
            return _context.Restaurants.Select(rest => 
                new Model.Restaurant()
                {
                    Name = rest.RestName,
                    State = rest.RestState,
                    City = rest.RestCity,
                    Id = rest.RestId
                }
            ).ToList();


            //Query Syntax
            // var result = (from rest in _context.Restaurants
            //             select rest);

            // List<Model.Restaurant> listOfRest = new List<Model.Restaurant>();
            // foreach (var rest in result)
            // {
            //     listOfRest.Add(new Model.Restaurant(){
            //         Name = rest.RestName,
            //         State = rest.RestState,
            //         City = rest.RestCity,
            //         Id = rest.RestId
            //     });
            // }

            // return listOfRest;
        }

        public Model.Restaurant GetRestaurantById(int p_id)
        {
            Entity.Restaurant restToFind = _context.Restaurants.Find(p_id);
            
            return new Model.Restaurant(){
                Id = restToFind.RestId,
                Name = restToFind.RestName,
                State = restToFind.RestState,
                City = restToFind.RestCity
            };
        }

        public List<Model.Review> GetAllReview(Model.Restaurant p_rest)
        {
            //Query syntax
            var result = (from rev in _context.Reviews
                        where rev.RestId == p_rest.Id
                        select rev);

            //Mapping the Queryable<Entity.Review> into a list<Model.Review>
            List<Model.Review> listOfReview = new List<Model.Review>();
            foreach (Entity.Review rev in result)
            {
                listOfReview.Add(new Model.Review(){
                    Id = rev.RevId,
                    Rating = rev.RevRating,
                    RestId = rev.RestId
                });
            }

            return listOfReview;
        }
    }
}