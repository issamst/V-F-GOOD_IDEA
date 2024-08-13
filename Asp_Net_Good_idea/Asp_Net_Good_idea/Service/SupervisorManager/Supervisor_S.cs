using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.supervisorManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Service.SupervisorManager
{
    public class Supervisor_S
    {
        private readonly AppDbContext _context;
        private readonly ILogger<Supervisor_S> _logger;

        public Supervisor_S(AppDbContext context, ILogger<Supervisor_S> logger)
        {
            _context = context;
            _logger = logger;
        }
       
        public async Task<List<SupervorM>> GetAllSupervisors()
        {
            return await _context.SupervorM.Include(d => d.TEID).ToListAsync();
        }

        public async Task<SupervorM> GetSupervisorById(int id)
        {
            return await _context.SupervorM.FindAsync(id);
        }

        public async Task<bool> CreateSupervisor(SupervorM supervisor)
        {
            try
            {
                supervisor.Date_Create = DateTime.Now;
                _context.SupervorM.Add(supervisor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating supervisor: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateSupervisor(int id, SupervorM supervisor)
        {
            try
            {
                var existingSupervisor = await _context.SupervorM.FirstOrDefaultAsync(x => x.Id == id);
                if (existingSupervisor != null)
                {
                    existingSupervisor.Speciality = supervisor.Speciality;
                    existingSupervisor.UserID = supervisor.UserID;
                    existingSupervisor.TEID = supervisor.TEID;
                    existingSupervisor.CommenterDisabled = supervisor.CommenterDisabled;
                    existingSupervisor.Date_Update = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating supervisor: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSupervisor(int id, string commenter)
        {
            try
            {
                var supervisor = await _context.SupervorM.FindAsync(id);
                if (supervisor != null)
                {
                    supervisor.Date_delete = DateTime.Now;
                    supervisor.CommenterDisabled = commenter;
                    _context.SupervorM.Update(supervisor);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting supervisor: {ex.Message}");
                return false;
            }
        }
    }
}
