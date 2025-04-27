using FormsBoard.Data.Context;
using FormsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsBoard.Data.Repositories
{
    public class MileageFormRepository : IMileageFormRepository
    {
        private readonly ApplicationDbContext _context;

        public MileageFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MileageForm>> GetAllFormsAsync()
        {
            return await _context.MileageForms
                .Include(f => f.Status)
                .Include(f => f.LineItems)
                .OrderByDescending(f => f.DateSubmitted)
                .ToListAsync();
        }

        public async Task<IEnumerable<MileageForm>> GetFormsByUserIdAsync(string userId)
        {
            return await _context.MileageForms
                .Include(f => f.Status)
                .Include(f => f.LineItems)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.DateSubmitted)
                .ToListAsync();
        }

        public async Task<IEnumerable<MileageForm>> GetFormsByStatusAsync(int statusId)
        {
            return await _context.MileageForms
                .Include(f => f.Status)
                .Include(f => f.LineItems)
                .Where(f => f.FormStatusId == statusId)
                .OrderByDescending(f => f.DateSubmitted)
                .ToListAsync();
        }

        public async Task<MileageForm> GetFormByIdAsync(int id)
        {
            return await _context.MileageForms
                .Include(f => f.Status)
                .Include(f => f.LineItems)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<MileageForm> CreateFormAsync(MileageForm form)
        {
            _context.MileageForms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<MileageForm> UpdateFormAsync(MileageForm form)
        {
            _context.Entry(form).State = EntityState.Modified;

            // Ensure line items are properly tracked
            if (form.LineItems != null)
            {
                foreach (var item in form.LineItems)
                {
                    if (item.Id == 0)
                    {
                        // New item
                        _context.MileageLineItems.Add(item);
                    }
                    else
                    {
                        // Existing item
                        _context.Entry(item).State = EntityState.Modified;
                    }
                }

                // Find and delete removed line items
                var existingItems = await _context.MileageLineItems
                    .Where(i => i.MileageFormId == form.Id)
                    .ToListAsync();

                var itemIdsToKeep = form.LineItems.Select(i => i.Id).ToList();

                foreach (var existingItem in existingItems)
                {
                    if (!itemIdsToKeep.Contains(existingItem.Id))
                    {
                        _context.MileageLineItems.Remove(existingItem);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return form;
        }

        public async Task DeleteFormAsync(int id)
        {
            var form = await _context.MileageForms.FindAsync(id);
            if (form != null)
            {
                _context.MileageForms.Remove(form);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FormStatus>> GetFormStatusesAsync()
        {
            return await _context.FormStatuses.ToListAsync();
        }
    }
}
