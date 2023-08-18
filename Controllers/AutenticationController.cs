using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using journalApi.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using journalapi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using journalapi.Models;
using journalapi.Services;
using journalapi.Controllers;

namespace journalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticationController : ControllerBase
    {

        private readonly string secretKey;
        private readonly JournalContext journalContext;
        IReasearcherService researcherService;
        private readonly ILogger<ResearcherController> _logger;

        public AutenticationController(
            IConfiguration config,
            JournalContext context,
            IReasearcherService service,
            ILogger<ResearcherController> logger
            )
        {
            _logger = logger;
            researcherService = service;
            journalContext = context;
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        /// <summary>
        /// Validates a researcher's credentials and generates a JWT token if valid.
        /// </summary>
        /// <param name="request">The researcher's credentials to validate.</param>
        /// <returns>
        /// - 200 OK with a JWT token if the credentials are valid.
        /// - 401 Unauthorized if the credentials are invalid or the researcher does not exist.
        /// </returns>
        [HttpPost]
        [Route("validate")]
        public IActionResult Validate([FromBody] Researcher request)
        {
            var current = journalContext.Researchers.Where(p => p.Email == request.Email).FirstOrDefault();
            if (current == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }

            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(current.Password);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string realPasword = new String(decoded_char);

            if (realPasword != request.Password)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
            else
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();


                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Email));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreated = tokenHandler.WriteToken(tokenConfig);
                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreated });

            }


        }

        /// <summary>
        /// Creates a new researcher.
        /// </summary>
        /// <param name="researcher">The researcher to create.</param>
        /// <returns>200 OK if successful.</returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] Researcher researcher)
        {
            try
            {
                var result = await researcherService.Create(researcher);
                return Ok(result);
            }
            catch (ResearcherServiceException ex)
            {
                _logger.LogError(ex, "Error while creating researcher.");
                return StatusCode(500, "An error occurred while creating researcher.");
            }
        }


    }
}
