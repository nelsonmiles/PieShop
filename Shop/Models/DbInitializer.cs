using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
    {
    public static class DbInitializer
        {
        public static void Seed(AppDbContext context)
            {

            if (!context.Categories.Any())
                {
                context.Categories.AddRange(Categories.Select(c => c.Value));
                }

            //if (!context.Pies.Any())
            //    {
            //    context.AddRange
            //    (
            //        new Pie { Name = "Peach Pie", Price = 15.95M, ShortDescription = "Sweet as peach", LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll marzipan marshmallow toffee brownie brownie candy tootsie roll. Chocolate cake gingerbread tootsie roll oat cake pie chocolate bar cookie dragée brownie. Lollipop cotton candy cake bear claw oat cake. Dragée candy canes dessert tart. Marzipan dragée gummies lollipop jujubes chocolate bar candy canes. Icing gingerbread chupa chups cotton candy cookie sweet icing bonbon gummies. Gummies lollipop brownie biscuit danish chocolate cake. Danish powder cookie macaroon chocolate donut tart. Carrot cake dragée croissant lemon drops liquorice lemon drops cookie lollipop toffee. Carrot cake carrot cake liquorice sugar plum topping bonbon pie muffin jujubes. Jelly pastry wafer tart caramels bear claw. Tiramisu tart pie cake danish lemon drops. Brownie cupcake dragée gummies.", Category = Categories["Fruit pies"], ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/peachpie.jpg", InStock = false, IsPieOfTheWeek = false, ImageThumbnailUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/peachpiesmall.jpg", AllergyInformation = "" }
            //          );
            //    }

            context.SaveChanges();
            }

        private static Dictionary<string, Category> categories;
        public static Dictionary<string, Category> Categories
            {
            get
                {
                if (categories == null)
                    {
                    var genresList = new Category[]
                    {
                         new Category { CategoryName = "Fruit pies" },
                        new Category { CategoryName = "Custard Pies" },
                        new Category { CategoryName = "Savory pies" },
                         new Category { CategoryName = "Cream pies" },
                    };

                    categories = new Dictionary<string, Category>();

                    foreach (Category genre in genresList)
                        {
                        categories.Add(genre.CategoryName, genre);
                        }
                    }

                return categories;
                }


            }
        }
    }
