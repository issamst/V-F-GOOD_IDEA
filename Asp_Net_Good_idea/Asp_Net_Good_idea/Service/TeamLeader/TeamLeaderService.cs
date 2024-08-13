using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.TeamLeader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Models.UserModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Asp_Net_Good_idea.Service.TeamLeader
{
    public class TeamLeaderService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TeamLeaderService> _logger;

        public TeamLeaderService(AppDbContext context, ILogger<TeamLeaderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TeamLeader_M>> GetAllTeamLeaders()
        {
            var teamLeader_M = await _context.TeamLeaders
                .Include(tl => tl.N_Area)
                .Include(tl => tl.N_Project)
                .Include(tl => tl.N_User)
                    .ThenInclude(u => u.Name_Title)
                .OrderByDescending(tl => tl.Date_Create)
                .AsNoTracking()
                .ToListAsync();

            var mappedTeamLeader_M = teamLeader_M
                .Select(tl => new TeamLeader_M
            {
                Id = tl.Id,
                Teamleader_Name = tl.Teamleader_Name,
                N_Project = tl.N_Project,
                N_Area = tl.N_Area,
                shift = tl.shift,
                N_User = tl.N_User.Select(u => new User
                {
                    Id = u.Id,
                    TEID = u.TEID,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    TitleID = u.TitleID,
                    Name_Title = u.Name_Title
                }).ToList()
            }).ToList();

            return mappedTeamLeader_M;
        }


        public async Task<TeamLeader_M> GetTeamLeaderById(int id)
        {

            return await _context.TeamLeaders
               .Include(tl => tl.N_User) // Include the navigation property correctly
               .Select(tl => new TeamLeader_M
               {
                   Id = tl.Id,
                   Teamleader_Name = tl.Teamleader_Name,
                   ProjectID = tl.ProjectID,
                   AreaID = tl.AreaID,
                   shift = tl.shift,
                   N_User = tl.N_User,
                   CommenterDelete = tl.CommenterDelete 

               })
               .FirstOrDefaultAsync(tl => tl.Id == id);

            //if (teamLeader_M == null)
            //{
            //    return NotFound();
            //}

            //return teamLeader_M;
        }

        private async Task<bool> AddTeamleader(int[] userID, int leaderId)
        {
            try
            {
                User user;
                foreach (int id in userID)
                {
                    user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                    user.LeaderId = leaderId;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding users to team leader: {ex.Message}");

                Console.WriteLine($"Error adding users to team leader: {ex.Message}");
                return false;
            }

        }
        public async Task<bool> CreateTeamLeader(TeamLeader_M teamLeader)
        {
            try
            {
                teamLeader.Date_Create = DateTime.Now;
                _context.TeamLeaders.Add(teamLeader);
                await _context.SaveChangesAsync();
                await AddTeamleader(teamLeader.UserID, teamLeader.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating team leader: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateTeamLeader(int id, TeamLeader_M teamLeader)
        {
            try
            {
                var existingTeamLeader = await _context.TeamLeaders
                    .Include(t => t.N_User)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (existingTeamLeader == null)
                {
                    return false;
                }

                existingTeamLeader.Teamleader_Name = teamLeader.Teamleader_Name;
                existingTeamLeader.ProjectID = teamLeader.ProjectID;
                existingTeamLeader.AreaID = teamLeader.AreaID;
                existingTeamLeader.shift = teamLeader.shift;
                //existingTeamLeader.CommentOnDelete = teamLeader.CommentOnDelete;
                existingTeamLeader.Date_Update = DateTime.Now;

                // Update users
                existingTeamLeader.N_User.Clear();
                if (teamLeader.UserID != null && teamLeader.UserID.Any())
                {
                    foreach (var userId in teamLeader.UserID)
                    {
                        var existingUser = await _context.Users.FindAsync(userId);
                        if (existingUser != null)
                        {
                            existingTeamLeader.N_User.Add(existingUser);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating team leader: {ex.Message}");
                return false;
            }
        }


        //public async Task<bool> DeleteTeamLeader(int id)
        //{
        //    try
        //    {
        //        var teamLeader = await _context.TeamLeaders.FindAsync(id);
        //        if (teamLeader == null)
        //        {
        //            return false;
        //        }

        //        _context.TeamLeaders.Remove(teamLeader);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error deleting team leader: {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task<bool> DeleteTeamLeader(int id, string comment)
        {
            try
            {
                var teamLeader = await _context.TeamLeaders.FindAsync(id);
                if (teamLeader != null)
                {
                    teamLeader.Date_delete = DateTime.Now;
                    teamLeader.CommenterDelete = comment;
                    _context.TeamLeaders.Update(teamLeader);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting TeamLeader: {ex.Message}");
                return false;
            }


        }


     




    }
}
