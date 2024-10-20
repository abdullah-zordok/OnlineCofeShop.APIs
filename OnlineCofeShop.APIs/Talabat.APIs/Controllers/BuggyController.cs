
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    //Front or Flutter want to watch Shape Response as Documentation to them

    //This Class => Show you shape of default Response all errors from 6 Errors 
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        //1-Show Shape(structure) of default Response of Notfound error  

        [HttpGet("notfound")]// Get : api/buggy/notfound

        public async Task<ActionResult> GetNotFoundRequest()
        {
            var product = await _dbContext.Products.FindAsync(100);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }

        //2-Show Shape of default Response of Server error(Exception)    

        [HttpGet("servererror")] //Get : api/buggy/servererror
        public async Task<ActionResult> GetServerError()
        {
            var product = await _dbContext.Products.FindAsync(100);

            var productToReturn = product.ToString(); // Will Throw  Exception [NullReferenceException]

            return Ok(productToReturn);
        }

        //3-show Shape of default Response of BadRequest error

        [HttpGet("badrequest")]

        public ActionResult GetBadRequest() //Get : api/buggy/badrequest
        {
            return BadRequest(new ApiResponse(400));

        }

        //4-show Shape of default Response of  Validation error => this is type from types of badrequest

        [HttpGet("badrequest/{id}")]    //Get : api/buggy/badrequest/five

        public ActionResult GetBadRequest(int id)  //Validation error   
        {
            return Ok();
        }


        //5-show Shape of default Response of Notfound end point 

        //Notfound end point => speak end point not found


        //6-show Shape of default Response of UnauthorizedError

        [HttpGet("unauthorized")] //Get : /api/buggy/unauthorized
        public ActionResult GetUnauthorizedError()
        {
            return Unauthorized(new ApiResponse(401));
        }


    }
}