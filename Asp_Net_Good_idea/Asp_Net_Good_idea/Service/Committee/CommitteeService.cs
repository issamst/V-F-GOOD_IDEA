using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Committee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Asp_Net_Good_idea.Service.Committee
{
    public class CommitteeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CommitteeService> _logger;

        public CommitteeService(AppDbContext context, ILogger<CommitteeService> logger)
        {
            _context = context;
            _logger = logger;

        }


        public async Task<List<Committee_M>> GetAllCommittees()
        {

            var Committee = await _context.Committees
                .Include(c => c.N_Area)
                .Include(c => c.N_Plant)
                .Include(c => c.N_Departement)
                .Include(c => c.Replacement)
                .Include(c => c.Responsible)
                .Include(c => c.ResponsibleTitle) 

                .OrderByDescending(c => c.Date_Create)
                .AsNoTracking()
                .ToListAsync();
            return Committee;

        }

        public async Task<Committee_M> GetCommitteeById(int id)
        {
            return await _context.Committees.FindAsync(id);

        }

        public async Task<Committee_M> CreateCommittee(Committee_M committee)
        {
            committee.Date_Create = DateTime.Now;
            _context.Committees.Add(committee);
            await _context.SaveChangesAsync();
           
            return committee;
        }

        public async Task<Committee_M> UpdateCommittee(int id, Committee_M committee)
        {
            var existingcommittee = await _context.Committees.FirstOrDefaultAsync(x => x.Id == id);
            if (existingcommittee != null)
            {
                existingcommittee.Name_Committee = committee.Name_Committee;
                existingcommittee.PlantID = committee.PlantID;
                existingcommittee.AreaID = committee.AreaID;
                existingcommittee.DepartementID = committee.DepartementID;
                existingcommittee.ReplacementID = committee.ReplacementID;
                existingcommittee.ResponsibleID = committee.ResponsibleID;
                existingcommittee.ResponsibleTitleID = committee.ResponsibleTitleID;
                //existingcommittee.type = committee.type;
                //existingcommittee.CommenterDelete = committee.CommenterDelete;
                existingcommittee.Date_Update = DateTime.Now;
                await _context.SaveChangesAsync();
                return existingcommittee;
            }
            return null;
        }

        public async Task<bool> DeleteCommittee(int id, string comment)
        {
            try
            {
                var committee = await _context.Committees.FindAsync(id);
                if (committee != null)
                {
                    committee.Date_delete = DateTime.Now;
                    committee.CommenterDelete = comment;
                    _context.Committees.Update(committee);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting committee: {ex.Message}");
                return false;
            }

     
        }






    }
}
