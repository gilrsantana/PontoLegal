using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly PontoLegalContext _context;

        public JobPositionRepository(PontoLegalContext context)
        {
            _context = context;
        }

        public async Task<JobPosition?> GetJobPositionByIdAsync(Guid id)
        {
            return await _context
                .JobPositions
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<JobPosition?> GetJobPositionByNameAsync(string modelName)
        {
            return await _context
                .JobPositions
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Name == modelName);
        }

        //public async Task<JobPosition?> GetJobPositionByNameIncludeDepartmentAsync(string modelName)
        //{
        //    return await _context
        //        .JobPositions
        //        .AsNoTracking()
        //        .Include(j => j.Department)
        //        .FirstOrDefaultAsync(j => j.Name == modelName);
        //}

        //public async Task<JobPosition?> GetJobPositionByIdIncludeDepartmentAsync(Guid id)
        //{
        //    return await _context
        //        .JobPositions
        //        .AsNoTracking()
        //        .Include(j => j.Department)
        //        .FirstOrDefaultAsync(j => j.Id == id);
        //}

        public async Task<ICollection<JobPosition>?> GetAllJobPositionsAsync(int skip = 0, int take = 25)
        {
            return await _context
                .JobPositions
                .AsNoTracking()
                .OrderBy(j => j.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<bool> AddJobPositionAsync(JobPosition jobPosition)
        {
            _context.JobPositions.Add(jobPosition);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateJobPositionAsync(JobPosition jobPosition)
        {
            _context.JobPositions.Update(jobPosition);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveJobPositionAsync(JobPosition jobPosition)
        {
            _context.JobPositions.Remove(jobPosition);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<JobPosition>?> GetJobPositionByDepartmentIdAsync(Guid departmentId)
        {
            return await _context
                .JobPositions
                .AsNoTracking()
                .OrderBy(j => j.Name)
                .Where(j => j.DepartmentId == departmentId)
                .ToListAsync();
        }
    }
}
