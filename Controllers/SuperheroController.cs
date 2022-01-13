#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Superhero.Controllers
{
    ///<summary>
    /// [APIController]
    /// 
    //Attribute handles automatic HTTP 400 responses when the model is in error, 
    //and automatic binding of URL parameters to method parameters, and similar.
    ///</summary>
    [ApiController]
    
    /// <summary>
    /// [Route("api/[controller]")]
    /// 
    /// The Route attribute allows you to map a URL pattern to the controller. 
    /// In this specific case, you are mapping the api/[controller] URL pattern to the controller. 
    /// At runtime, the [controller] placeholder is replaced by the controller class name without 
    /// the Controller suffix. That means that the SuperheroController will be mapped to the 
    /// api/superhero URL, and this will be the base URL for all the actions implemented by the controller. 
    /// The Route attribute can be also applied to the methods of the controller, as you will see.
    /// 
    /// The base class, IControllerBase provides properties and methods that are useful for handling HTTP requests 
    /// </summary>
    public class SuperheroController : ControllerBase 
    {
        private static List<SuperheroItem> Superheroes = new List<SuperheroItem> {
            new SuperheroItem { Id= 1, Name = "Superman"  },
            new SuperheroItem { Id= 2, Name = "Batman"    },
            new SuperheroItem { Id= 3, Name = "Spiderman" }
        };

        /// <summary>
        /// The method Get() allows the client to get the whole list of Superhero items
        /// The HttpGet attribute which maps the method to HTTP GET requests sent to the api/superhero URL
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        /// <summary>
        /// This means that the method will return a List<SuperheroItem> type object or an object deriving 
        /// from ActionResult type. 
        /// The ActionResult type represents the HTTP status codes to be returned as a response.
        /// </summary>
        /// <returns>The return type of the method is ActionResult<List<SuperheroItem>>. </returns>    
        [Route("api/[controller]")]
        public ActionResult<List<SuperheroItem>> Get()
        {
            /// <summary>
            /// Some of these features are methods that create HTTP status code in a readable way, 
            /// like Ok(), NotFound(), BadRequest(), and so on. The Ok(Superheroes) value returned by 
            /// the Get() method represents the 200 OK HTTP status code with the representation of the 
            /// Superheroes variable as the body of the response.
            /// </summary>

            return Ok(Superheroes);
        }

        /// <summary>
        /// This method Get() allows the client to select the Superhero item
        /// The HttpGet attribute which maps the method to HTTP GET requests sent to the api/superhero URL
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/{Id}")]  //use this to avoid using ?id= but rather use ~/id      
        public ActionResult<List<SuperheroItem>> Get(int? Id)
        {
            var superheroItem = Superheroes.Find(x => x.Id == Id);
            return superheroItem == null ? NotFound() : Ok(superheroItem);
        }

        /// <summary>
        /// This action is mapped to the HTTP POST verb via the HttpPost attribute. 
        /// The Post() method has also a superheroItem parameter whose value comes from the body of 
        /// the HTTP POST request. Here, the method checks if the term to be created already exists. 
        /// If it is so, a 409 Conflict HTTP status code will be returned. Otherwise, the new item is 
        /// appended to the Superheroes list. By following the REST guidelines, the action returns 
        /// a 201 Created HTTP status code. 
        /// 
        /// The response includes the newly created item in the body and its URL in the Location HTTP header.
        /// </summary>
        /// <param name="superheroItem"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]")]
        public ActionResult Post(SuperheroItem superheroItem)
        {
            var existingSuperheroItem = Superheroes.Find(x => x.Id == superheroItem.Id);
            
            if (existingSuperheroItem != null)
            {
                return Conflict("Cannot create the Id because it already exists.");
            }
            else
            {
                Superheroes.Add(superheroItem);
                var resourceUrl = Request.Path.ToString() + "/" + superheroItem.Id;
                return Created(resourceUrl, superheroItem);
            }
        }

        /// <summary>
        /// The Put() method is decorated with the HttpPut attribute that maps it to the HTTP PUT verb. 
        /// In short, it checks if the Superhero item to be updated exists in the Superheroes list. 
        /// If the item doesn't exist, it returns a 400 Bad Request HTTP status code. Otherwise, 
        /// it updates the item's definition and returns a 200 OK HTTP status code.
        /// </summary>
        [HttpPut]
        [Route("api/[controller]")]
        public ActionResult Put(SuperheroItem superheroItem)
        {
            var existingSuperheroItem = Superheroes.Find(x => x.Id == superheroItem.Id);
            if (existingSuperheroItem == null)
            {
                return BadRequest("Cannot update a nont existing term.");
            }
            else
            {
                existingSuperheroItem.Name = superheroItem.Name;
                return Ok();
            }
        }

        /// <summary>
        /// The HttpDelete attribute maps the method Delete() to the DELETE HTTP verb. 
        /// The Route attribute appends a new element to the URL the same way you learned when 
        /// implemented the action that gets a single Superhero item. So, when a DELETE HTTP request 
        /// hits the api/superhero/{Id} URL pattern, the method checks if the item exists in the Superheroes list. 
        /// If it doesn't exist, a 404 Not Found HTTP status code is returned. Otherwise, the Delete() method 
        /// removes the item from the Superheroes list and returns a 204 No Content HTTP status code.
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/{Id}")]
        public ActionResult Delete(int? Id)
        {
            var superheroItem = Superheroes.Find(x => x.Id == Id);
            if (superheroItem == null)
            {
                return NotFound();
            }
            else
            {
                Superheroes.Remove(superheroItem);
                return NoContent();
            }
        }

    }
}