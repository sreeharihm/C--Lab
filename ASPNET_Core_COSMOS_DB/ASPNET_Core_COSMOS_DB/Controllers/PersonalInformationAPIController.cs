using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_Core_COSMOS_DB.Models;
using ASPNET_Core_COSMOS_DB.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_Core_COSMOS_DB.Controllers
{
    [Produces("application/json")]
    [Route("api/PersonalInformationAPI")]
    [EnableCors("AllowAll")]
    public class PersonalInformationAPIController : Controller
    {
        IDbCollectionOperationsRepository<PersonalInformationModel,string> _repo;

        public PersonalInformationAPIController(IDbCollectionOperationsRepository<PersonalInformationModel, string> r)
        {
            _repo = r;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonalInformationModel>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Person/All")]
        public IActionResult Get()
        {
            var person = _repo.GetItemsFromCollectionAsync().Result;
            return Ok(person);
        }
        [HttpGet]
        [ProducesResponseType(typeof(PersonalInformationModel),200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Person/{id}")]
        public IActionResult Get(string id)
        {
            var person = _repo.GetItemFromCollectionAsync(id).Result;
            return Ok(person);
        }
        [HttpPost]
        [ProducesResponseType(typeof(PersonalInformationModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Person/Create")]
        public IActionResult Post([FromBody]PersonalInformationModel per)
        {
           var person= _repo.AddDocumentIntoCollectionAsync(per).Result;
            return Ok(person);
        }
        [HttpPut]
        [ProducesResponseType(typeof(PersonalInformationModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Person/Update/{id}")]
        public IActionResult Put(string id, [FromBody]PersonalInformationModel per)
        {
            var person = _repo.UpdateDocumentFromCollection(id, per).Result;
            return Ok(person);
        }
        [HttpDelete]
        [ProducesResponseType(typeof(PersonalInformationModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Person/Delete/{id}")]
        public IActionResult Delete(string id)
        {
            var res = _repo.DeleteDocumentFromCollectionAsync(id);
            return Ok(res.Status);
        }
    }
}