using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.TeamLeader;
using Asp_Net_Good_idea.Models.UserModel;
using Asp_Net_Good_idea.Service.TeamLeader;
using Asp_Net_Good_idea.Service.Committee;

namespace Asp_Net_Good_idea.Controllers.TeamLeader
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamLeadersController : ControllerBase
    {
        private readonly TeamLeaderService _teamLeaderService;
        private readonly ILogger<TeamLeadersController> _logger;


        public TeamLeadersController(TeamLeaderService teamLeaderService, ILogger<TeamLeadersController> logger)
        {
            _teamLeaderService = teamLeaderService;
            _logger = logger;
        }

       
        //NewGet

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamLeader_M>>> GetAllTeamLeaders()
        {
            try
            {
                var teamLeader_M = await _teamLeaderService.GetAllTeamLeaders();
                return Ok(teamLeader_M);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving team leaders: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }


        //OldGet

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TeamLeader_M>>> GetAllTeamLeaders()
        //{
        //    var teamLeader_M = await _context.TeamLeaders
        //.Include(tl => tl.N_Area)
        //.Include(tl => tl.N_Project)
        //.Include(tl => tl.N_User)
        //            .ThenInclude(u => u.Name_Title) // Ensure User_Title is included

        //.OrderByDescending(tl => tl.Date_Create)
        //.AsNoTracking()
        //.ToListAsync();

        //    // Project to TeamLeader_M after loading N_User
        //    var mappedTeamLeader_M = teamLeader_M
        //        .Select(tl => new TeamLeader_M
        //        {
        //            Id = tl.Id,
        //            Teamleader_Name = tl.Teamleader_Name,
        //            N_Project = tl.N_Project,
        //            N_Area = tl.N_Area,
        //            shift = tl.shift,
        //            // Modify this part to load N_User properly
        //            N_User = tl.N_User.Select(u => new User
        //            {
        //                Id = u.Id,
        //                TEID = u.TEID,
        //                FirstName = u.FirstName,
        //                LastName = u.LastName,
        //                FullName = u.FullName,
        //                Email = u.Email,
        //                Phone = u.Phone,
        //                TitleID = u.TitleID,
        //                Name_Title = u.Name_Title,

        //            }).ToList()
        //        });

        //    return Ok(mappedTeamLeader_M);
        //    //var teamLeader_M = await _context.TeamLeaders
        //    //  .Include(tl => tl.N_Area)
        //    //  .Include(tl => tl.N_Project)
        //    //  .Include(tl => tl.N_User)
        //    //    .Select(tl => new TeamLeader_M
        //    //    {
        //    //        Id = tl.Id,
        //    //        Teamleader_Name = tl.Teamleader_Name,
        //    //        ProjectID = tl.ProjectID,
        //    //        AreaID = tl.AreaID,
        //    //        shift = tl.shift,
        //    //        N_User = tl.N_User
        //    //    })
        //    //  .OrderByDescending(tl => tl.Date_Create)
        //    //  .AsNoTracking()
        //    //  .ToListAsync();
        //    //return teamLeader_M;

        //}

        //NewGet By id

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamLeader_M>> GetTeamLeaderById(int id)
        {
            var teamLeader = await _teamLeaderService.GetTeamLeaderById(id);
            if (teamLeader == null)
            {
                return NotFound();
            }
            return Ok(teamLeader);
        }

        //oldGet BY id


        //[HttpGet("{id}")]
        //public async Task<ActionResult<TeamLeader_M>> GetTeamLeader_M(int id)
        //{
        //    //var teamLeader_M = await _context.TeamLeaders.FindAsync(id);

        //    var teamLeader_M = await _context.TeamLeaders
        //       .Include(tl => tl.N_User) // Include the navigation property correctly
        //       .Select(tl => new TeamLeader_M
        //       {
        //           Id = tl.Id,
        //           Teamleader_Name = tl.Teamleader_Name,
        //           ProjectID = tl.ProjectID,
        //           AreaID = tl.AreaID,
        //           shift = tl.shift,
        //           N_User = tl.N_User
        //       })
        //       .FirstOrDefaultAsync(tl => tl.Id == id);

        //    if (teamLeader_M == null)
        //    {
        //        return NotFound();
        //    }

        //    return teamLeader_M;
        //}

        //New Update Put

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamLeader(int id, [FromBody] TeamLeader_M teamLeader)
        {
            try
            {
                var success = await _teamLeaderService.UpdateTeamLeader(id, teamLeader);
                if (!success)
                {
                    return StatusCode(500, "An error occurred while updating the team leader.");
                }

                return Ok(new { Message = "Team leader updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating team leader: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }



        //Old UPDATE Put

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTeamLeader_M(int id, TeamLeader_M teamLeader_M)
        //{
        //    try
        //    {
        //        var existingTeamLeader_M = await _context.TeamLeaders
        //                                                  .Include(t => t.N_User) // Include related users
        //                                                  .FirstOrDefaultAsync(x => x.Id == id);

        //        if (existingTeamLeader_M == null)
        //        {
        //            return NotFound(new { Message = "TeamLeader not found" });
        //        }

        //        existingTeamLeader_M.Teamleader_Name = teamLeader_M.Teamleader_Name;
        //        existingTeamLeader_M.N_Project = teamLeader_M.N_Project;
        //        existingTeamLeader_M.N_Area = teamLeader_M.N_Area;
        //        existingTeamLeader_M.shift = teamLeader_M.shift;
        //        existingTeamLeader_M.CommentOnDelete = teamLeader_M.CommentOnDelete;
        //        existingTeamLeader_M.Date_Update = DateTime.Now;

        //        // Update users
        //        existingTeamLeader_M.N_User.Clear();
        //        if (teamLeader_M.UserID != null && teamLeader_M.UserID.Any())
        //        {
        //            foreach (var userId in teamLeader_M.UserID)
        //            {
        //                var existingUser = await _context.Users.FindAsync(userId);
        //                if (existingUser != null)
        //                {
        //                    existingTeamLeader_M.N_User.Add(existingUser);
        //                }
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        return Ok(new { Message = "TeamLeader updated successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error updating TeamLeader: {ex.Message}");
        //        return StatusCode(500, new { Message = "An error occurred while updating the TeamLeader", Details = ex.Message });
        //    }
        //}





        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTeamLeader_M(int id, TeamLeader_M teamLeader_M)
        //{
        //    if (id != teamLeader_M.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(teamLeader_M).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TeamLeader_MExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        //New Post
        [HttpPost]
        public async Task<ActionResult<TeamLeader_M>> CreateTeamLeader([FromBody] TeamLeader_M teamLeader)
        {
            try
            {
                bool isCreated = await _teamLeaderService.CreateTeamLeader(teamLeader);
                if (!isCreated)
                {
                    return StatusCode(500, "An error occurred while creating the team leader.");
                }

                return CreatedAtAction(nameof(GetTeamLeaderById), new { id = teamLeader.Id }, teamLeader);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating team leader: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Old Post

        //[HttpPost]
        //public async Task<ActionResult<TeamLeader_M>> PostTeamLeader_M(TeamLeader_M teamLeader_M)
        //{
        //    teamLeader_M.Date_Create = DateTime.Now;

        //    _context.TeamLeaders.Add(teamLeader_M);
        //    await _context.SaveChangesAsync();

        //    await AddTeamleader(teamLeader_M.UserID, teamLeader_M.Id);

        //    return CreatedAtAction("GetTeamLeader_M", new { id = teamLeader_M.Id }, teamLeader_M);
        //}


        //New Delete 


        [HttpDelete("deleteTeamleader/{id}/{comment}")]
        public async Task<IActionResult> DeleteTeamLeader(int id, string comment)
        {
            bool isDeleted = await _teamLeaderService.DeleteTeamLeader(id, comment);
            if (isDeleted)
            {
                return Ok(new { Message = "Team leader deleted successfully" });
            }
            else
            {
                return NotFound("Team leader not found");
            }
        }


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTeamLeader(int id)
        //{
        //    try
        //    {
        //        var success = await _teamLeaderService.DeleteTeamLeader(id);
        //        if (!success)
        //        {
        //            return NotFound("Team leader not found");
        //        }
        //        return Ok(new { Message = "Team leader deleted successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error deleting team leader: {ex.Message}");
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

    }



}
