using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Committee;
using System.Data;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.UserModel;
using Asp_Net_Good_idea.Service.Committee;
using Asp_Net_Good_idea.Service.Area;

namespace Asp_Net_Good_idea.Controllers.CommitteController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteesController : ControllerBase
    {
        private readonly CommitteeService _committeeService;

        public CommitteesController(CommitteeService committeeService)
        {
            _committeeService = committeeService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Committee_M>>> GetAllCommittees()
        {
            var committees = await _committeeService.GetAllCommittees();
            return Ok(committees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Committee_M>> GetCommitteeById(int id)
        {
            var committee = await _committeeService.GetCommitteeById(id);
               
            if (committee == null)
            {
                return NotFound();
            }
            return committee;
        }


        [HttpPost]
        public async Task<ActionResult<Committee_M>> CreateCommittee(Committee_M committee)
        {
            var createdCommittee = await _committeeService.CreateCommittee(committee);
            return CreatedAtAction(nameof(GetCommitteeById), new { id = createdCommittee.Id }, createdCommittee);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommittee(int id, Committee_M committee)
        {
            var updatedCommittee = await _committeeService.UpdateCommittee(id, committee);
            if (updatedCommittee == null)
            {
                return NotFound("Committee not found");
            }
            return Ok(new { Message = "Committee updated successfully" });
        }


    


        [HttpDelete("deleteCommittee/{id}/{comment}")]
        public async Task<IActionResult> DeleteCommittee(int id, string comment)
        {
            bool isDeleted = await _committeeService.DeleteCommittee(id, comment);
            if (isDeleted)
            {
                return Ok(new { Message = "Committee deleted successfully" });
            }
            else
            {
                return NotFound("Committee not found");
            }
        }


    }
}
