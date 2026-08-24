using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;

namespace Internalexamportal.Core.Services
{
    public class RollNumberingService : IRollNumberingService
    {
        private readonly InternalExamportalContext _context;

        public RollNumberingService(InternalExamportalContext context)
        {
            _context = context;
        }

        public async Task<string> GetNextRollNumber()
        {
            CandidateNumbering candidateNumbering = null;
            bool updateSuccessful = false;
            while (!updateSuccessful)
            {
                try
                {
                    candidateNumbering = await _context.CandidateNumbering.FindAsync(1);
                    candidateNumbering.Count++;
                    await _context.SaveChangesAsync();
                    updateSuccessful = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    _context.Entry(candidateNumbering).State = EntityState.Detached;
                }
            }
            return $"{candidateNumbering.Count.ToString("0000.##")}";
        }
        public async Task<bool> RevertRollNumberCount()
        {
            CandidateNumbering candidateNumbering = null;
            bool updateSuccessful = false;
            while (!updateSuccessful)
            {
                try
                {
                    candidateNumbering = await _context.CandidateNumbering.FindAsync(1);
                    candidateNumbering.Count--;
                    await _context.SaveChangesAsync();
                    updateSuccessful = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    _context.Entry(candidateNumbering).State = EntityState.Detached;
                }
            }
            return updateSuccessful;
        }
    }
}
