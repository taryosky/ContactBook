using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using ContactBook.Data;
using ContactBook.DTO;
using ContactBook.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserManager<User> _userManager { get; }

        private ContactBookContext _context { get; set; }

        public IConfiguration _config { get; }

        private Cloudinary _cloudinary;

        public UserController(UserManager<User> userManager, ContactBookContext context, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
            Account account = new Account
            {
                Cloud = config.GetSection("CloudinarySettings:CloudName").Value,
                ApiKey = config.GetSection("CloudinarySettings:ApiKey").Value,
                ApiSecret = config.GetSection("CloudinarySettings:ApiSecret").Value,
            };
            _cloudinary = new Cloudinary(account);
        }

        [HttpGet]
        [Route("[controller]/All-users")]
        public async Task<IActionResult> GetAlloUsers([FromQuery] int page)
        {
            var result = _context.Users.Skip(page * 10).Take(10).Include(x => x.Address).ToList();
            List<UserDetailsToReturnDTO> Users = new List<UserDetailsToReturnDTO>();
            foreach (var user in result)
            {
                UserDetailsToReturnDTO dto = new UserDetailsToReturnDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };

                AddressDTO adddrDto = new AddressDTO
                {
                    City = user.Address.City,
                    State = user.Address.State,
                    HouseNumber = user.Address.HouseNumber,
                    Street = user.Address.Street,
                    Country = user.Address.Country
                };

                dto.Address = adddrDto;

                Users.Add(dto);
            }
            await Task.CompletedTask;
            return Ok(Users);
        }

        [HttpGet]
        [Route("[controller]/{IdOrEmail}")]
        public async Task<IActionResult> GetUser([FromRoute] string IdOrEmail)
        {
            if (string.IsNullOrWhiteSpace(IdOrEmail)) return BadRequest();
            var IdEmail = IdOrEmail;

            IQueryable<User> Users = _context.Users.Include(x => x.Address);
            User user = null;
            if (IdEmail.Contains("@"))
            {
                user = await Users.FirstOrDefaultAsync(x => x.Email == IdOrEmail);
            }
            else
            {
                user = await Users.FirstOrDefaultAsync(x => x.Id == IdOrEmail);
            }

            if (user == null)
            {
                return NotFound();
            }

            AddressDTO Address = new AddressDTO();
            Address.State = user.Address.State;
            Address.City = user.Address.City;
            Address.Street = user.Address.Street;
            Address.Country = user.Address.Country;
            Address.HouseNumber = user.Address.HouseNumber;

            UserDetailsToReturnDTO dto = new UserDetailsToReturnDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = Address
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("[controller]/search")]
        public async Task<ActionResult<List<User>>> SearchUsers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest();

            var usersFromdb = await _context.Users.Include(x => x.Address).Where(x => (x.FirstName.Contains(query) || x.LastName.Contains(query))).ToListAsync();
            var usersToReturn = new List<UserDetailsToReturnDTO>();
            foreach (var u in usersFromdb)
            {
                AddressDTO add = new AddressDTO
                {
                    City = u.Address.City,
                    Country = u.Address.Country,
                    HouseNumber = u.Address.HouseNumber,
                    State = u.Address.State,
                    Street = u.Address.Street,
                };
                UserDetailsToReturnDTO us = new UserDetailsToReturnDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = add
                };

                usersToReturn.Add(us);
            }
            //await Task.CompletedTask;
            return Ok(usersToReturn);
        }

        [HttpPost]
        [Route("[controller]/add-new")]
        public async Task<IActionResult> AddUser([FromForm] RegistrationDetailsDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Address == null)
            {
                ModelState.AddModelError("Address", "Must contain at least one address");
                return BadRequest(ModelState);
            }

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest();

            Address address = new Address
            {
                City = model.Address.City,
                State = model.Address.State,
                Country = model.Address.Country,
                UserId = user.Id,
                Street = model.Address.Street,
                HouseNumber = model.Address.HouseNumber
            };
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GeUser", new { IdOrEmail = user.Email }, user);
        }

        [HttpDelete]
        [Route("[controller]/delete/{Id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstAsync(x => x.Id == Id);
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == Id);

            if (user == null && address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("[controller]/update/{Id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string Id, [FromForm] UserDetailsUpdateDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (Id != model.Id) return BadRequest();
            var user = await _context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null) return NotFound(model);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;
            user.Address.State = model.Address.State;
            user.Address.City = model.Address.City;
            user.Address.Street = model.Address.Street;
            user.Address.Country = model.Address.Country;
            user.Address.HouseNumber = model.Address.HouseNumber;

            _context.Users.Update(user);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [Route("[controller]/upload/{UserId}")]
        public async Task<IActionResult> UploadPhoto([FromRoute] string UserId, [FromForm] FileToUploadDTO file)
        {
            if (string.IsNullOrWhiteSpace(UserId) || file == null) return BadRequest();
            if (file.File.Length <= 0) return BadRequest();
            var user = await _context.Users.FindAsync(UserId);
            if (user == null) return BadRequest();

            var uploadResult = new ImageUploadResult();
            using (var img = file.File.OpenReadStream())
            {
                var imageParams = new ImageUploadParams
                {
                    File = new FileDescription(user.Id, img),
                    Transformation = new Transformation().Width(400).Height(400).Gravity("face").Crop("fill")
                };
                uploadResult = _cloudinary.Upload(imageParams);
            }

            Photo photo = new Photo
            {
                UserId = user.Id,
                ImageUrl = uploadResult.Url.ToString(),
                PublicKey = uploadResult.PublicId
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            return Ok(new { Url = uploadResult.Url.ToString() });
        }
    }
}
