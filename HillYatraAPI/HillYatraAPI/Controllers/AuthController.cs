using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using HillYatraAPI.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HillYatraAPI.Controllers
{
    [Produces("application/json")]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return ("[value:4]");
        }
        private IRepositoryWrapper _repoWrapper;
        private readonly RepositoryContext _context;
        private readonly ILoggerManager _logger;
        public AuthController(RepositoryContext context)
        {
            _context = context;
        }
        public class LoginResponse
        {
            public string Message { get; set; }
            public bool IsSuccess { get; set; }
        }
        public class JwtPacket
        {
            public string ID { get; set; }
            public string Token { get; set; }
            public string FirstName { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; }
        }
        public class LoginData
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpPost("register")]
        public JwtPacket Register(User user)
        {
            return CreateJwtPacket(user);
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginData loginData)
        {
            LoginResponse res = new LoginResponse();
            try
            {
                //if(!IsAuthenticated("LDAP://golon9dcm01.global.loc", loginData.Email,loginData.Password))
                // return NotFound("email or password incorrect");
                //var user = _context.User.Include(a => a.UserCityCe).ThenInclude(b => b.CityCodeNavigation).
                //    Include(a => a.UserRole).Include(a => a.UserRoleCe).ThenInclude(b => b.Role).SingleOrDefault(u => u.Email == loginData.Email
                //      && u.Password == loginData.Password
                //     && u.IsActive == true
                // );
                var user = _context.User.Include(a => a.UserType).SingleOrDefault(u => u.Email == loginData.Email
               && u.Password == loginData.Password
              &&   u.IsActive == 1
          );
                if (user == null)
                {
                    res.IsSuccess = false;
                    res.Message = "successfully logged in";
                    return Ok(CreateJwtPacket(null));
                }
                return Ok(CreateJwtPacket(user));
            }
            catch (Exception ex)
            {
                return Ok(CreateJwtPacket(null));
            }
        }
        JwtPacket CreateJwtPacket(User user)
        {
            var jwtResponse = new JwtPacket();
            if (user != null)
            {
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mykeymykeymykeymykey"));
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
               // List<string> listRoles = new List<string>();
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
                claims.AddClaim(new Claim("UserName", user.UserName));
                claims.AddClaim(new Claim("Email", user.Email));
                claims.AddClaim(new Claim("ID", user.Id.ToString()));
                claims.AddClaim(new Claim("Role", user.UserType.Description));
                //var cities = user.UserCityCe.Select(a => a.CityCode).ToList();
                //foreach (var ciity in cities)
                //{
                //    claims.AddClaim(new Claim(ClaimTypes.StateOrProvince, ciity));
                //}
                //TODO: later add roles and uncomment below lines
                //foreach (var role in user.UserRoleCe)
                //{
                //    claims.AddClaim(new Claim(ClaimTypes.Role, role.Role.RoleName));
                //    listRoles.Add(role.Role.RoleName);
                //    // var jwt = new JwtSecurityToken(claims: claims.Claims, signingCredentials: signingCredentials);
                //    // var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                //    //tt beta for testing jwtResponse = new JwtPacket { ID = user.Id.ToString(), Token = encodedJwt, FirstName = user.UserName, Role = user.UserRole.RoleName, Roles = listRoles };
                //}
                var jwt = new JwtSecurityToken(claims: claims.Claims, signingCredentials: signingCredentials);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                //jwtResponse = new JwtPacket { ID = user.Id.ToString(), Token = encodedJwt, FirstName = user.UserName, Roles = listRoles };
                jwtResponse = new JwtPacket { ID = user.Id.ToString(), Token = encodedJwt, FirstName = user.UserName, Role = user.UserType.Title };
            }
            return jwtResponse;
        }
        #region ldap functions
        public bool ValidateUidPwdAndGetUserTypeGlobal(string TPXId, string password)
        {
            string strADPath = "LDAP://a.b.c/dc=a,dc=b,dc=c";
            try
            {
                DirectoryEntry objDirEntry = new DirectoryEntry(strADPath, TPXId, password);
                DirectorySearcher search = new DirectorySearcher(objDirEntry);
                search.Filter = "(samaccountname=" + TPXId + ")";
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool IsAuthenticated(string ldap, string usr, string pwd)
        {
            bool authenticated = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ldap, usr, pwd);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                Console.WriteLine(cex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return authenticated;
        }
        #endregion
    }
}

public class MyClass
{
    private MyClass()
    {
        //some non client related initialization
     } // Private constructor
    public MyClass(string inStr) : this() { }
}

public class MyClass2 : MyClass // Inherited from MyClass
{
    public MyClass2(string inStr) : base(inStr) { }
}