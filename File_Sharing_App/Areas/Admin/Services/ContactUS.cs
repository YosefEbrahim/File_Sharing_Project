using AutoMapper;
using AutoMapper.QueryableExtensions;
using File_Sharing_App.Areas.Admin.Models;
using File_Sharing_App.Data;

namespace File_Sharing_App.Areas.Admin.Services
{
    public class ContactUS : IContactUS
    {
        private readonly ApplicationDbcontext _context;
        private readonly IMapper _mapper;

        public ContactUS(ApplicationDbcontext context,IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;

        }

        public async Task ChangeStatusAysnc(int Id, bool IsClosed)
        {
            var selectedItem= await _context.Contact.FindAsync(Id);
            if(selectedItem != null)
            {
                selectedItem.IsClosed = IsClosed;
                _context.Update(selectedItem);
                 await _context.SaveChangesAsync();
            }
                
        }

        public IQueryable<ContactUSViewModel> GetAll()
        {
            var result = _context.Contact.ProjectTo<ContactUSViewModel>(_mapper.ConfigurationProvider);
            return result;
        }
    }
}
