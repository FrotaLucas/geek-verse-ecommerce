using GeekVerse.Server.Services.AuthService;

namespace GeekVerse.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly IAuthService _authService;
        private readonly DataContext _context;
        public AddressService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddress()).Data;

            if( dbAddress == null)
            {
                address.UserId = _authService.GetUserId();
                _context.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.Street = address.Street;
                dbAddress.City = address.City;
                dbAddress.State = address.State;
                dbAddress.Zip = address.Zip;
                dbAddress.Country = address.Country;    
                //return to frontend when dbAddress != null
                response.Data = dbAddress;
            }

            await _context.SaveChangesAsync();

            return response;

        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
           int userId = _authService.GetUserId();
           var address = await _context.Addresses.FirstOrDefaultAsync( a => a.UserId == userId );

            return new ServiceResponse<Address>
            {
                Data = address
            };
        }
    }
}
